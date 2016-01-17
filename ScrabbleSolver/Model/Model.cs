using System;
using System.Collections.Generic;
using System.ComponentModel;
using ScrabbleSolver.Board;
using ScrabbleSolver.Common;
using ScrabbleSolver.Model.Items;
using ScrabbleSolver.Model.Player;
using Container = ScrabbleSolver.Board.Container;

namespace ScrabbleSolver.Model
{
	/// <summary>
	/// Klasa modelu gry.
	/// </summary>
	public class Model
	{
		private readonly List<Player.Player> Players;
		private readonly Dictionary.Dictionary GameDictionary;
		private readonly Language.Language GameLanguage;
		private Board.Board GameBoard;
		private TilesSet TilesSet;
		private Player.Player CurrentPlayer;

		//Licznik spasowan. Gra konczy sie gdy wszyscy gracze spasuja.
		private int PassCounter;

		public Model(Dictionary.Dictionary GameDictionary, Language.Language Language)
		{
			this.GameLanguage = Language;
			TilesSet = new TilesSet(Language);
			this.Players = new List<Player.Player>();
			this.GameBoard = new Board.Board();
			this.GameDictionary = GameDictionary;
			this.PassCounter = 0;
		}

		public void AddPlayer(Player.Player NewPlayer)
		{
			Players.Add(NewPlayer);
			NewPlayer.GetNewTiles();
		}

		public void ResetGame()
		{
			GameBoard = new Board.Board();
			Players.Clear();
			TilesSet = new TilesSet(GameLanguage);
			PassCounter = 0;
		}

		public Dictionary.Dictionary GetDictionary()
		{
			return this.GameDictionary;
		}

		public Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, String>> GetGameInfo()
		{
			Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, String>> GameInfo = new Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, string>>();
			int i = 0;

			foreach(Player.Player TempPlayer in Players)
			{
				Dictionary<GameInfoTypeEnum, String> PlayerInfo = new Dictionary<GameInfoTypeEnum, string>();
				PlayerInfo.Add(GameInfoTypeEnum.PLAYER_SCORE, TempPlayer.GetPointsNumber().ToString());
				GameInfo.Add((PlayerIdEnum)i, PlayerInfo);
				i++;
			}

			return GameInfo;
		}

		public Dictionary<PlayerIdEnum, List<Tile>> GetHeldCharacters()
		{
			Dictionary<PlayerIdEnum, List<Tile>> HeldCharacters = new Dictionary<PlayerIdEnum, List<Tile>>();
			int i = 0;

			foreach(Player.Player TempPlayer in Players)
			{
				HeldCharacters.Add((PlayerIdEnum)i, TempPlayer.GetRack());
				i++;
			}

			return HeldCharacters;
		}

		public PlayerIdEnum GetCurrentPlayerIndex()
		{
			int i = 0;

			for(; i < Players.Count; ++i)
			{
				if(Players[i] == CurrentPlayer)
				{
					return (PlayerIdEnum)i;
				}
			}
			return (PlayerIdEnum)i;
		}

