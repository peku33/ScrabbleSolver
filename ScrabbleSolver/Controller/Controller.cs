using System;
using System.Collections.Concurrent;
using ScrabbleSolver.Events;
using ScrabbleSolver.Model;
using ScrabbleSolver.Model.Player;
using System.Collections.Generic;
using ScrabbleSolver.Board;

namespace ScrabbleSolver.Controller
{
	public class Controller
	{
		private readonly Model.Model GameModel;
		private readonly GameForm GameForm;
		private readonly BlockingCollection<ApplicationEvent> ViewEvents;
		private readonly Dictionary<System.Type, Strategy> Strategies;

		public Controller(Model.Model Model, GameForm GameForm, BlockingCollection<ApplicationEvent> ViewEvents)
		{
			this.GameModel = Model;
			this.GameForm = GameForm;
			this.ViewEvents = ViewEvents;
			this.Strategies = new Dictionary<System.Type, Strategy>();
		}

		public void AddStrategies()
		{
			Strategies.Add(typeof(UpdateViewEvent), new PlayerMoveStrategy(this));
		}

		public void Start()
		{
			GameModel.InitPlayers(new AIPlayer(GameModel), new AIPlayer(GameModel), null, null);
			GameModel.TestDisplay();

			int PlayerIndex = 0; 
			while(!GameModel.IsEnd())
			{
				GameModel.SetCurrentPlayer(PlayerIndex);

				if(GameModel.IsHumanTurn())
				{
					Strategy EventStrategy;
					ApplicationEvent Event = ViewEvents.Take();

					Strategies.TryGetValue(Event.GetType(), out EventStrategy);

					if(EventStrategy != null)
					{
						if(EventStrategy.Execute(Event)) //Jesli ruch odbyl sie zgodnie z zasadami to przechodzimy do nastepnego gracza, jesli nie to powtarzamy ture
						{
							++PlayerIndex;
						}
					}
				}
				else
				{
					++PlayerIndex;
					GameModel.NextAITurn();
				}

				GameModel.TestDisplay(); //Konsolowe wyswietlanie stanu gry na potrzeby testow
										 //	GameForm.UptadeForm(); //TODO add parameters

				PlayerIndex %= GameModel.GetPlayersNumber();
			}


			Console.ReadLine(); //oczekiwanie na enter, zeby gra nie zamykala sie automatycznie - na potrzeby testow
		}

		public Model.Model GetModel()
		{
			return GameModel;
		}

		abstract class Strategy
		{
			protected Controller Parent;

			protected Strategy(Controller Parent)
			{
				this.Parent = Parent;
			}
			/// <summary>
			/// Wykonanie ruchu
			/// </summary>
			/// <param name="Event"></param>
			/// <returns>true jesli ruch odbyl sie poprawnie</returns>
			public abstract bool Execute(ApplicationEvent Event);
		}

		class PlayerMoveStrategy : Strategy
		{
			public PlayerMoveStrategy(Controller Parent) : base(Parent) {}

			public override bool Execute(ApplicationEvent Event)
			{
				return Parent.GetModel().GetCurrentPlayer().MakeMove(Event as PutWordEvent);
			}
		}

		class ChangeTileStrategy : Strategy
		{
			public ChangeTileStrategy(Controller Parent) : base(Parent){}

			public override bool Execute(ApplicationEvent Event)
			{
				Parent.GetModel().GetCurrentPlayer().ReplaceTile(Event as ReplaceTileEvent);
				return true;
			}
		}

		class PassStrategy : Strategy
		{
			public PassStrategy(Controller Parent) : base(Parent){}

			public override bool Execute(ApplicationEvent Event)
			{
				Parent.GetModel().GetCurrentPlayer().Pass();
				return true;
			}
		}
	}
}
