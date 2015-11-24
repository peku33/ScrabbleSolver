using System;
using System.Text;
using System.Linq;

namespace ScrabbleSolver.Dictionary
{
	class Dictionary
	{
		private static readonly int MaxWordLength = 15;
		private static readonly int MaxInputWordLength = 128;

		WordSet AllWords = null;

		public Dictionary(String DictionaryPath)
		{
			AllWords = new WordSet();

			System.IO.StreamReader DictionaryFileReader = new System.IO.StreamReader(DictionaryPath);
			System.Text.StringBuilder SB = null;

			while(!DictionaryFileReader.EndOfStream)
			{
                char CharacterRead = (char) DictionaryFileReader.Read();

				if(char.IsLetter(CharacterRead))
				{
					if(SB == null)
						SB = new StringBuilder(MaxInputWordLength);

					SB.Append((char) CharacterRead);
				}
				else
				{
					if(SB != null && SB.Length > 0)
					{
						if(SB.Length <= MaxWordLength)
						{
							AllWords.Add(SB.ToString());
                        }

						SB = null;
					}
				}
			}

			DictionaryFileReader.Dispose();
        }

		public WordSet FindWordsWithCharAtPosition(char C, int P)
		{
			WordSet WS = new WordSet();
			foreach(String Word in AllWords)
			{
				if(Word.Length <= P)
					continue;

				if(Word[P] != C)
					continue;

				WS.Add(Word);
			}

			return WS;
		}

		public WordSet FindWordsToBeCreatedWithCharacters(String Characters)
		{
			System.Collections.Generic.Dictionary<char, int> CharacterCounts = new System.Collections.Generic.Dictionary<char, int>();
			foreach(char Character in Characters)
				if(CharacterCounts.ContainsKey(Character))
					CharacterCounts[Character]++;
				else
					CharacterCounts.Add(Character, 1);



			WordSet WS = new WordSet();
			foreach(String Word in AllWords)
			{
				if(Word.Length > Characters.Length)
					continue;

				bool WordFits = true;

				//Odfiltrowujemy słowa zawierające litery których nie mamy
				foreach(char WordChar in Word)
				{
					if(!CharacterCounts.ContainsKey(WordChar))
					{
						WordFits = false;
						break;
                    }
				}

				if(!WordFits)
					continue;

				//Odfiltrowujemy słowa zawierające więcej ilości danej litery, niż mamy
				foreach(System.Collections.Generic.KeyValuePair<char, int> CharacterCount in CharacterCounts)
				{
					if(Word.Count(WordCharacter => WordCharacter == CharacterCount.Key) > CharacterCount.Value)
					{
						WordFits = false;
						break;
					}
				}

				if(!WordFits)
					continue;

				WS.Add(Word);
            }

			return WS;
		}
	}
}
