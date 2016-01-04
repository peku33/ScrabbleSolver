using System;
using ScrabbleSolver.Board;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca gracza
	/// </summary>
	public abstract class Player
	{
		// Ilość punktów zgromadzonych przez gracza
		protected int PointsNumber;

		// Nazwa gracza
		protected String Name;

		// Tabliczka z kostkami
		protected readonly Rack Rack;

		/*//Plansza gry
		protected readonly Board.Board GameBoard;
*/
		protected readonly Model GameModel;

		//Czy gracz ma jeszcze mozliwosc ruchu
		protected bool Blocked;

		protected Player(Model GameModel)
		{
			Rack = new Rack();
			PointsNumber = 0;
			Blocked = false;
			this.GameModel = GameModel;
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

		public bool IsBlocked()
		{
			return Blocked;
		}

		public void SetBlocked(bool Blocked)
		{
			this.Blocked = Blocked;
		}

		/// <summary>
		/// Losowanie nowych kostek
		/// </summary>
		public void GetNewTiles()
		{
			TilesSet TilesSet = GameModel.GetTilesSet();

            while(!TilesSet.IsEmpty() && !Rack.IsFull())
			{
				Rack.Add(TilesSet.GetRandomTile());
			}
		}

		public int GetLettersNumber()
		{
			return Rack.Count;
		}

		/// <summary>
		/// TYLKO DO TESTOW - zwraca String z literami znajdujacymi sie w tabliczce
		/// </summary>
		/// <returns></returns>
		public String GetLettersString()
		{
			return Rack.GetTileString();
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
			Dictionary.Dictionary GameDictionary = GameModel.GetDictionary();
			Board.Board GameBoard = GameModel.GetBoard();

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
			Board.Board GameBoard = GameModel.GetBoard();
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

			Points += CountWord(ConsideredContainer, CellIndex);

			if(Vertical)
			{
				int Index = StartCell.GetXCoordinate();
				Row TempRow;
				var NewWordParamteres = new Tuple<int, int>(0, 0);

				for(int i = 0; i < Word.Length; ++i)
				{
					TempRow = GameBoard.FindRow(StartCell.GetYCoordinate() + i);

					if(!TempRow.Get(Index).IsVisited())
					{
						NewWordParamteres = GetWordInfo(TempRow, Index);
						int StartIndex = NewWordParamteres.Item2;

						if(NewWordParamteres.Item1 > 1)
						{
							Points += CountWord(TempRow, StartIndex);
						}
					}
				}
			}
			else
			{
				int Index = StartCell.GetYCoordinate();
				Column TempColumn;
				var NewWordParamteres = new Tuple<int, int>(0, 0);

				for(int i = 0; i < Word.Length; ++i)
				{
					TempColumn = GameBoard.FindColumn(StartCell.GetXCoordinate() + i);

					if(!TempColumn.Get(Index).IsVisited())
					{
						NewWordParamteres = GetWordInfo(TempColumn, Index);
						int StartIndex = NewWordParamteres.Item2;

						if(NewWordParamteres.Item1 > 1)
						{
							Points += CountWord(TempColumn, StartIndex);
						}
					}
				}
			}
			return Points;
		}

		/// <summary>
		/// Metoda wstawia kostki w odpowiednie miejsce na planszy i usuwa je z tabliczki gracza
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
					TempCell.SetTile(Rack.Contains(NewWord[i]) ? new Tile(NewWord[i]) : new Tile(NewWord[i], true));

					Rack.Remove(NewWord[i]);
				}
			}
		}

		/// <summary>
		/// Metoda wstawia kostki w odpowiednie miejsce na planszy, usuwa je z tabliczki i oznacza pola jako odwiedzone - wykorzystywana przy ostatecznym wstawianiu kostek na plansze
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartCell"></param>
		/// <param name="Word"></param>
		protected void PutAndSetTiles(Container Container, int StartIndex, Dictionary.Dictionary.WordFound Word)
		{
			String NewWord = Word.GetWord();

			for(int i = 0; i < NewWord.Length; ++i)
			{
				Cell TempCell = Container.Get(StartIndex + i);

				if(!TempCell.IsVisited())
				{
					TempCell.SetTile(Rack.Contains(NewWord[i]) ? new Tile(NewWord[i]) : new Tile(NewWord[i], true));

					Rack.Remove(NewWord[i]);
					TempCell.SetVisited(true);
				}
			}
		}

		/// <summary>
		/// Metoda wyjmuje okreslona ilosc kostek z planszy i dodaje je do tabliczki
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
					Tile TempTile = TempCell.GetTile();
					if(TempTile != null)
					{
						if(TempTile.IsBlank())
						{
							Rack.Add(new Tile(' ', true));
						}
						else
						{
							Rack.Add(TempTile);
						}
					}
					TempCell.SetTile(null);
				}
			}
		}

		/// <summary>
		/// Metoda zliczajaca ile punktow warte jest slowo
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartIndex"></param>
		/// <returns></returns>
		protected int CountWord(Container Container, int StartIndex)
		{
			int Points = 0;
			int WordMultiplier = 1;
			Cell TempCell;
			int i = 0;
			int LettersUsed = 0; //Licznik dolozonych w tej turze kostek

			while((TempCell = Container.Get(StartIndex + i++)) != null)
			{
				Tile TempTile = TempCell.GetTile();

				if(TempTile == null)
				{
					break;
				}

				if(TempCell.IsVisited())
				{
					Points += TempTile.GetValue();
				}
				else
				{
					++LettersUsed;
					WordMultiplier *= TempCell.GetWordMultiplier();
					Points += TempTile.GetValue() * TempCell.GetLetterMultiplier();
				}
			}

			if(LettersUsed == Configuration.MaxLetterNumber)
			{
				return Points * WordMultiplier + Configuration.SpecialBonus;
			}
			return Points * WordMultiplier;
		}

		/// <summary>
		/// Metoda zwracajaca informacje o slowie, ktore przechodzi przez pole o indeksie Index w kontenerze podanym 
		/// w argumencie wywolania.
		/// </summary>
		/// <param name="Container">Plaszyzna z ktorej odczytujemy slowo</param>
		/// <param name="Index">Indeks, na ktorym nie ma jeszcze ulozonej kostki</param>
		/// <returns>tuple zawierajacy dlugosc slowa oraz indeks od ktorego slowo sie zaczyna w kontenerze Container</returns>
		private Tuple<int, int> GetWordInfo(Container Container, int Index)
		{
			Cell TempCell;
			int StartIndex = 0;
			int NewWordLength = 0;

			for(int i = 0; i < Container.Count; ++i)
			{
				TempCell = Container.Get(i);

				if(TempCell.IsVisited() || i == Index)
				{
					++NewWordLength;
				}
				else if(i > Index)
				{
					return Tuple.Create(NewWordLength, StartIndex);
				}
				else
				{
					NewWordLength = 0;
					StartIndex = i + 1;
				}
			}
			return Tuple.Create(NewWordLength, StartIndex);
		}
	}
}
