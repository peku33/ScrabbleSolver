using System;

namespace ScrabbleSolver.Dictionary
{
	/// <summary>
	/// Klasa dostarzająca słownik wraz z operacjami
	/// 
	/// </summary>
	abstract class Dictionary
	{
		//Maksymalna długość słowa, słowa dłuższe będą pominięte. Wartość wynika z rozmiaru planszy
		protected static readonly int MaxWordLength = 15;

		//Rozmiar bufora wczytującego słowa, dla bezpieczeństwa 128
		private static readonly int MaxInputWordLength = 128;

		//Ścieżka do pliku słownika
		private readonly String DictionaryPath;

		//Enkoder pozwalający stworzyć tablicę gdzie kluczami są znaki a wartościami dowolne obiekty. Przemapowuje litery na indeksy od 0, bez dziur.
		protected readonly Encoding.Encoding DictionaryEncoding;

		protected Dictionary(String DictionaryPath, Encoding.Encoding DictionaryEncoding)
		{
			this.DictionaryPath = DictionaryPath;
			this.DictionaryEncoding = DictionaryEncoding;
		}

		public Encoding.Encoding GetDictionaryEncoding()
		{
			return DictionaryEncoding;
		}

		/// <summary>
		/// Przeładowanie słownika z pliku
		/// </summary>
		public void Reload()
		{
			Clear();

			System.IO.StreamReader DictionaryFileReader = new System.IO.StreamReader(DictionaryPath);
			System.Text.StringBuilder SB = null;

			while(!DictionaryFileReader.EndOfStream)
			{
				char CharacterRead = (char) DictionaryFileReader.Read();

				if(char.IsLetter(CharacterRead))
				{
					if(SB == null)
						SB = new System.Text.StringBuilder(MaxInputWordLength);

					SB.Append((char) CharacterRead);
				}
				else
				{
					if(SB != null && SB.Length > 0)
					{
						if(SB.Length <= MaxWordLength)
							AddWord(SB.ToString());

						SB = null;
					}
				}
			}

			DictionaryFileReader.Dispose();
		}

		/// <summary>
		/// Dodaj złosowo do słownika
		/// </summary>
		/// <param name="Word">Słowo do dodania</param>
		abstract protected void AddWord(String Word);

		/// <summary>
		/// Wyczyść słownik, usuń wszystkie wyrazy
		/// </summary>
		abstract protected void Clear();

		/// <summary>
		/// Czy podane słowo istnieje w słowniku?
		/// </summary>
		/// <param name="Word">Słowo do wyszukania</param>
		/// <returns>true jeśli słowo istnieje, false jeśli nie</returns>
		abstract public bool Exists(string Word);


		/// <summary>
		/// Klasa opisująca układ już istniejących liter.
		/// 
		/// Realizuje mapowanie 'Pozycja' => Litera
		/// </summary>
		public class AlreadySetLetters
		{
			private char[] Characters;

			public AlreadySetLetters()
			{
				Characters = new char[MaxWordLength];
				for(int I = 0; I < MaxWordLength; I++)
					Characters[I] = (char) 0;
			}

			/// <summary>
			/// Zwraca literę na danej pozycji, 0 jeśli pozycja jest pusta
			/// </summary>
			/// <param name="Position">Pozycja, gdzie 0 oznacza pierwszą</param>
			/// <returns>Znak na danej pozycji, 0 jeśli pozycja jest pusta</returns>
			public char Get(int Position)
			{
				return Characters[Position];
			}

			/// <summary>
			/// Ustawia znak na danej pozycji
			/// </summary>
			/// <param name="Position">Pozycja, gdzie 0 oznacza pierwszą</param>
			/// <param name="Character">Znak na danej pozycji</param>
			public void Set(int Position, char Character)
			{
				if(Characters[Position] != (char) 0)
					throw new OverflowException("Character at position already set");

				Characters[Position] = Character;
			}
		}

		/// <summary>
		/// Klasa reprezentująca aktualnie posiadane do wykorzystania litery.
		/// </summary>
		public class HeldCharacters
		{
			//Encodig.Encoding służący do napełniania tablicy znaków
			private Encoding.Encoding DictionaryEncoding;

