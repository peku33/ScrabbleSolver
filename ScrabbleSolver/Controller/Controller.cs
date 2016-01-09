using System;
using System.CodeDom;
using System.Collections.Concurrent;
using ScrabbleSolver.Events;
using ScrabbleSolver.Model;
using ScrabbleSolver.Model.Player;
using System.Collections.Generic;

namespace ScrabbleSolver.Controller
{
	public class Controller
	{
		private readonly Model.Model GameModel;
		private readonly GameForm GameForm;
		private readonly BlockingCollection<ApplicationEvent> ViewEvents;
		private readonly Dictionary<System.Type, Strategy> Strategy;

		public Controller(Model.Model Model, GameForm GameForm, BlockingCollection<ApplicationEvent> ViewEvents)
		{
			this.GameModel = Model;
			this.GameForm = GameForm;
			this.ViewEvents = ViewEvents;
			this.Strategy = new Dictionary<System.Type, Strategy>();

			Strategy.Add(typeof(UpdateViewEvent), new PlayerMoveStrategy());
		}

		public void Start()
		{
			GameModel.InitPlayers(new AIPlayer(GameModel), new AIPlayer(GameModel), null, null);
			GameModel.TestDisplay();

			int i = 0;
			while(!GameModel.IsEnd())
			{
				GameModel.SetCurrentPlayer(i);

				if(GameModel.IsHumanTurn())
				{
					Strategy EventStrategy;
					ApplicationEvent Event = ViewEvents.Take();

					Strategy.TryGetValue(Event.GetType(), out EventStrategy);

					if(EventStrategy != null)
					{
						EventStrategy.Execute(Event);
					}
				}
				else
				{
					GameModel.NextAITurn();
				}

				GameModel.TestDisplay(); //Konsolowe wyswietlanie stanu gry na potrzeby testow
										 //	GameForm.UptadeForm(); //TODO add parameters

				++i;
				i %= GameModel.GetPlayersNumber();
			}


			Console.ReadLine(); //oczekiwanie na enter, zeby gra nie zamykala sie automatycznie - na potrzeby testow
		}
	}

	abstract class Strategy
	{
		public abstract void Execute(ApplicationEvent Event);
	}

	class PlayerMoveStrategy : Strategy
	{
		public override void Execute(ApplicationEvent Event)
		{

		}
	}
}
