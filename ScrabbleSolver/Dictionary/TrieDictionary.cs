using System;

namespace ScrabbleSolver.Dictionary
{
	/// <summary>
	/// Ram peak: 579MB
	/// 
	/// === TrieDictionary ===
	/// Load: 11428ms
	/// Exists: 'ęą': 0ms(False)
	/// Exists: 'żyznościach': 0ms(True)
	/// Exists: 'żywotopisarstwa': 0ms(True)
	/// Exists: 'dwuwiosłowymi': 0ms(True)
	/// Exists: 'żżż': 0ms(False)
	/// Find1(n): 2ms(39)
	/// Find2(blank): 2ms(270)
	/// 
	/// Klasa implementująca słownik poprzez drzewo TRIE
	/// </summary>
	class TrieDictionary : Dictionary
	{
		/// <summary>
		/// Klasa opisująca węzeł drzewa TRIE
		/// </summary>
		private class TrieNode
		{
			//Rodzic węzła, dla korzenia null
			private readonly TrieNode Parent;

			//Pozycja węzła, dla korzenia -1
			private readonly int Position;

			//Znak zawarty w węźle, dla korzenia 0
			private readonly char Character;
			
			//Czy węzeł kończy słowo?
			private bool Ending;

			//Kodowanie słów
			private readonly Encoding.Encoding DictionaryEncoding;

			//Tablica potomków
			TrieNode[] Children;

			public TrieNode(TrieNode Parent, int Position, char Character, Encoding.Encoding DictionaryEncoding)
			{
				this.Parent = Parent;
				this.Position = Position;
				this.Character = Character;
				this.Ending = false;
				this.DictionaryEncoding = DictionaryEncoding;

				Children = null;
            }

			public int GetPosition()
			{
				return Position;
			}

			/// <summary>
			/// Sprawdź, czy podany węzeł kończy słowo
			/// </summary>
			/// <returns>true jeśli na tym węźle kończy się słowo, false jeśli nie</returns>
			public bool IsEnding()
			{
				return this.Ending;
            }

			/// <summary>
			/// Oznacz węzeł jako kończący słowo
			/// </summary>
			public void MarkEnding()
			{
				this.Ending = true;
			}

			/// <summary>
			/// Przeczytaj jako pełne słowo.
			/// 
			/// Polega na zbudowaniu słowa od korzenia do obecnego węzła.
			/// Nie sprawdza poprawności słowa ani tego czy słowo może kończyć się na obecnym węźle.
			/// </summary>
			/// <returns>Słowo</returns>
			public String Read()
			{
				TrieNode Current = this;
				char[] Result = new Char[Current.Position + 1];
				while(Current != null && Current.Position >= 0)
				{
					Result[Current.Position] = Current.Character;
					Current = Current.Parent;
				}

				return new string(Result);
			}

			/// <summary>
			/// Zwróć potomka o indeksie litery I lub null jeśli nie istnieje
			/// </summary>
			/// <param name="I">Indeks litery zgodnie z Encoding.Encoding</param>
			/// <returns>Węzeł potomka, null jeśli nie istnieje</returns>
			public TrieNode GetChildByIndex(int I)
			{
				if(Children == null)
					return null;

				return Children[I];
			}

			/// <summary>
			/// Zwróć potomka o literze C lub null jeśli nie istnieje
			/// </summary>
			/// <param name="C">Litera, zostanie przetworzona zgodnie z Encoding.Encoding</param>
			/// <returns>Węzeł potomka, null jeśli nie istnieje</returns>
			public TrieNode GetChildByCharacter(char C)
			{
				return GetChildByIndex(DictionaryEncoding.ToArrayIndex(C));
			}

			/// <summary>
			/// Zwróć potomka na podstawie litery, stwórz jeśli nie istnieje
			/// </summary>
			/// <param name="C">Litera, zostanie przetworzona zgodnie z Encoding.Encoding</param>
			/// <returns>Węzeł potomka</returns>
			public TrieNode MakeChildByCharacter(char C)
			{
                if(Children == null)
					Children = new TrieNode[DictionaryEncoding.GetArraySize()];

				int I = DictionaryEncoding.ToArrayIndex(C);
				if(Children[I] == null)
					Children[I] = new TrieNode(this, Position + 1, C, DictionaryEncoding);

				return Children[I];
			}
		}

