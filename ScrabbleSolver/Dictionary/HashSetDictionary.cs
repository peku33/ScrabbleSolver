using System;

namespace ScrabbleSolver.Dictionary
{
	/// <summary>
	/// Ram Peak: 189MB
	/// 
	/// === HashSetDictionary ===
	/// Load: 5044ms
	/// Exists: 'ęą': 0ms(False)
	/// Exists: 'żyznościach': 0ms(True)
	/// Exists: 'żywotopisarstwa': 0ms(True)
	/// Exists: 'dwuwiosłowymi': 0ms(True)
	/// Exists: 'żżż': 0ms(False)
	/// Find1(n): 976ms(39)
	/// Find2(blank): 2038ms(270)
	/// 
	/// 
	/// </summary>
	class HashSetDictionary : Dictionary
	{
		System.Collections.Generic.HashSet<String> AllWords;

		public HashSetDictionary(String DictionaryPath, Encoding.Encoding DictionaryEncoding) : base(DictionaryPath, DictionaryEncoding)
		{

		}
		protected override void Clear()
		{
			AllWords = new System.Collections.Generic.HashSet<String>();
		}

		protected override void AddWord(string Word)
		{
			AllWords.Add(Word);
		}

		public override bool Exists(string Word)
		{
			return AllWords.Contains(Word);
		}

		public override WordsFound Find(AlreadySetLetters ASL, HeldCharacters HC)
		{
			WordsFound WFs = new WordsFound();

			foreach(String Word in AllWords)
			{
				WordFound WF = MatchWord(Word, ASL, HC);
				if(WF != null)
					WFs.Add(WF);
			}

			return WFs;
		}

		private WordFound MatchWord(String Word, AlreadySetLetters ASL, HeldCharacters HC)
		{
			bool AnyLetterUsed = false;

			for(int I = 0; I < Word.Length; I++)
			{
				//Czy na tej pozycji istnieje konkretnie ustalony znak?
				char CharAtCurrentPosition = ASL.Get(I);

				if(CharAtCurrentPosition != 0) //Tak
				{
					//Jeśli znak nie jest taki sam - pomijamy dalsze wyszukiwanie - słowo się nie nadaje
					if(Word[I] != CharAtCurrentPosition)
						return null;
				}
				else //Nie
				{
					//Czy możemy uzupełnić to miejsce korzystając z dostępnych liter?
					HC = HC.GetWithoutCharacterByCharacter(Word[I]);
					if(HC == null) //Nie - przestajemy szukać
						return null;

					AnyLetterUsed = true;
				}
			}

			if(!AnyLetterUsed)
				return null;

			return new WordFound(Word, HC);
			
		}

		public int[] GetCharacterOccurenceCount()
		{
			int[] CharacterOccurences = new int[DictionaryEncoding.GetArraySize()];
			foreach(String Word in AllWords)
				foreach(char Character in Word)
					CharacterOccurences[DictionaryEncoding.ToArrayIndex(Character)]++;

			return CharacterOccurences;
		}
	}
}
