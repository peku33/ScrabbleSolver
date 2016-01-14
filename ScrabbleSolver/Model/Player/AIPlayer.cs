using System;
using System.Collections.Generic;
using ScrabbleSolver.Board;
using ScrabbleSolver.Events;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca gracza komputerowego.
	/// </summary>
	class AIPlayer : Player
	{

		public AIPlayer(Model GameModel) : base(GameModel)
		{
			this.Name = "CPU";
		}

		/// <summary>
		/// Procedura wykonywania ruchu przez algorytm gracza komputerowego. Dla kazdego pola, algorytm sprawdza czy można zgodnie z zasadami gry wstawić słowo w taki sposób, że pierwsza jego litera
		/// znajdować będzie się na rozpatrywanym polu. Następnie algorytm wyszukuje najlepiej punktowane słowo i zapisuje je (a także zapisuje informacje o tym, gdzie słowo powinno zostać
		/// umieszczone). Po wykonaniu procedury dla wszystkich pól, algorytm wstawia najlepsze znalezione słowo. Jeśli gracz nie ma możliwości wstawienia słowa w żadne miejsce, wymienia losową kostkę lub 
		/// ustawia flage informujaca ze nie jest w stanie wykonac juz ruchu jesli zestaw kostek jest pusty.
		/// </summary>
		public override bool MakeMove(PutWordEvent MoveEvent)
		{
			int BestResult = 0;
			int BestStartIndex = 0;
			Dictionary.Dictionary.WordFound BestWord = null;
			Container BestContainer = null;

			Board.Board GameBoard = GameModel.GetBoard();
			Dictionary.Dictionary GameDictionary = GameModel.GetDictionary();
			Dictionary.Dictionary.HeldCharacters HC = new Dictionary.Dictionary.HeldCharacters(GameDictionary.GetDictionaryEncoding());
			HC.Add(Rack.GetTileString());

			int MinLength = 0;
			int MaxLength = GameBoard.GetBoardSide();

			foreach(Row TempRow in GameBoard.GetRows())
			{
				for(int i = 0; i < GameBoard.GetBoardSide() - 1; ++i)
				{
					Cell TempCell = TempRow.Get(i);

					if(GameModel.IsPositionCorrect(TempCell, false)) //Czy da sie ustawic slowo zaczynajac od tego pola tak, aby bylo to zgodne z regulami gry
					{
						MaxLength = GameBoard.GetBoardSide() - TempCell.GetXCoordinate();
						MinLength = GameModel.GetMinLength(TempCell, false);

						Dictionary.Dictionary.AlreadySetLetters ASL = new Dictionary.Dictionary.AlreadySetLetters();
						GameModel.FillAlreadySetLetters(ASL, TempRow, TempCell.GetXCoordinate());

						Dictionary.Dictionary.WordsFound WordsFound = GameDictionary.Find(ASL, HC);
						WordsFound = GameModel.FilterWords(WordsFound, MinLength, MaxLength);

						foreach(Dictionary.Dictionary.WordFound TempWord in WordsFound)
						{
							if(GameModel.IsMoveCorrect(TempWord.GetWord(), TempCell, false)) //Czy ustawienie tego slowa nie spowoduje kolizji w drugiej plaszczyznie
							{
								GameModel.PutTiles(TempRow, TempCell.GetXCoordinate(), TempWord, Rack);

								int Result = GameModel.CountPoints(TempWord.GetWord(), TempCell, false);
								if(Result > BestResult)
								{
									BestResult = Result;
									BestStartIndex = TempCell.GetXCoordinate();
									BestWord = TempWord;
									BestContainer = TempRow;
								}

								GameModel.RemoveTiles(TempRow, TempCell.GetXCoordinate(), TempWord.GetWord().Length, Rack);
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

					if(GameModel.IsPositionCorrect(TempCell, true))
					{
						MaxLength = GameBoard.GetBoardSide() - TempCell.GetYCoordinate();
						MinLength = GameModel.GetMinLength(TempCell, true);

						Dictionary.Dictionary.AlreadySetLetters ASL = new Dictionary.Dictionary.AlreadySetLetters();
						GameModel.FillAlreadySetLetters(ASL, TempColumn, TempCell.GetYCoordinate());

						Dictionary.Dictionary.WordsFound WordsFound = GameDictionary.Find(ASL, HC);
						WordsFound = GameModel.FilterWords(WordsFound, MinLength, MaxLength);

						foreach(Dictionary.Dictionary.WordFound TempWord in WordsFound)
						{
							if(GameModel.IsMoveCorrect(TempWord.GetWord(), TempCell, true))
							{
								GameModel.PutTiles(TempColumn, TempCell.GetYCoordinate(), TempWord, Rack);

								int Result = GameModel.CountPoints(TempWord.GetWord(), TempCell, true);
								if(Result > BestResult)
								{
									BestResult = Result;
									BestStartIndex = TempCell.GetYCoordinate();
									BestWord = TempWord;
									BestContainer = TempColumn;
								}

								GameModel.RemoveTiles(TempColumn, TempCell.GetYCoordinate(), TempWord.GetWord().Length, Rack);
							}
						}
					}
				}
			}

			if(BestWord == null || BestWord.GetWord().Length < 2) //Jesli nie da sie ulozyc zadnego slowa
			{
				if(GameModel.GetTilesSet().IsEmpty()) //Jesli nie ma juz z czego dobierac
				{
					Pass();
				}
				else
				{
					ReplaceTile();
					GameModel.ResetPassCounter();
				}
				return true;
			}
			PointsNumber += BestResult;
			GameModel.PutAndSetTiles(BestContainer, BestStartIndex, BestWord, Rack);
			GetNewTiles();
		}

		public override void MakeFirstMove(PutWordEvent MoveEvent)
		{
			int BestResult = 0;
			int BestStartIndex = 0;
			Dictionary.Dictionary.WordFound BestWord = null;
			Board.Board GameBoard = GameModel.GetBoard();
			Dictionary.Dictionary GameDictionary = GameModel.GetDictionary();

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
						GameModel.PutTiles(CenterRow, TempCell.GetXCoordinate(), TempWord, Rack);

						int Result = GameModel.CountWord(CenterRow, TempCell.GetXCoordinate());
						if(Result > BestResult)
						{
							BestResult = Result;
							BestStartIndex = TempCell.GetXCoordinate();
							BestWord = TempWord;
						}

						GameModel.RemoveTiles(CenterRow, TempCell.GetXCoordinate(), TempWord.GetWord().Length, Rack);
					}
				}
			}

			if(BestWord == null || BestWord.GetWord().Length < 2) //Jesli nie da sie ulozyc zadnego slowa
			{
				ReplaceTile();
				return;
			}

			PointsNumber += BestResult;
			GameModel.PutAndSetTiles(CenterRow, BestStartIndex, BestWord, Rack);
			GameBoard.SetEmpty(false);
			GetNewTiles();
		}

		/// <summary>
		/// Wymiana kostki. Z tabliczki zostaje wyjeta losowa kostka, ktora nie jest blankiem - blanka nigdy nie oplaca sie wymieniac.
		/// </summary>
		public override void ReplaceTile(ReplaceTileEvent ReplaceTile)
		{
			Tile TempTile;

			if(GameModel.GetTilesSet().IsEmpty())
			{
				return;
			}
			while((TempTile = Rack.GetRandomTile()).GetLetter().Equals(' '))
			{
				Rack.Add(TempTile);
			}

			Rack.Add(GameModel.GetTilesSet().GetRandomTile());
		}

		public override void Pass()
		{
			GameModel.PlayerPassed();
		}
	}
}
