using System;
using System.Collections.Generic;
using ScrabbleSolver.Model.Items;
using ScrabbleSolver.Model.Player;

namespace ScrabbleSolver.Model
{
	/// <summary>
	/// Klasa modelu gry.
	/// </summary>
	public class Model
	{
		private readonly Queue<Player.Player> Players;
		private readonly Board.Board GameBoard;
		private readonly Dictionary.Dictionary GameDictionary;

		public Model(Dictionary.Dictionary GameDictionary)
		{
			this.Players = new Queue<Player.Player>();
			this.GameBoard = new Board.Board(Configuration.BoardFile);
			this.GameDictionary = GameDictionary;

			TilesSet.Init();
		}

		public void InitPlayers(int HumanPlayers, int CPUPlayers) //TODO przerobic funkcje tak, aby dalo sie inicjalizowac graczy z wybranymi nickami
		{
			if(HumanPlayers + CPUPlayers > Configuration.MaxPlayersNumber)
			{
				throw new SystemException("too much players!");
			}
			for(int i = 0; i < HumanPlayers; ++i)
			{
				Players.Enqueue(new HumanPlayer("Gracz" + i, GameBoard, GameDictionary));
			}
			for(int i = 0; i < CPUPlayers; ++i)
			{
				Players.Enqueue(new AIPlayer(GameBoard, GameDictionary));
			}

			foreach(Player.Player Player in Players)
			{
				Player.GetNewTiles();
			}
		}

		public Dictionary.Dictionary GetDictionary()
		{
			return this.GameDictionary;
		}

		public void NextTurn()
		{
			TestDisplay(); //Konsolowe wyswietlanie stanu gry na potrzeby testow

			Player.Player P = Players.Dequeue();

			if (GameBoard.IsEmpty())
			{
				P.MakeFirstMove();
			}
			else
			{
				P.MakeMove();
			}
			Console.ReadLine(); //Czekanie na klawisz na potrzeby testow

			Players.Enqueue(P);
		}

		public bool isEnd()
		{
			foreach(Player.Player Player in Players)
			{
				if(Player.HasFinished())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Konsolowe wysiwetlanie planszy do testow
		/// </summary>
		public void TestDisplay()
		{
			GameBoard.ConsoleDisplay();

			foreach(Player.Player TempPlayer in Players)
			{
				System.Console.WriteLine(TempPlayer.GetName() + ": " + TempPlayer.GetPointsNumber() + " Rack: " + TempPlayer.GetLettersString());
			}

			System.Console.WriteLine();
		}

		/// <summary>
		/// Zwraca gracza z największa ilością punktów. W przypadku gdy kilku graczy ma tą samą ilośc punktów, zwraca gracza z mniejszą ilością kostek. W przypadku, gdy gracze mają taką samą ilość punktów 
		/// i taką samą ilość kostek, zwraca gracza, który jest bliżej w kolejce.
		/// </summary>
		/// <returns></returns>
		public Player.Player GetBestPlayer()
		{
			Player.Player BestPlayer = null;

			foreach(Player.Player TempPlayer in Players)
			{
				if(BestPlayer == null || BestPlayer.GetPointsNumber() < TempPlayer.GetPointsNumber())
				{
					BestPlayer = TempPlayer;
				}
				else if(BestPlayer.GetPointsNumber() == TempPlayer.GetPointsNumber())
				{
					if(BestPlayer.GetLettersNumber() > TempPlayer.GetLettersNumber())
					{
						BestPlayer = TempPlayer;
					}	
				}
			}
			return BestPlayer;
		}
	}
}
