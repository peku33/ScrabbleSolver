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
				case 'ź': return 33;
				case 'ż': return 34;
				default: throw new System.ArgumentException("Char " + C + " out of bounds");
			}
		}
	}
}
