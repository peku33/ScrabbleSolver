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
							return false;
						}
						if(TempCell.GetYColumnCoordinate() == MiddleCoordinate)
						{
							MiddleField = true;
						}
					}
				}
				else
				{
					foreach(Cell TempCell in Cells)
					{
						if(TempCell.GetYColumnCoordinate() != MiddleCoordinate)
						{
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
					return false;
				}
			}

			String NewWord;
			int StartIndex;
			Cell StartCell;

			GameModel.PutTiles(Cells);

			if(Vertical)
			{
				NewWord = GameModel.GetWord(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYColumnCoordinate(), Vertical);

				if(NewWord.Length == 1)
				{
					Vertical = !Vertical;
					NewWord = GameModel.GetWord(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate(), Vertical);
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(StartIndex, Cells[0].GetYColumnCoordinate());
				}
				else
				{
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYColumnCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(Cells[0].GetXColumnCoordinate(), StartIndex);
				}
			}
			else
			{
				NewWord = GameModel.GetWord(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate(), Vertical);

				if(NewWord.Length == 1)
				{
					Vertical = !Vertical;
					NewWord = GameModel.GetWord(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYColumnCoordinate(), Vertical);
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindColumn(Cells[0]), Cells[0].GetYColumnCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(Cells[0].GetXColumnCoordinate(), StartIndex);
				}
				else
				{
					StartIndex = GameModel.GetWordInfo(GameModel.GetBoard().FindRow(Cells[0]), Cells[0].GetXColumnCoordinate()).Item2;
					StartCell = GameModel.GetBoard().GetCell(StartIndex, Cells[0].GetYColumnCoordinate());
				}
			}

			if(!GameModel.GetDictionary().Exists(NewWord) || !GameModel.IsMoveCorrect(NewWord, StartCell, Vertical)
				|| (!GameModel.GetBoard().IsEmpty() && !GameModel.IsPositionCorrect(StartCell, Vertical))) //Jesli slowo nie istnieje albo jest wstawione w niepoprawne miejsce albo jego ulozenie powoduje ulozenie niepoprawnych slow
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
	}
}
