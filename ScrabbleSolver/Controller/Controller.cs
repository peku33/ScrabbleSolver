using System;
using ScrabbleSolver.Model;
using ScrabbleSolver.Model.Player;

namespace ScrabbleSolver.Controller
{
	public class Controller
	{
		private readonly Model.Model GameModel;

		public Controller(Model.Model Model)
		{
			this.GameModel = Model;
		}

		public void Start()
		{
			GameModel.InitPlayers(new AIPlayer(GameModel), new AIPlayer(GameModel), null, null);
			GameModel.TestDisplay();

			int i = 0;
			while(!GameModel.IsEnd())
			{
				GameModel.NextTurn(i);
				GameModel.TestDisplay(); //Konsolowe wyswietlanie stanu gry na potrzeby testow
				++i;
				i %= GameModel.GetPlayersNumber();
			}

			Console.ReadLine(); //oczekiwanie na enter, zeby gra nie zamykala sie automatycznie - na potrzeby testow
		}
	}
}