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
		private readonly List<Player.Player> Players;
		private readonly Board.Board GameBoard;
		private readonly Dictionary.Dictionary GameDictionary;
		private readonly TilesSet TilesSet;

		public Model(Dictionary.Dictionary GameDictionary)
		{
			TilesSet = new TilesSet();
			this.Players = new List<Player.Player>();
			this.GameBoard = new Board.Board(Configuration.BoardFile);
			this.GameDictionary = GameDictionary;
		}

		public void InitPlayers(Player.Player Player1, Player.Player Player2, Player.Player Player3, Player.Player Player4) //TODO przerobic funkcje tak, aby dalo sie inicjalizowac graczy z wybranymi nickami
		{
			if(Player1 != null)
				Players.Add(Player1);
			if(Player2 != null)
				Players.Add(Player2);
			if(Player3 != null)
				Players.Add(Player3);
			if(Player4 != null)
				Players.Add(Player4);

			foreach(Player.Player Player in Players)
			{
				Player.GetNewTiles();
			}
		}

		public Dictionary.Dictionary GetDictionary()
		{
			return this.GameDictionary;
		}

		public Board.Board GetBoard()
		{
			return this.GameBoard;
		}

		public TilesSet GetTilesSet()
		{
			return TilesSet;
		}

		public int GetPlayersNumber()
		{
			return Players.Count;
		}

		public void NextTurn(int Index)
		{
			Player.Player P = Players[Index];

			if(GameBoard.IsEmpty() && P != null)
			{
				P.MakeFirstMove();
			}
			else
			{
				P.MakeMove();
			}
			Console.ReadLine(); //Czekanie na klawisz na potrzeby testow
		}

		/// <summary>
		/// Gra konczy sie gdy dowolnemu graczowi zabraknie kostek lub gdy zaden z graczy nie ma ruchu
		/// </summary>
		/// <returns></returns>
		public bool isEnd()
		{
			bool Blocked = true;

			foreach(Player.Player Player in Players)
			{
				if(Player.HasFinished())
				{
					return true;
				}
				if(!Player.IsBlocked())
				{
					Blocked = false;
				}
			}

			if(Blocked)
			{
				return true;
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
