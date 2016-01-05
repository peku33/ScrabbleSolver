using System;

namespace ScrabbleSolver.Language
{
	public abstract class Language
	{
		protected Encoding.Encoding Encoding;

		//Litery dostepne w grze
		protected string Letters;

		//Wartosc litery
		protected int[] Values;

		//Liczba liter w grze
		protected int[] LetterNumber;

		public abstract Encoding.Encoding GetEncoding();

		public abstract int GetLetterValue(char C);

		public abstract int GetLetterNumber(char C);

		public int GetLettersNumber()
		{
			return Letters.Length;
		}

		public string GetLetters()
		{
			return Letters;
		}
	}
}
