using System;

namespace ScrabbleSolver.Encoding
{
	class Polish : Encoding
	{
		public override int GetArraySize()
		{
			return 35;
		}

		public override int ToArrayIndex(char C)
		{
			if(C >= 'a' && C <= 'z')
				return C - 'a';

			switch(C)
			{
				case 'ą': return 26;
				case 'ć': return 27;
				case 'ę': return 28;
				case 'ł': return 29;
				case 'ń': return 30;
				case 'ó': return 31;
				case 'ś': return 32;
				case 'ż': return 33;
				case 'ź': return 34;
				default: throw new System.ArgumentException("Char " + C + " out of bounds");
			}
		}

		public override char FromArrayIndex(int I)
		{
			if(I < 26)
				return (char) (I + 'a');

			switch(I)
			{
				case 26: return 'ą';
				case 27: return 'ć';
				case 28: return 'ę';
				case 29: return 'ł';
				case 30: return 'ń';
				case 31: return 'ó';
				case 32: return 'ś';
				case 33: return 'ż';
				case 34: return 'ź';
				default: throw new System.ArgumentException("Index " + I + " out of bounds");
			}
		}

		public override bool IsLetterValid(char C)
		{
			try
			{
				ToArrayIndex(C);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