		public List<Cell> GetBoardCells()
		{
			return GameBoard.GetBoardCells();
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

		public void ResetPassCounter()
		{
			PassCounter = 0;
		}

		public void PlayerPassed()
		{
			++PassCounter;
		}

		public void SetCurrentPlayer(int Index)
		{
			CurrentPlayer = Players[Index];
		}

		public Player.Player GetCurrentPlayer()
		{
			return CurrentPlayer;
		}

		public bool IsHumanTurn()
		{
			return CurrentPlayer.GetType() == typeof(HumanPlayer);
		}

		public void NextAITurn()
		{
			if(GameBoard.IsEmpty() && CurrentPlayer != null)
			{
				CurrentPlayer.MakeFirstMove(null);
			}
			else
			{
				CurrentPlayer.MakeMove(null);
			}
			Console.ReadLine(); //Czekanie na klawisz na potrzeby testow
		}

		/// <summary>
		/// Gra konczy sie gdy dowolnemu graczowi zabraknie kostek lub gdy zaden z graczy nie ma ruchu
		/// </summary>
		/// <returns></returns>
		public bool IsEnd()
		{
			if(PassCounter == GetPlayersNumber() * 2) //Koniec gry nastepuje gdy kazdy z graczy bedzie pasowac przez dwie kolejki
			{
				return true;
			}

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

		/// <summary>
		/// Metoda sprawdza czy na planszy z widoku pojawily sie nowe kostki i czy zostaly wstawione zgodnie z zasadami gry. 
		/// </summary>
		/// <returns>Lista nowowstawionych kostek i flaga informujaca czy ruch odbywa sie w pionie czy w poziomie lub null jesli ruch jest niepoprawny</returns>
		public Tuple<List<Cell>, bool> CheckAndGetNewCells(List<Cell> NewBoardCells)
		{
			List<Cell> Cells = new List<Cell>();
			bool isVertical = false;

			foreach(Cell TempCell in NewBoardCells)
			{
				Cell OldCell = GameBoard.GetCell(TempCell.GetXCoordinate(), TempCell.GetYCoordinate());

				if(OldCell != null && OldCell.GetTile() == null && TempCell.GetTile() != null)
				{
					Cells.Add(TempCell);
				}
			}

			if(Cells.Count == 0) //Nic nie zostalo wstawione => gracz pasuje
			{
				return new Tuple<List<Cell>, bool>(Cells, false);
			}

			if(Cells.Count > 1)
			{
				if(Cells[0].GetXCoordinate() == Cells[1].GetXCoordinate())
				{
					isVertical = true;
				}

				if(!ContainedInOneContainer(Cells))
				{
					return null;
				}
			}

			Container Container;
			int StartIndex = GameBoard.GetBoardSide();
			int EndIndex = 0;

			if(isVertical)
			{
				foreach(Cell TempCell in Cells)
				{
					int y = TempCell.GetYCoordinate();
					if(y > EndIndex)
					{
						EndIndex = y;
					}
					if(y < StartIndex)
					{
						StartIndex = y;
					}
				}
			}
			else
			{
				foreach(Cell TempCell in Cells)
				{
					int x = TempCell.GetXCoordinate();
					if(x > EndIndex)
					{
						EndIndex = x;
					}
					if(x < StartIndex)
					{
						StartIndex = x;
					}
				}
			}
			if(isVertical)
			{
				Container = GameBoard.FindRow(Cells[0]);
			}
			else
			{
				Container = GameBoard.FindColumn(Cells[0]);
			}

			if(!IsOneWord(Container, StartIndex, EndIndex))
			{
				return null;
			}

			return new Tuple<List<Cell>, bool>(Cells, isVertical);
		}

		/// <summary>
		/// Metoda sprawdza czy wszystkie komorki z listy znajduja sie w tym samym rzedzie lub kolumnie
		/// </summary>
		/// <param name="Cells"></param>
		/// <returns></returns>
		public bool ContainedInOneContainer(List<Cell> Cells)
		{
			if(Cells.Count == 0)
			{
				return true;
			}

			int x = Cells[0].GetXCoordinate();
			int y = Cells[0].GetYCoordinate();
			bool Vertical = true;

			foreach(Cell TempCell in Cells)
			{
				if(TempCell.GetXCoordinate() != x)
				{
					Vertical = false;
					break;
				}
			}

			if(Vertical)
			{
				return true;
			}

			foreach(Cell TempCell in Cells)
			{
				if(TempCell.GetYCoordinate() != y)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Metoda sprawdzajaca czy pomiedzy indeksami przekazanymi w argumencie wywolania nie ma pustego pola
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartIndex"></param>
		/// <param name="EndIndex"></param>
		/// <returns></returns>
		private bool IsOneWord(Container Container, int StartIndex, int EndIndex)
		{
			for(; StartIndex > EndIndex; ++StartIndex)
			{
				if(Container.Get(StartIndex).GetTile() == null)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Metoda sprawdza czy da sie zgodnie z zasadami gry ulozyc slowo, ktora zaczyna sie od pola przekazanego w argumencie wywolania
		/// </summary>
		/// <param name="StartCell"></param>
		/// <param name="Vertical">Flaga informujaca czy sprawdzamy czy slowo da sie ulozyc w pionie czy poziomie</param>
		/// <returns></returns>
		public bool IsPositionCorrect(Cell StartCell, bool Vertical)
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

			if(GetFirstLetterDistance(ConsideredContainer, CellIndex + 1) <= Configuration.MaxLetterNumber) //plus jeden bo zaczynamy od pierwszego zajetego pola
			{
				return true;
			}
			if(GetFirstLetterDistance(LeftNeighbour, CellIndex) <= Configuration.MaxLetterNumber)
			{
				return true;
			}
			if(GetFirstLetterDistance(RightNeighbour, CellIndex) <= Configuration.MaxLetterNumber)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Metoda sprawdzajaca, jaka musi byc najmniejsza dlugosc slowa, ktore zaczynac sie bedzie od pola przekazanego w argumencie wywolania, aby ulozenie go moglo byc zgodne z zasadami gry.
		/// </summary>
		/// <param name="StartCell">Pole, od ktorego zaczynamy ulozenie slowa</param>
		/// <param name="Vertical">true jeśli układamy slowo w pionie, false w przeciwnym razie</param>
		/// <returns></returns>
		public int GetMinLength(Cell StartCell, bool Vertical)
		{
			Container ConsideredContainer;
			Container LeftNeighbour;
			Container RightNeighbour;
			int CellIndex;
			Cell TempCell;

			int MinLength = 0;
			int ActualMinLength = GameBoard.GetBoardSide() + 1;

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

			bool FirstLetter = false; //czy napotkalismy juz jakas litere
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
						ActualMinLength = MinLength;
						break;
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
						ActualMinLength = ActualMinLength > MinLength ? MinLength : ActualMinLength;
						break;
					}
					++MinLength;
				}
			}
			else
			{
				if(FirstLetter)
				{
					ActualMinLength = ActualMinLength > MinLength ? MinLength : ActualMinLength;
				}
			}

			MinLength = GetFirstLetterDistance(LeftNeighbour, CellIndex);
			ActualMinLength = ActualMinLength > MinLength ? MinLength : ActualMinLength;

			MinLength = GetFirstLetterDistance(RightNeighbour, CellIndex);
			ActualMinLength = ActualMinLength > MinLength ? MinLength : ActualMinLength;

			return ActualMinLength;
		}


		/// <summary>
		/// Metoda napelniajaca klase AlreadySetLetters literami znajdujacymi sie na planszy.
		/// </summary>
		/// <param name="ASL"></param>
		/// <param name="ConsideredContainer"></param>
		/// <param name="FirstIndex"></param>
		public void FillAlreadySetLetters(Dictionary.Dictionary.AlreadySetLetters ASL, Container ConsideredContainer, int FirstIndex)
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
		public Dictionary.Dictionary.WordsFound FilterWords(Dictionary.Dictionary.WordsFound WF, int MinLength, int MaxLength)
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

		/// <summary>
		/// Metoda sprawdzajaca czy wlozenie slowa nie sprawi ze na planszy znajda sie ciagi znakow nie bedace slowami
		/// </summary>
		/// <param name="Word"></param>
		/// <param name="StartCell"></param>
		/// <param name="Vertical"></param>
		/// <returns></returns>
		public bool IsMoveCorrect(String Word, Cell StartCell, bool Vertical)
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
		/// Metoda zwraca slowo, ktore przechodzi przez index podany w argumecnie wywolania
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="Index"></param>
		/// <returns></returns>
		public String GetWord(Container Container, int Index)
		{
			String NewWord = String.Empty;

			foreach(Cell TempCell in Container)
			{
				if(TempCell.GetTile() != null)
				{
					NewWord += TempCell.GetTile().GetLetter();
				}
				else if(TempCell.GetXCoordinate() > Index) //Ulozone zostalo cale slowo
				{
					if(NewWord.Length > 1)
					{
						return NewWord;
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
				return NewWord;
			}

			return NewWord;
		}


		/// <summary>
		/// Metoda zliczajaca punkty ktore otrzyma gracz za ulozenie okreslonego slowa w okreslone miejsce
		/// </summary>
		/// <param name="Word"></param>
		/// <param name="StartCell"></param>
		/// <param name="Vertical"></param>
		/// <returns></returns>
		public int CountPoints(String Word, Cell StartCell, bool Vertical)
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
		/// Metoda zliczajaca ile punktow warte jest slowo
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartIndex"></param>
		/// <returns></returns>
		public int CountWord(Container Container, int StartIndex)
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

				Char Letter = TempTile.GetLetter();
				int Value = TempTile.IsBlank() ? GameLanguage.GetLetterValue(' ') : GameLanguage.GetLetterValue(Letter);

				if(TempCell.IsVisited())
				{
					Points += Value;
				}
				else
				{
					++LettersUsed;
					WordMultiplier *= TempCell.GetWordMultiplier();
					Points += Value * TempCell.GetLetterMultiplier();
				}
			}

			if(LettersUsed == Configuration.MaxLetterNumber)
			{
				return Points * WordMultiplier + Configuration.SpecialBonus;
			}
			return Points * WordMultiplier;
		}

		/// <summary>
		/// Metoda wstawia kostki w odpowiednie miejsce na planszy i usuwa je z tabliczki gracza
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartCell"></param>
		/// <param name="Word"></param>
		/// <param name="Rack"></param>
		public void PutTiles(Container Container, int StartIndex, Dictionary.Dictionary.WordFound Word, Rack Rack)
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

		public void PutTiles(List<Cell> Cells)
		{
			foreach(Cell TempCell in Cells)
			{
				GameBoard.GetCell(TempCell.GetXCoordinate(), TempCell.GetYCoordinate()).SetTile(TempCell.GetTile());
			}
		}

		public void RemoveTiles(List<Cell> Cells)
		{
			foreach(Cell TempCell in Cells)
			{
				GameBoard.GetCell(TempCell.GetXCoordinate(), TempCell.GetYCoordinate()).SetTile(null);
			}
		}

		public void SetVisited(List<Cell> Cells)
		{
			foreach(Cell TempCell in Cells)
			{
				GameBoard.GetCell(TempCell.GetXCoordinate(), TempCell.GetYCoordinate()).SetVisited(true);
			}
		}

		/// <summary>
		/// Metoda wstawia kostki w odpowiednie miejsce na planszy, usuwa je z tabliczki i oznacza pola jako odwiedzone - wykorzystywana przy ostatecznym wstawianiu kostek na plansze
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="StartCell"></param>
		/// <param name="Word"></param>
		/// <param name="Rack"></param>
		public void PutAndSetTiles(Container Container, int StartIndex, Dictionary.Dictionary.WordFound Word, Rack Rack)
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
		/// <param name="Rack"></param>
		public void RemoveTiles(Container Container, int StartIndex, int Number, Rack Rack)
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
		/// Metoda zwracajaca informacje o slowie, ktore przechodzi przez pole o indeksie Index w kontenerze podanym 
		/// w argumencie wywolania.
		/// </summary>
		/// <param name="Container">Plaszyzna z ktorej odczytujemy slowo</param>
		/// <param name="Index">Indeks, na ktorym nie ma jeszcze ulozonej kostki</param>
		/// <returns>tuple zawierajacy dlugosc slowa oraz indeks od ktorego slowo sie zaczyna w kontenerze Container</returns>
		public Tuple<int, int> GetWordInfo(Container Container, int Index)
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

		/// <summary>
		/// Zliczanie odleglosci od wskazanego indeksu w kontenerze do pierwszego indeksu na ktorym znajduje sie kostka. 
		/// </summary>
		/// <param name="Container"></param>
		/// <param name="Index"></param>
		/// <returns>Odleglosc do pierwszego zajetego pola lub rozmiar boku planszy + 1 jesli takie pole nie istnieje</returns>
		private int GetFirstLetterDistance(Container Container, int Index)
		{
			int Distance = 1;

			if(Container != null)
			{
				for(int i = 0; i < Configuration.MaxLetterNumber; ++i)
				{
					Cell TempCell = Container.Get(Index + i);

					if(TempCell == null)
					{
						break;
					}

					if(TempCell.IsVisited())
					{
						return Distance;
					}
					++Distance;
				}
			}
			return GameBoard.GetBoardSide() + 1;
		}
	}
}