		TrieNode Root;
		public TrieDictionary(String DictionaryPath, Encoding.Encoding DictionaryEncoding) : base(DictionaryPath, DictionaryEncoding)
		{

		}

		protected override void Clear()
		{
			Root = new TrieNode(null, -1, (char) 0, DictionaryEncoding);
		}

		protected override void AddWord(string Word)
		{
			TrieNode Parent = Root;

			foreach(char Character in Word)
				Parent = Parent.MakeChildByCharacter(Character);

			Parent.MarkEnding();

		}

		public override bool Exists(string Word)
		{
			TrieNode Current = Root;
			foreach(char Character in Word)
			{
				Current = Current.GetChildByCharacter(Character);
				if(Current == null)
					return false;
			}

			return Current.IsEnding();
        }

		/// <summary>
		/// Kolekcja opisująca wszystkie możliwe rozwiązania.
		/// 
		/// Każdym elementem kolekcji jest:
		///		- ostatni węzeł słowa (idąc w górę po drzewie można uzyskać pełne słowo)
		///		- lista liter pozostałych u gracza po stworzeniu tej kombinacji
		/// </summary>
		private class NodesFound : System.Collections.Generic.List<Tuple<TrieNode, HeldCharacters>> { }

		public override WordsFound Find(AlreadySetLetters ASL, HeldCharacters HC)
		{
			NodesFound NF = new NodesFound();
			FindRecursiveStep(NF, Root, ASL, HC);

			WordsFound WF = new WordsFound();
			foreach(var T in NF)
				WF.Add(new Dictionary.WordFound(T.Item1.Read(), T.Item2));

			return WF;
		}

		private void FindRecursiveStep(NodesFound NF, TrieNode CurrentNode, AlreadySetLetters ASL, HeldCharacters RemainingCharacters)
		{
			//Czy na obecnym znaku możemy poprzestać? Koniec słowa + przynajmniej jedna litera użyta?
			if(CurrentNode.IsEnding() && RemainingCharacters.GetAnythingUsed())
				NF.Add(new Tuple<TrieNode, HeldCharacters>(CurrentNode, RemainingCharacters));

			//Poszukiwanie kolejnego węzła.
			char CharacterAtNextPosition = ASL.Get(CurrentNode.GetPosition() + 1);

			//Czy na następnej pozycji mamy wymuszoną literę przez znak leżący już na planszy?
			if(CharacterAtNextPosition != 0) //Tak
			{
				//Czy istnieją słowa zawierające taką literę na tym miejscu? Jeśli nie - przerywamy, jeśli tak - idziemy dalej
				TrieNode ChildNode = CurrentNode.GetChildByCharacter(CharacterAtNextPosition);
				if(ChildNode != null)
					FindRecursiveStep(NF, ChildNode, ASL, RemainingCharacters);
            }
			else //Dowolny znak na tej pozycji, próbujemy każdy z posiadanych
			{
				//Zapuszczamy algorytm dalej dla każdej posiadanej literki
				for(int I = 0; I < DictionaryEncoding.GetArraySize(); I++)
				{
					//Czy są słowa z taką następną literą?
					TrieNode ChildNode = CurrentNode.GetChildByIndex(I);
					if(ChildNode == null)
						continue;

					//Czy posiadamy taką literę (ewentualne mydło zostanie dopasowane automatycznie)
					HeldCharacters NextRemainingCharacters = RemainingCharacters.GetWithoutCharacterByIndex(I);
					if(NextRemainingCharacters == null) //Nie
						continue;

					//Możemy wykonać krok na to miejsce - zapuszczamy algorytm dalej
					FindRecursiveStep(NF, ChildNode, ASL, NextRemainingCharacters);
                }
			}
		}
	}
}
