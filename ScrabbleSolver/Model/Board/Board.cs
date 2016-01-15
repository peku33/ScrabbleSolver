using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca planszę gry
	/// </summary>
	public class Board
	{
		//Liczba kostek mieszczących się w jednym rzędzie planszy
		private static readonly int BoardSide = 15;

		//Lista wierszy planszy
		private readonly List<Row> Rows;

		//Lista kolumn planszy
		private readonly List<Column> Columns;

		private bool Empty;

		public Board()
		{
			Rows = new List<Row>();
			Columns = new List<Column>();
			Empty = true;

			Init();
		}

		public void Clear()
		{
			foreach(Row TempRow in Rows)
			{
				foreach(Cell TempCell in TempRow)
				{
					TempCell.SetVisited(false);
					TempCell.SetTile(null);
					Empty = true;
				}
			}
		}

		/// <summary>
		/// Wyswietlanie aktualnego stanu planszy na konsoli na potrzeby testowania
		/// </summary>
		public void ConsoleDisplay()
		{
			System.Console.Write("  ");

			for(int i = 0; i < BoardSide; ++i)
			{
				if(i > 9)
					System.Console.Write(i);
				else
					System.Console.Write(i + " ");
			}

			System.Console.WriteLine();

			foreach(Row TempRow in Rows)
			{
				if(TempRow.GetYCoordinate() > 9)
				{
					System.Console.Write(TempRow.GetYCoordinate());
				}
				else
				{
					System.Console.Write(TempRow.GetYCoordinate() + " ");
				}

				foreach(Cell TempCell in TempRow)
				{
					if(TempCell.IsVisited())
					{
						if(TempCell.IsBlank())
						{
							System.Console.Write("#");
						}
						else
						{
							System.Console.Write(TempCell.GetTile().GetLetter());
						}
						System.Console.Write(" ");
					}
					else
					{
						System.Console.Write("*");
						System.Console.Write(" ");
					}
				}
				System.Console.Write("\n");
			}
		}

		public bool IsEmpty()
		{
			return Empty;
		}

		public void SetEmpty(bool Empty)
		{
			this.Empty = Empty;
		}

		public int GetBoardSide()
		{
			return Board.BoardSide;
		}

		public List<Row> GetRows()
		{
			return this.Rows;
		}

		public List<Column> GetColumns()
		{
			return this.Columns;
		}

		public Cell GetCell(int x, int y)
		{
			return Columns[x].Get(y);
		}

		public Row FindRow(Cell Cell)
		{
			return Rows.FirstOrDefault(TempRow => TempRow.Contains(Cell));
		}

		public Row FindRow(int Index)
		{
			return Rows.FirstOrDefault(TempRow => TempRow.GetYCoordinate() == Index);
		}

		public Column FindColumn(Cell Cell)
		{
			return Columns.FirstOrDefault(TempColumn => TempColumn.Contains(Cell));
		}

		public Column FindColumn(int Index)
		{
			return Columns.FirstOrDefault(TempColumn => TempColumn.GetXCoordinate() == Index);
		}

		/// <summary>
		/// Inicjalizacja planszy gry.
		/// </summary>
		private void Init()
		{
			for(int i = 0; i < BoardSide; ++i)
			{
				Rows.Add(new Row(i));
				Columns.Add(new Column(i));
			}

			for(int i = 0; i < BoardSide; ++i)
			{
				for(int j = 0; j < BoardSide; ++j)
				{
					Cell TempCell = new Cell(new Coordinates(j, i), 1, 1);
					Rows[i].Add(TempCell);
					Columns[j].Add(TempCell);
				}
			}

			InitMultipliers();
		}

		/// <summary>
		/// Inicjalizacja premii słownych i literowych
		/// </summary>
		private void InitMultipliers()
		{
			int TripleMultiplier = 3;
			int DoubleMultiplier = 2;

			int CenterIndex = BoardSide / 2;
			int LastIndex = BoardSide - 1;

			//potrojne premie slowne
			Rows[0].Get(0).SetWordMultiplier(TripleMultiplier);
			Rows[0].Get(CenterIndex).SetWordMultiplier(TripleMultiplier);
			Rows[0].Get(LastIndex).SetWordMultiplier(TripleMultiplier);
			Rows[LastIndex].Get(0).SetWordMultiplier(TripleMultiplier);
			Rows[LastIndex].Get(CenterIndex).SetWordMultiplier(TripleMultiplier);
			Rows[LastIndex].Get(LastIndex).SetWordMultiplier(TripleMultiplier);
			Rows[CenterIndex].Get(0).SetWordMultiplier(TripleMultiplier);
			Rows[CenterIndex].Get(LastIndex).SetWordMultiplier(TripleMultiplier);

			//podwojne premie slowne
			Rows[CenterIndex].Get(CenterIndex).SetWordMultiplier(DoubleMultiplier);
			for(int i = 1; i < 5; ++i)
			{
				Row TempRow = Rows[i];
				TempRow.Get(i).SetWordMultiplier(DoubleMultiplier);
				TempRow.Get(LastIndex - i).SetWordMultiplier(DoubleMultiplier);
				TempRow = Rows[LastIndex - i];
				TempRow.Get(i).SetWordMultiplier(DoubleMultiplier);
				TempRow.Get(LastIndex - i).SetWordMultiplier(DoubleMultiplier);
			}

			//potrojne premie literowe
			Rows[1].Get(CenterIndex - 2).SetLetterMultiplier(TripleMultiplier);
			Rows[1].Get(CenterIndex + 2).SetLetterMultiplier(TripleMultiplier);
			Rows[LastIndex - 1].Get(CenterIndex - 2).SetLetterMultiplier(TripleMultiplier);
			Rows[LastIndex - 1].Get(CenterIndex + 2).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex - 2].Get(CenterIndex - 2).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex - 2].Get(CenterIndex + 2).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex + 2].Get(CenterIndex - 2).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex + 2].Get(CenterIndex + 2).SetLetterMultiplier(TripleMultiplier);

			Rows[CenterIndex - 2].Get(1).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex - 2].Get(LastIndex - 1).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex + 2].Get(1).SetLetterMultiplier(TripleMultiplier);
			Rows[CenterIndex + 2].Get(LastIndex - 1).SetLetterMultiplier(TripleMultiplier);

			//podwojne premie literowe
			for(int i = 0; i < 2; ++i)
			{
				int StartIndex = 3;

				Row TempRow = Rows[StartIndex - i];
				Column TempColumn = Columns[StartIndex - i];
				TempRow.Get(CenterIndex - i).SetLetterMultiplier(DoubleMultiplier);
				TempRow.Get(CenterIndex + i).SetLetterMultiplier(DoubleMultiplier);
				TempColumn.Get(CenterIndex - i).SetLetterMultiplier(DoubleMultiplier);
				TempColumn.Get(CenterIndex + i).SetLetterMultiplier(DoubleMultiplier);

				TempRow = Rows[LastIndex - (StartIndex - i)];
				TempColumn = Columns[LastIndex - (StartIndex - i)];
				TempRow.Get(CenterIndex - i).SetLetterMultiplier(DoubleMultiplier);
				TempRow.Get(CenterIndex + i).SetLetterMultiplier(DoubleMultiplier);
				TempColumn.Get(CenterIndex - i).SetLetterMultiplier(DoubleMultiplier);
				TempColumn.Get(CenterIndex + i).SetLetterMultiplier(DoubleMultiplier);
			}
			Rows[0].Get(3).SetLetterMultiplier(DoubleMultiplier);
			Rows[0].Get(LastIndex - 3).SetLetterMultiplier(DoubleMultiplier);
			Rows[LastIndex].Get(3).SetLetterMultiplier(DoubleMultiplier);
			Rows[LastIndex].Get(LastIndex - 3).SetLetterMultiplier(DoubleMultiplier);
			Columns[0].Get(3).SetLetterMultiplier(DoubleMultiplier);
			Columns[0].Get(LastIndex - 3).SetLetterMultiplier(DoubleMultiplier);
			Columns[LastIndex].Get(3).SetLetterMultiplier(DoubleMultiplier);
			Columns[LastIndex].Get(LastIndex - 3).SetLetterMultiplier(DoubleMultiplier);
			Rows[CenterIndex - 1].Get(CenterIndex - 1).SetLetterMultiplier(DoubleMultiplier);
			Rows[CenterIndex - 1].Get(CenterIndex + 1).SetLetterMultiplier(DoubleMultiplier);
			Rows[CenterIndex + 1].Get(CenterIndex - 1).SetLetterMultiplier(DoubleMultiplier);
			Rows[CenterIndex + 1].Get(CenterIndex + 1).SetLetterMultiplier(DoubleMultiplier);
		}
	}
}