			//Tablica zawierająca ilość dostępnych znaków. Kluczem jest kod znaku podany przez Encoding.Encoding a wartością liczba dostępnych elementów. Istnieje dodatkowo jeden element tablicy więcej, na końcu, zawierający ilość dostępnych mydeł
			private char[] CharactersCount;

			public HeldCharacters(Encoding.Encoding DictionaryEncoding)
			{
				this.DictionaryEncoding = DictionaryEncoding;

				CharactersCount = new char[DictionaryEncoding.GetArraySize() + 1];
				for(int I = 0; I < CharactersCount.Length; I++)
					CharactersCount[I] = (char) 0;
			}

			private HeldCharacters(Encoding.Encoding DictionaryEncoding, char[] CharactersCount)
			{
				this.DictionaryEncoding = DictionaryEncoding;
				this.CharactersCount = CharactersCount;
			}

			/// <summary>
			/// Dodaje literę do listy posiadanych. Spacja oznacza mydło / blank.
			/// </summary>
			/// <param name="C">Litera do dodania, spacja oznacza mydło</param>
			public void Add(char C)
			{
				if(C == ' ')
					CharactersCount[DictionaryEncoding.GetArraySize()]++;
				else
					CharactersCount[DictionaryEncoding.ToArrayIndex(C)]++;
			}

			public void Add(String S)
			{
				foreach(char C in S)
					Add(C);
			}

			/// <summary>
			/// Zwraca nowy obiekt tej klasy z usuniętą literą o podanym indeksie.
			/// 
			/// W pierwszej kolejności sprawdzane jest, czy podana litera może zostać pobrana (czy istnieje w zbiorze)
			/// Jeśli nie, sprawdza, czy możemy użyć mydła
			/// Jeśli nie, zwraca null
			/// 
			/// Zwraca zestaw znaków pomniejszony o zabraną literę.
			/// 
			/// Wątkowo bezpieczne
			/// </summary>
			/// <param name="I">Kod znaku do pobrania</param>
			/// <returns>Zestaw znaków pomniejszony o zabraną literę lub null jeśli ruch nie jest możliwy</returns>
			public HeldCharacters GetWithoutCharacterByIndex(int I)
			{
				if(CharactersCount[I] <= 0) //Brak bezpośrednio tego znaku
				{
					//Jest mydło?
					I = DictionaryEncoding.GetArraySize();
					if(CharactersCount[I] <= 0)
						return null;
				}

				char[] NewCharactersCount = (char[]) CharactersCount.Clone();
				NewCharactersCount[I]--;
				return new HeldCharacters(DictionaryEncoding, NewCharactersCount);
			}

			/// <summary>
			/// j./w. ale pzyjmuje literę, a nie jej kod
			/// </summary>
			/// <param name="C">Litera do pobrania</param>
			/// <returns>Zestaw znaków pomniejszony o zabraną literę lub null jeśli ruch nie jest możliwy</returns>
			public HeldCharacters GetWithoutCharacterByCharacter(char C)
			{
				return GetWithoutCharacterByIndex(DictionaryEncoding.ToArrayIndex(C));
			}
		}


		/// <summary>
		/// Klasa opisująca znalezione słowo oraz litery pozostałe po jego ułożeniu
		/// </summary>
		public class WordFound
		{
			//Słowo
			private readonly String Word;

			//Znaki pozostałe po ułożeniu słowa
			private readonly HeldCharacters RemainingCharacters;

			public WordFound(String Word, HeldCharacters RemainingCharacters)
			{
				this.Word = Word;
				this.RemainingCharacters = RemainingCharacters;
			}

			public String GetWord()
			{
				return Word;
			}
			public HeldCharacters GetRemainingCharacters()
			{
				return RemainingCharacters;
			}
		}

		public class WordsFound : System.Collections.Generic.List<WordFound> { } //

		abstract public WordsFound Find(AlreadySetLetters ASL, HeldCharacters HC);
	}
}
