using System;
using System.Collections.Generic;
using ScrabbleSolver.Board;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca gracza komputerowego.
	/// </summary>
	class AIPlayer : Player
	{

		public AIPlayer(Board.Board GameBoard, Dictionary.Dictionary GameDictionary) : base(GameBoard, GameDictionary)
		{
			this.Name = "CPU";
		}

		/// <summary>
		/// Procedura wykonywania ruchu przez algorytm gracza komputerowego. Dla kazdego pola, algorytm sprawdza czy można zgodnie z zasadami gry wstawić słowo w taki sposób, że pierwsza jego litera
		/// znajdować będzie się na rozpatrywanym polu. Następnie algorytm wyszukuje najlepiej punktowane słowo i zapisuje je (a także zapisuje informacje o tym, gdzie słowo powinno zostać
		/// umieszczone). Po wykonaniu procedury dla wszystkich pól, algorytm wstawia najlepsze znalezione słowo. Jeśli gracz nie ma możliwości wstawienia słowa w żadne miejsce, wymienia losową kostkę.
		/// </summary>
		public override void MakeMove()
		{
			if(GameBoard.IsEmpty())
			{
				MakeFirstMove();
				return;
			}

			int BestResult = 0;
			int BestStartIndex = 0;
			Dictionary.Dictionary.WordFound BestWord = null;
			Container BestContainer = null;

			Dictionary.Dictionary.HeldCharacters HC = new Dictionary.Dictionary.HeldCharacters(GameDictionary.GetDictionaryEncoding());

			HC.Add(Rack.GetTileString());

			int MinLength = 0;
			int MaxLength = GameBoard.GetBoardSide();

			foreach(Row TempRow in GameBoard.GetRows())
			{
				for(int i = 0; i < GameBoard.GetBoardSide() - 1; ++i)
				{
					Cell TempCell = TempRow.Get(i);

					if(isPositionCorrect(TempCell, false)) //Czy da sie ustawic slowo zaczynajac od tego pola tak, aby bylo to zgodne z regulami gry
					{
						MaxLength = GameBoard.GetBoardSide() - TempCell.GetXCoordinate();
						MinLength = GetMinLength(TempCell, false);

						Dictionary.Dictionary.AlreadySetLetters ASL = new Dictionary.Dictionary.AlreadySetLetters();
						FillAlreadySetLetters(ASL, TempRow, TempCell.GetXCoordinate());

						Dictionary.Dictionary.WordsFound WordsFound = GameDictionary.Find(ASL, HC);
						WordsFound = FilterWords(WordsFound, MinLength, MaxLength);

						foreach(Dictionary.Dictionary.WordFound TempWord in WordsFound)
						{
							if(IsMoveCorrect(TempWord.GetWord(), TempCell, false)) //Czy ustawienie tego slowa nie spowoduje kolizji w drugiej plaszczyznie
							{
								int Result = CountPoints(TempWord.GetWord(), TempCell, false);
								if(Result > BestResult)
								{
									BestResult = Result;
									BestStartIndex = TempCell.GetXCoordinate();
									BestWord = TempWord;
									BestContainer = TempRow;
								}
							}
						}
					}
				}
			}

			foreach(Column TempColumn in GameBoard.GetColumns())
			{
				for(int i = 0; i < GameBoard.GetBoardSide() - 1; ++i)
				{
					Cell TempCell = TempColumn.Get(i);

					if(isPositionCorrect(TempCell, true))
					{
						MaxLength = GameBoard.GetBoardSide() - TempCell.GetYCoordinate();
						MinLength = GetMinLength(TempCell, true);

						Dictionary.Dictionary.AlreadySetLetters ASL = new Dictionary.Dictionary.AlreadySetLetters();
						FillAlreadySetLetters(ASL, TempColumn, TempCell.GetYCoordinate());

						Dictionary.Dictionary.WordsFound WordsFound = GameDictionary.Find(ASL, HC);
						WordsFound = FilterWords(WordsFound, MinLength, MaxLength);

						foreach(Dictionary.Dictionary.WordFound TempWord in WordsFound)
						{
							if(IsMoveCorrect(TempWord.GetWord(), TempCell, true))
							{
								int Result = CountPoints(TempWord.GetWord(), TempCell, true);
								if(Result > BestResult)
								{
									BestResult = Result;
									BestStartIndex = TempCell.GetYCoordinate();
									BestWord = TempWord;
									BestContainer = TempColumn;
								}
							}
						}
					}
				}
			}

			if(BestWord == null || BestWord.GetWord().Length < 2) //Jesli nie da sie ulozyc zadnego slowa
			{
				ReplaceTile();
				return;
			}

			this.PointsNumber += BestResult;

			PutTiles(BestContainer, BestStartIndex, BestWord);

			GetNewTiles();
		}

		public void MakeFirstMove()
		{
			int BestResult = 0;
			int BestStartIndex = 0;
			Dictionary.Dictionary.WordFound BestWord = null;
			int CenterIndex = GameBoard.GetBoardSide() % 2 == 0 ? GameBoard.GetBoardSide() / 2 : GameBoard.GetBoardSide() / 2;
			Container CenterRow = GameBoard.FindRow(CenterIndex);

			Dictionary.Dictionary.HeldCharacters HC = new Dictionary.Dictionary.HeldCharacters(GameDictionary.GetDictionaryEncoding());
			HC.Add(Rack.GetTileString());
			Dictionary.Dictionary.AlreadySetLetters ASL = new Dictionary.Dictionary.AlreadySetLetters();
			Dictionary.Dictionary.WordsFound WordsFound = GameDictionary.Find(ASL, HC);

			for(int i = CenterIndex - Configuration.MaxLetterNumber; i <= CenterIndex; ++i)
			{
				Cell TempCell = CenterRow.Get(i);

				foreach(Dictionary.Dictionary.WordFound TempWord in WordsFound)
				{
					if(TempWord.GetWord().Length > CenterIndex - i)
					{
						int Result = CountWord(CenterRow, TempWord.GetWord(), TempCell.GetXCoordinate());
						if(Result > BestResult)
						{
							BestResult = Result;
							BestStartIndex = TempCell.GetXCoordinate();
							BestWord = TempWord;
						}
					}
				}
			}

			if(BestWord == null || BestWord.GetWord().Length < 2) //Jesli nie da sie ulozyc zadnego slowa
			{
				ReplaceTile();
				return;
			}

			this.PointsNumber += BestResult;

			PutTiles(CenterRow, BestStartIndex, BestWord);

			GameBoard.SetEmpty(false);
			GetNewTiles();
		}

		/// <summary>
		/// Wymiana kostki. Z tabliczki zostaje wyjeta losowa kostka, ktora nie jest blankiem - blanka nigdy nie oplaca sie wymieniac.
		/// </summary>
		public override void ReplaceTile()
		{
			Tile TempTile;

			if(TilesSet.IsEmpty())
			{
				return;
			}

			while((TempTile = Rack.GetRandomTile()).GetLetter().Equals(' '))
			{
				Rack.Add(TempTile);
			}

			Rack.Add(TilesSet.GetRandomTile());
		}

		/// <summary>
		/// Metoda sprawdza czy da sie zgodnie z zasadami gry ulozyc slowo, ktora zaczyna sie od pola przekazanego w argumencie wywolania
		/// </summary>
		/// <param name="StartCell"></param>
		/// <param name="Vertical">Flaga informujaca czy sprawdzamy czy slowo da sie ulozyc w pionie czy poziomie</param>
		/// <returns></returns>
		private bool isPositionCorrect(Cell StartCell, bool Vertical)
		{
			Container ConsideredContainer;
			Container LeftNeighbour;
			Container RightNeighbour;
			int CellIndex;
			Cell TempCell;

			if(Vertical) //jesli sprawdzamy czy da sie ulozyc slowo pionowo
			{
				ConsideredContainer = GameBoard.FindColumn(StartCell);
				CellIndex = StartCell.GetYCoordinate();
				LeftNeighbour = GameBoard.FindColumn(StartCell.GetXCoordinate() - 1);
				RightNeighbour = GameBoard.FindColumn(StartCell.GetXCoordinate() + 1);
			}
			else
			{
				ConsideredContainer = GameBoard.FindRow(StartCell);
				CellIndex = StartCell.GetXCoordinate();
				LeftNeighbour = GameBoard.FindRow(StartCell.GetYCoordinate() - 1);
				RightNeighbour = GameBoard.FindRow(StartCell.GetYCoordinate() + 1);
			}

			TempCell = ConsideredContainer.Get(CellIndex - 1);
			if(TempCell != null) //Sprawdzenie, czy ponad polem nie znajduje sie juz jakis znak
			{
				if(TempCell.IsVisited())
				{
					return false;
				}
			}

			if(StartCell.IsVisited())
			{
				for(int i = CellIndex + 1; i < GameBoard.GetBoardSide(); ++i) //Sprawdzenie, czy za rozpatrywanym polem pozostały jakies wolne pola
				{
					if(!ConsideredContainer.Get(i).IsVisited())
					{
						return true;
					}
				}
				return false;
			}

			for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
			{
				TempCell = ConsideredContainer.Get(CellIndex + i + 1); //+ 1 bo zaczynamy od pola pod pierwszym polem

				if(TempCell == null)
				{
					break;
				}
				else if(TempCell.IsVisited())
				{
					return true;
				}
			}

			if(LeftNeighbour != null)
			{
				for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
				{
					TempCell = LeftNeighbour.Get(CellIndex + i);

					if(TempCell == null)
					{
						break;
					}
					else if(TempCell.IsVisited())
					{
						return true;
					}
				}
			}

			if(RightNeighbour != null)
			{
				for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
				{
					TempCell = RightNeighbour.Get(CellIndex + i);

					if(TempCell == null)
					{
						break;
					}
					else if(TempCell.IsVisited())
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Metoda sprawdzajaca, jaka musi byc najmniejsza dlugosc slowa, ktore zaczynac sie bedzie od pola przekazanego w argumencie wywolania.
		/// </summary>
		/// <param name="StartCell">Pole, od ktorego zaczynamy ulozenie slowa</param>
		/// <param name="Vertical">true jeśli układamy slowo w pionie, false w przeciwnym razie</param>
		/// <returns></returns>
		private int GetMinLength(Cell StartCell, bool Vertical)
		{
			int MinLength = 0;

			Container ConsideredContainer;
			Container LeftNeighbour;
			Container RightNeighbour;
			int CellIndex;
			Cell TempCell;

			if(Vertical) //jesli sprawdzamy czy da sie ulozyc slowo pionowo
			{
				ConsideredContainer = GameBoard.FindColumn(StartCell);
				CellIndex = StartCell.GetYCoordinate();
				LeftNeighbour = GameBoard.FindColumn(StartCell.GetXCoordinate() - 1);
				RightNeighbour = GameBoard.FindColumn(StartCell.GetXCoordinate() + 1);
			}
			else
			{
				ConsideredContainer = GameBoard.FindRow(StartCell);
				CellIndex = StartCell.GetXCoordinate();
				LeftNeighbour = GameBoard.FindRow(StartCell.GetYCoordinate() - 1);
				RightNeighbour = GameBoard.FindRow(StartCell.GetYCoordinate() + 1);
			}

			bool FirstLetter = false; //Zmienna informujaca czy napotkalismy juz jakas litere
			for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
			{
				TempCell = ConsideredContainer.Get(CellIndex + i);

				if(TempCell == null)
				{
					break;
				}

				if(TempCell.IsVisited())
				{
					if(!FirstLetter)
					{
						FirstLetter = true;
					}
				}
				else
				{
					if(FirstLetter)
					{
						return MinLength;
					}
				}

				++MinLength;
			}

			TempCell = ConsideredContainer.Get(CellIndex + Configuration.MaxLetterNumber);
			if(TempCell != null && TempCell.IsVisited()) //Jesli siodme pole za polem startowym w rozwazanym kontenerze jest zajete
			{
				for(int i = Configuration.MaxLetterNumber; i < GameBoard.GetBoardSide(); ++i)
				{
					TempCell = ConsideredContainer.Get(CellIndex + i);

					if(TempCell == null || !TempCell.IsVisited())
					{
						return MinLength;
					}
					++MinLength;
				}
			}
			else
			{
				if(FirstLetter)
				{
					return MinLength;
				}
			}

			MinLength = 1;

			if(LeftNeighbour != null)
			{
				for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
				{
					TempCell = LeftNeighbour.Get(CellIndex + i);

					if(TempCell == null)
					{
						break;
					}

					if(TempCell.IsVisited())
					{
						return MinLength;
					}
					else
					{
						++MinLength;
					}
				}
			}

			MinLength = 1;

			if(RightNeighbour != null)
			{
				for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
				{
					TempCell = RightNeighbour.Get(CellIndex + i);

					if(TempCell == null)
					{
						break;
					}

					if(TempCell.IsVisited())
					{
						return MinLength;
					}
					else
					{
						++MinLength;
					}
				}
			}
			return MinLength;
		}

		/// <summary>
		/// Metoda napelniajaca klase AlreadySetLetters literami znajdujacymi sie na planszy.
		/// </summary>
		/// <param name="ASL"></param>
		/// <param name="ConsideredContainer"></param>
		/// <param name="FirstIndex"></param>
		private void FillAlreadySetLetters(Dictionary.Dictionary.AlreadySetLetters ASL, Container ConsideredContainer, int FirstIndex)
		{
			int LettersLeft = Configuration.MaxLetterNumber;

			for(int i = 0; LettersLeft != 0; ++i)
			{
				Cell TempCell = ConsideredContainer.Get(FirstIndex + i);

				if(TempCell == null)
				{
					return;
				}

				if(TempCell.IsVisited())
				{
					ASL.Set(i, TempCell.GetTile().GetLetter());
				}
				else
				{
					--LettersLeft;
				}
			}
		}

		/// <summary>
		/// Metoda usuwa z listy znalezionych slow slowa o niewlasciwej dlguosc
		/// </summary>
		/// <param name="MinLength">minimalna dlugosc slowa</param>
		/// <param name="MaxLength">maksymalna dlugosc slowa</param>
		/// <returns></returns>
		private Dictionary.Dictionary.WordsFound FilterWords(Dictionary.Dictionary.WordsFound WF, int MinLength, int MaxLength)
		{
			Dictionary.Dictionary.WordsFound FilteredWords = new Dictionary.Dictionary.WordsFound();

			foreach(Dictionary.Dictionary.WordFound Word in WF)
			{
				int WordLength = Word.GetWord().Length;
				if(WordLength >= MinLength && WordLength <= MaxLength)
				{
					FilteredWords.Add(Word);
				}
			}
			return FilteredWords;
		}
	}
}
