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
							AllWords.Add(SB.ToString());

						SB = null;
					}
				}
			}

			DictionaryFileReader.Dispose();
        }

		public WordSet FindWordsToBeCreatedWithCharacters(String Characters)
		{
			//Create a dictionary of characters we have
			//Character => occurence count
			System.Collections.Generic.Dictionary<char, int> CharacterCounts = new System.Collections.Generic.Dictionary<char, int>();
			foreach(char Character in Characters)
				if(CharacterCounts.ContainsKey(Character))
					CharacterCounts[Character]++;
				else
					CharacterCounts.Add(Character, 1);


			WordSet WS = new WordSet();

			foreach(String Word in AllWords)
			{
				//If word is longer than number of characters we have
				if(Word.Length > Characters.Length)
					continue;

				bool WordFits = true;

				//Filtering out words containing characters we dont have
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

				//Filtering out words having more occurences of specific character than we have
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

				//Everything else is ok
				WS.Add(Word);
            }

			return WS;
		}
	}
}
