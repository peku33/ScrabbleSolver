using System;
using System.Collections.Generic;
using ScrabbleSolver.Board;
using ScrabbleSolver.Events;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca ludzkiego gracza.
	/// </summary>
	class HumanPlayer : Player
	{

		public HumanPlayer(String PlayerName, Model Model) : base(Model)
		{
			this.Name = PlayerName;
		}

		/// <summary>
		/// Metoda dostaje w argumencie wywolania informacje o wszystkich polach na planszy. Jej zadaniem jest odfiltrowanie nowowstawionych kostek, sprawdzenie czy kostki tworza poprawne
		/// slowo i czy zostaly ulozone zgodnie z zasadami gry, a nastepnie dodanie punktow graczowi i wylosowanie dla niego nowych kostek.
		/// </summary>
		/// <param name="MoveEvent"></param>
		/// <returns></returns>
		public override bool MakeMove(PutWordEvent MoveEvent)
		{
			List<Cell> Cells;

			if(MoveEvent != null)
			{
				Cells = MoveEvent.GetBoardCells();
			}
			else
			{
				return false;
			}

			Tuple<List<Cell>, bool> newCellsInfo = GameModel.CheckAndGetNewCells(Cells);
			if(newCellsInfo == null)
			{
				return false;
			}
			if(newCellsInfo.Item1.Count == 0) // Gracz pasuje
			{
				Pass();
				return true;
			}

			Cells = newCellsInfo.Item1;
			bool Vertical = newCellsInfo.Item2;

			if(GameModel.GetBoard().IsEmpty() && Cells.Count != 0) //Jesli pierwszy ruch to sprawdzamy czy slowo jest w srodkowym rzedzie lub kolumnie
			{
				int MiddleCoordinate = GameModel.GetBoard().GetBoardSide() / 2;
				bool MiddleField = false;

				if(Vertical)
				{
					foreach(Cell TempCell in Cells)
					{
						if(TempCell.GetXColumnCoordinate() != MiddleCoordinate)
						{
							GameModel.RemoveTiles(Cells);
							return false;
						}
						if(TempCell.GetYRowCoordinate() == MiddleCoordinate)
						{
							MiddleField = true;
						}
					}
				}
				else
				{
					foreach(Cell TempCell in Cells)
					{
						if(TempCell.GetYRowCoordinate() != MiddleCoordinate)
						{
							GameModel.RemoveTiles(Cells);
							return false;
						}
						if(TempCell.GetXColumnCoordinate() == MiddleCoordinate)
						{
							MiddleField = true;
						}
					}
				}

				if(!MiddleField)
				{
					GameModel.RemoveTiles(Cells);
					return false;
				}
			}

			String NewWord;
			int StartIndex;
			Cell StartCell;

			GameModel.PutTiles(Cells);

			if(Vertical)
			{
				NewWord = GameModel.GetWord(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYRowCoordinate(), Vertical);

				if(NewWord.Length == 1)
				{
					Vertical = !Vertical;
					NewWord = GameModel.GetWord(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate(), Vertical);
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(StartIndex, Cells[0].GetYRowCoordinate());
				}
				else
				{
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYRowCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(Cells[0].GetXColumnCoordinate(), StartIndex);
				}
			}
			else
			{
				NewWord = GameModel.GetWord(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate(), Vertical);

				if(NewWord.Length == 1)
				{
					Vertical = !Vertical;
					NewWord = GameModel.GetWord(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYRowCoordinate(), Vertical);
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYRowCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(Cells[0].GetXColumnCoordinate(), StartIndex);
				}
				else
				{
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(StartIndex, Cells[0].GetYRowCoordinate());
				}
			}

			if(!GameModel.GetDictionary().Exists(NewWord) || !GameModel.IsMoveCorrect(NewWord, StartCell, Vertical)
				|| (!GameModel.GetBoard().IsEmpty() && !IsWordPutCorrect(StartCell, Vertical, NewWord.Length))) //Jesli slowo nie istnieje albo jest wstawione w niepoprawne miejsce albo jego ulozenie powoduje ulozenie niepoprawnych slow
			{
				GameModel.RemoveTiles(Cells);
				return false;
			}

			PointsNumber += GameModel.CountPoints(NewWord, StartCell, Vertical);
			GameModel.SetVisited(Cells);

			Rack.Clear();
			Rack.AddRange(MoveEvent.GetPlayerTiles()); //Aktualizacja tabliczki
			GetNewTiles();
			GameModel.GetBoard().SetEmpty(false);
			return true;
		}

		public override void MakeFirstMove(PutWordEvent MoveEvent)
		{
		}

		public override void ReplaceTile(ReplaceTileEvent ReplaceEvent)
		{
			if(GameModel.GetTilesSet().IsEmpty())
			{
				return;
			}
			Rack.Remove(new Tile(ReplaceEvent.GetReplacedTile()[0]));

			GameModel.GetTilesSet().Add(new Tile(ReplaceEvent.GetReplacedTile()[0]));
			Rack.Add(GameModel.GetTilesSet().GetRandomTile()); //wylosowanie nowej kostki i dodanie do tabliczki
		}

		public override void Pass()
		{
			GameModel.PlayerPassed();
		}

		public bool IsWordPutCorrect(Cell StartCell, bool Vertical, int Length)
		{
			int Index;
			Board.Board GameBoard = GameModel.GetBoard();
			Cell FirstNeighbour, SecondNeighbour;

			if(Vertical)
			{
				Index = StartCell.GetXColumnCoordinate();
				Row TempRow;

				for(int i = 0; i < Length; ++i)
				{
					int y = StartCell.GetYRowCoordinate();
					int x = StartCell.GetXColumnCoordinate();
					TempRow = GameBoard.FindRow(y + i);

					FirstNeighbour = TempRow.Get(x + 1);
					SecondNeighbour = TempRow.Get(x - 1);

					if(FirstNeighbour != null && FirstNeighbour.GetTile() != null)
					{
						return true;
					}
					if(SecondNeighbour != null && SecondNeighbour.GetTile() != null)
					{
						return true;
					}
				}
			}
			else
			{
				Index = StartCell.GetYRowCoordinate();
				Column TempColumn;

				for(int i = 0; i < Length; ++i)
				{
					int y = StartCell.GetYRowCoordinate();
					int x = StartCell.GetXColumnCoordinate();
					TempColumn = GameBoard.FindColumn(x + i);

					FirstNeighbour = TempColumn.Get(y + 1);
					SecondNeighbour = TempColumn.Get(y - 1);

					if(FirstNeighbour != null && FirstNeighbour.GetTile() != null)
					{
						return true;
					}
					if(SecondNeighbour != null && SecondNeighbour.GetTile() != null)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
