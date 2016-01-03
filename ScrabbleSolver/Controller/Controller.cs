using System;

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
			GameModel.InitPlayers(0, 2);
			GameModel.TestDisplay();

			while(!GameModel.isEnd())
			{
				GameModel.NextTurn();
				GameModel.TestDisplay(); //Konsolowe wyswietlanie stanu gry na potrzeby testow
			}

			Console.ReadLine(); //oczekiwanie na enter, zeby gra nie zamykala sie automatycznie - na potrzeby testow
		}
	}
}