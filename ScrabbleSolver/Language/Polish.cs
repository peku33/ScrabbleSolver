using System;

namespace ScrabbleSolver.Language
{
	class Polish : Language
	{
		public Polish()
		{
			Encoding = new Encoding.Polish();
			Letters = "aąbcćdeęfghijklłmnńoóprsśtuwyzżź ";
			Values = new[] {1, 3, 2, 3, 1, 5, 3, 3, 1, 3, 2, 3, 2, 1, 1, 2, 0, 2, 2, 2, 3, 0, 2, 0, 1, 1, 3, 8, 4, 3, 7, 6, 3, 4, 9, 0};
			LetterNumber = new[] {9, 2, 4, 2, 6, 1, 2, 1, 8, 2, 3, 2, 3, 6, 7, 3, 0, 4, 3, 2, 2, 0, 5, 0, 5, 5, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2};
		}

		public override Encoding.Encoding GetEncoding()
		{
			return Encoding;
		}

		public override int GetLetterValue(char C)
		{
			if(C == ' ')
			{
				return Values[Values.Length - 1];
			}
			return Values[Encoding.ToArrayIndex(C)];
		}

		public override int GetLetterNumber(char C)
		{
			if(C == ' ')
			{
				return LetterNumber[Values.Length - 1];
			}
			return LetterNumber[Encoding.ToArrayIndex(C)];
		}
	}
}
