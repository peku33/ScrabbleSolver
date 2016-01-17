using System;
using System.Collections.Concurrent;
using ScrabbleSolver.Events;
using ScrabbleSolver.Model;
using ScrabbleSolver.Model.Player;
using System.Collections.Generic;
using ScrabbleSolver.Board;
using ScrabbleSolver.Common;

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

		/// <summary>
		/// Dodanie strategii do mapy
		/// </summary>
		public void AddStrategies()
		{
			//Strategies.Add(typeof(UpdateViewEvent), new PutWordStrategy(this));
			Strategies.Add(typeof(PassEvent), new PassStrategy(this));
			Strategies.Add(typeof(PutWordEvent), new PutWordStrategy(this));
			Strategies.Add(typeof(ReplaceTileEvent), new ReplaceTileStrategy(this));
			Strategies.Add(typeof(NewGameEvent), new NewGameStrategy(this));
		}

		public void Start()
		{
			while(true)
			{
				GameModel.TestDisplay();
				int PlayerIndex = 0;

				if(GameModel.GetPlayersNumber() == 0) //Zaden gracz nie gra, wiec gra stoi dopoki nie otrzyma zdarzenia nowej gry
				{
					PlayerIndex = 0;
					Strategy EventStrategy;
					ApplicationEvent Event;

					do
					{
						Event = ViewEvents.Take();
					} while(Event.GetType() != typeof(NewGameEvent));

					Strategies.TryGetValue(Event.GetType(), out EventStrategy);
					EventStrategy.Execute(Event);
					continue;
				}

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
							if(EventStrategy.Execute(Event))
							//Jesli ruch odbyl sie zgodnie z zasadami to przechodzimy do nastepnego gracza, jesli nie to powtarzamy ture
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
				
				//TODO - dojscie do tego miejsca programu oznacza, ze gra sie skonczyla - trzeba wywolac z widoku funkcje, ktora podkresli kto wygral
			}
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

			public abstract bool Execute(ApplicationEvent Event);
		}

		class PutWordStrategy : Strategy
		{
			public PutWordStrategy(Controller Parent) : base(Parent)
			{
			}

			public override bool Execute(ApplicationEvent Event)
			{
				return Parent.GetModel().GetCurrentPlayer().MakeMove(Event as PutWordEvent);
			}
		}

		class ReplaceTileStrategy : Strategy
		{
			public ReplaceTileStrategy(Controller Parent) : base(Parent)
			{
			}

			public override bool Execute(ApplicationEvent Event)
			{
				Parent.GetModel().GetCurrentPlayer().ReplaceTile(Event as ReplaceTileEvent);
				return true;
			}
		}

		class PassStrategy : Strategy
		{
			public PassStrategy(Controller Parent) : base(Parent)
			{
			}

			public override bool Execute(ApplicationEvent Event)
			{
				Parent.GetModel().GetCurrentPlayer().Pass();
				return true;
			}
		}

		class NewGameStrategy : Strategy
		{
			public NewGameStrategy(Controller Parent) : base(Parent)
			{
			}

			public override bool Execute(ApplicationEvent Event)
			{
				NewGameEvent NewGameEvent = Event as NewGameEvent;

				if(NewGameEvent == null)
				{
					return false;
				}

				Parent.GetModel().ResetGame();

				List<Player> Players = new List<Player>();
				List<Tuple<bool, bool>> PlayerInfoList = new List<Tuple<bool, bool>>();

				foreach(PlayerIdEnum PlayerId in Enum.GetValues(typeof(PlayerIdEnum)))
				{
					PlayerInfoList.Add(NewGameEvent.GetPlayerInfo(PlayerId));
				}

				foreach(Tuple<bool, bool> PlayerInfo in PlayerInfoList)
				{
					bool ShouldAddPlayer = PlayerInfo.Item1;
					bool isComputer = PlayerInfo.Item2;
					int PlayerIndex = 1;
					if(ShouldAddPlayer)
					{
						if(isComputer)
						{
							Parent.GetModel().AddPlayer(new AIPlayer(Parent.GetModel()));
						}
						else
						{
							Parent.GetModel().AddPlayer(new HumanPlayer("Player" + PlayerIndex++, Parent.GetModel()));
						}
					}
				}

				return true;
			}
		}
	}
}
