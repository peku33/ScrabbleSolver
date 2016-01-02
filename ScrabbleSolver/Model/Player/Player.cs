using System;
using ScrabbleSolver.Board;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca gracza
	/// </summary>
	abstract class Player
	{
		// Ilość punktów zgromadzonych przez gracza
		protected int PointsNumber;

		// Nazwa gracza
		protected String Name;

		// Tabliczka z kostkami
		protected readonly Rack Rack;

		//Plansza gry
		protected readonly Board.Board GameBoard;

		//Słownik gry
		protected readonly Dictionary.Dictionary GameDictionary;

		protected Player(Board.Board GameBoard, Dictionary.Dictionary GameDictionary)
		{
			Rack = new Rack();
			this.GameBoard = GameBoard;
			this.GameDictionary = GameDictionary;
			PointsNumber = 0;
		}

		/// <summary>
		/// Wykonanie ruchu
		/// </summary>
		public abstract void MakeMove();

		/// <summary>
		/// Wykonanie pierwszego ruchu
		/// </summary>
		public abstract void MakeFirstMove();

		/// <summary>
		/// Wymiana kostki
		/// </summary>
		public abstract void ReplaceTile();

		public int GetPointsNumber()
		{
			return this.PointsNumber;
		}

		public String GetName()
		{
			return this.Name;
		}

		public bool HasFinished()
		{
			return Rack.Count == 0;
		}

		/// <summary>
		/// Metoda sprawdzajaca czy wlozenie slowa nie sprawi ze na planszy znajda sie ciagi znakow nie bedace slowami
		/// </summary>
		/// <param name="Word"></param>
		/// <param name="StartCell"></param>
		/// <param name="Vertical"></param>
		/// <returns></returns>
		protected bool IsMoveCorrect(String Word, Cell StartCell, bool Vertical)
		{
			int Index;

			if(Vertical)//Sprawdzenie czy wstawienie slowa nie spowodowalo polaczenia ze soba dwoch slow na planszy tworzac tym samym niepoprawne slowo
			{
				Column TempColumn = GameBoard.FindColumn(StartCell.GetXCoordinate());
				String NewWord = String.Copy(Word);
				Index = StartCell.GetYCoordinate() + Word.Length; //Indeks pola bezposrednio za slowem.

				if(Index < GameBoard.GetBoardSide())
				{
					Cell TempCell = TempColumn.Get(Index);

					while(TempCell != null && TempCell.IsVisited())
					{
						NewWord += TempCell.GetTile().GetLetter();
						TempCell = TempColumn.Get(++Index);
					}
				}

				if(!GameDictionary.Exists(NewWord))
				{
					return false;
				}
			}
			else
			{
				Row TempRow = GameBoard.FindRow(StartCell.GetYCoordinate());
				String NewWord = String.Copy(Word);
				Index = StartCell.GetXCoordinate() + Word.Length; //Indeks pola bezposrednio za slowem.

				if(Index < GameBoard.GetBoardSide())
				{
					Cell TempCell = TempRow.Get(Index);

					while(TempCell != null && TempCell.IsVisited())
					{
						NewWord += TempCell.GetTile().GetLetter();
						TempCell = TempRow.Get(++Index);
					}
				}

				if(!GameDictionary.Exists(NewWord))
				{
					return false;
				}
			}

			if(Vertical)//Sprawdzenie czy wstawienie slowa nie spowodowalo utworzenia niepoprawnych slow prostopadle do kierunku wstawiania slowa
			{
				Index = StartCell.GetXCoordinate();
				Row TempRow;

				for(int i = 0; i < Word.Length; ++i)
				{
					TempRow = GameBoard.FindRow(StartCell.GetYCoordinate() + i);
					String NewWord = String.Empty;

					foreach(Cell TempCell in TempRow)
					{
						if(TempCell.GetXCoordinate() == Index)
						{
							NewWord += Word[i];
						}
						else if(TempCell.IsVisited())
						{
							NewWord += TempCell.GetTile().GetLetter();
						}
						else if(TempCell.GetXCoordinate() > Index) //Ulozone zostalo cale slowo
						{
							if(NewWord.Length > 1)
							{
								if(!GameDictionary.Exists(NewWord))
								{
									return false;
								}
							}
							break;
						}
						else
						{
							NewWord = String.Empty;
						}
					}
					if(NewWord.Length > 1)//Jesli slowo konczy sie przy krawedzi planszy
					{
						if(!GameDictionary.Exists(NewWord))
							return false;
					}
				}
			}
			else
			{
				Index = StartCell.GetYCoordinate();
				Column TempColumn;

				for(int i = 0; i < Word.Length; ++i)
				{
					TempColumn = GameBoard.FindColumn(StartCell.GetXCoordinate() + i);
					String NewWord = String.Empty;

					foreach(Cell TempCell in TempColumn)
					{
						if(TempCell.GetYCoordinate() == Index)
						{
							NewWord += Word[i];
						}
						else if(TempCell.IsVisited())
						{
							NewWord += TempCell.GetTile().GetLetter();
						}
						else if(TempCell.GetYCoordinate() > Index)
						{
							if(NewWord.Length > 1)
							{
								if(!GameDictionary.Exists(NewWord))
								{
									return false;
								}
							}
							break;
						}
						else
						{
							NewWord = String.Empty;
						}
					}
					if(NewWord.Length > 1)//Jesli slowo konczy sie przy krawedzi planszy
					{
						if(!GameDictionary.Exists(NewWord))
							return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Metoda zliczajaca punkty ktore otrzyma gracz za ulozenie okreslonego slowa w okreslone miejsce
		/// </summary>
		/// <param name="Word"></param>
		/// <param name="StartCell"></param>
		/// <param name="Vertical"></param>
		/// <returns></returns>
		protected int CountPoints(String Word, Cell StartCell, bool Vertical)
		{
			int Points = 0;

			Container ConsideredContainer;
			int CellIndex;

			if(Vertical)
			{
				ConsideredContainer = GameBoard.FindColumn(StartCell);
				CellIndex = StartCell.GetYCoordinate();
			}
			else
			{
				ConsideredContainer = GameBoard.FindRow(StartCell);
				CellIndex = StartCell.GetXCoordinate();
			}

			Points += CountWord(ConsideredContainer, Word, CellIndex);

			if(Vertical)
			{
				int Index = StartCell.GetXCoordinate();
				Row TempRow;
				var NewWordParamteres = new Tuple<String, int>(String.Empty, 0);

				for(int i = 0; i < Word.Length; ++i)
				{
					TempRow = GameBoard.FindRow(StartCell.GetYCoordinate() + i);
					
					if(!TempRow.Get(Index).IsVisited())
					{
						NewWordParamteres = CreateWord(TempRow, Index, Word[i]);
						int StartIndex = NewWordParamteres.Item2;
						String NewWord = NewWordParamteres.Item1;

						if(NewWord.Length > 1)
						{
							Points += CountWord(TempRow, NewWord, StartIndex);
						}
					}
				}
			}
			else
			{
				int Index = StartCell.GetYCoordinate();
				Column TempColumn;
				var NewWordParamteres = new Tuple<String, int>(String.Empty, 0);

				for(int i = 0; i < Word.Length; ++i)
				{
					TempColumn = GameBoard.FindColumn(StartCell.GetXCoordinate() + i);
					
					if(!TempColumn.Get(Index).IsVisited())
					{
						NewWordParamteres = CreateWord(TempColumn, Index, Word[i]);
						int StartIndex = NewWordParamteres.Item2;
						String NewWord = NewWordParamteres.Item1;

						if(NewWord.Length > 1)
						{
							Points += CountWord(TempColumn, NewWord, StartIndex);
						}
					}
				}
			}

			return Points;
		}

		/// <summary>
		/// Metoda wstawia kostki w odpowiednie miejsce na planszy
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartCell"></param>
		/// <param name="Word"></param>
		protected void PutTiles(Container Container, int StartIndex, Dictionary.Dictionary.WordFound Word)
		{
			String NewWord = Word.GetWord();

			for(int i = 0; i < NewWord.Length; ++i)
			{
				Cell TempCell = Container.Get(StartIndex + i);

				if(!TempCell.IsVisited())
				{
					if(Rack.Contains(NewWord[i])) //Sprawdzenie, czy tabliczka zawiera kostke z dana litera - jesli nie, to wstawiamy blanka
					{
						TempCell.SetTile(new Tile(NewWord[i]));
					}
					else
					{
						TempCell.SetTile(new Tile(NewWord[i], true));
					}
				}
			}
		}

		/// <summary>
		/// Metoda wstawia kostki w odpowiednie miejsce na planszy i usuwa je z tabliczki gracza
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartCell"></param>
		/// <param name="Word"></param>
		protected void PutAndRemoveTiles(Container Container, int StartIndex, Dictionary.Dictionary.WordFound Word)
		{
			String NewWord = Word.GetWord();

			for(int i = 0; i < NewWord.Length; ++i)
			{
				Cell TempCell = Container.Get(StartIndex + i);

				if(!TempCell.IsVisited())
				{
					if(Rack.Contains(NewWord[i])) //Sprawdzenie, czy tabliczka zawiera kostke z dana litera - jesli nie, to wstawiamy blanka
					{
						TempCell.SetTile(new Tile(NewWord[i]));
					}
					else
					{
						TempCell.SetTile(new Tile(NewWord[i], true));
					}

					Rack.Remove(NewWord[i]);
					TempCell.SetVisited(true);
				}
			}
		}

		/// <summary>
		/// Metoda wyjmuje okreslona ilosc kostek z planszy
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartIndex"></param>
		/// <param name="Word"></param>
		protected void RemoveTiles(Container Container, int StartIndex, int Number)
		{
			for(int i = 0; i < Number; ++i)
			{
				Cell TempCell = Container.Get(StartIndex + i);

				if(!TempCell.IsVisited())
				{
					TempCell.SetTile(null);
				}
			}
		}

		/// <summary>
		/// Losowanie nowych kostek
		/// </summary>
		public void GetNewTiles()
		{
			while(!TilesSet.IsEmpty() && !Rack.IsFull())
			{
				Rack.Add(TilesSet.GetRandomTile());
			}
		}

		/// <summary>
		/// Metoda ukladajaca slowo, ktore przechodzi przez pole o indeksie EmptyIndex w kontenerze podanym 
		/// w argumencie wywolania.
		/// </summary>
		/// <param name="Container">Plaszyzna z ktorej odczytujemy slowo</param>
		/// <param name="EmptyIndex">Indeks, na ktorym nie ma jeszcze ulozonej kostki</param>
		/// <param name="NewLetter">Symbol, ktory ma zostac wstawiony w pusty indeks</param>
		/// <returns>tuple zawierajacy nowe słowo oraz indeks od ktorego slowo sie zaczyna w kontenerze Container</returns>
		private Tuple<String, int> CreateWord(Container Container, int EmptyIndex, char NewLetter)
		{
			Cell TempCell;
			int StartIndex = 0;
			String NewWord = String.Empty;

			for(int i = 0; i < Container.Count; ++i)
			{
				TempCell = Container.Get(i);

				if(i == EmptyIndex)
				{
					NewWord += NewLetter;
				}
				else if(TempCell.IsVisited())
				{
					NewWord += TempCell.GetTile().GetLetter();
				}
				else if(i > EmptyIndex)
				{
					return Tuple.Create(NewWord, StartIndex);
				}
				else
				{
					NewWord = String.Empty;
					StartIndex = i + 1;
				}
			}

			return Tuple.Create(NewWord, StartIndex);
		}

		/// <summary>
		/// Metoda zliczajaca ile punktow warte jest slowo
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="Word"></param>
		/// <param name="StartIndex"></param>
		/// <returns></returns>
		protected int CountWord(Container Container, String Word, int StartIndex)
		{
			int Points = 0;
			int WordMultiplier = 1;
			Cell TempCell;

			for(int i = 0; i < Word.Length; ++i)
			{
				TempCell = Container.Get(StartIndex + i);

				if(TempCell.IsVisited() && !TempCell.IsBlank())
				{
					Points += Configuration.GetLetterValue(Word[i]);
				}
				else
				{
					WordMultiplier *= TempCell.GetWordMultiplier();
					Points += Configuration.GetLetterValue(Word[i]) * TempCell.GetLetterMultiplier();
				}
			}

			return Points * WordMultiplier;
		}

		/// <summary>
		/// TYLKO DO TESTOW
		/// </summary>
		/// <returns></returns>
		public String GetLettersString()
		{
			return Rack.GetTileString();
		}
	}
}
