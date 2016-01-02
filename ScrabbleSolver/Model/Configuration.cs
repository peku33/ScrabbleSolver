using System;

namespace ScrabbleSolver.Model
{
	/// <summary>
	/// Klasa przechowujaca podstawowa konfiguracje aplikacji.
	/// </summary>
	public static class Configuration
	{
		//Stałe określające liczbę kostek z daną literą dostępnych w grze
		public static readonly int ANumber = 9;
		public static readonly int INumber = 8;
		public static readonly int ONumber = 7;
		public static readonly int ENumber = 6;
		public static readonly int NNumber = 6;
		public static readonly int YNumber = 5;
		public static readonly int ZNumber = 5;
		public static readonly int WNumber = 5;
		public static readonly int RNumber = 4;
		public static readonly int CNumber = 4;
		public static readonly int MNumber = 3;
		public static readonly int SNumber = 3;
		public static readonly int KNumber = 3;
		public static readonly int PNumber = 3;
		public static readonly int TNumber = 2;
		public static readonly int UNumber = 2;
		public static readonly int LNumber = 2;
		public static readonly int DNumber = 2;
		public static readonly int _LNumber = 2; //ł
		public static readonly int JNumber = 2;
		public static readonly int BNumber = 2;
		public static readonly int GNumber = 2;
		public static readonly int _ANumber = 1; //ą
		public static readonly int HNumber = 1;
		public static readonly int _SNumber = 1; //ś
		public static readonly int _ZNumber = 1; //ż
		public static readonly int _ENumber = 1; //ę
		public static readonly int FNumber = 1;
		public static readonly int _ONumber = 1; //ó
		public static readonly int _NNumber = 1; //ń
		public static readonly int _CNumber = 1; //ć
		public static readonly int __ZNumber = 1;//ź
		public static readonly int BlankNumber = 2; //Blank

		//Ścieżki do plików inicjalizacyjnych
		public static String BoardFile = @"..\..\Board.txt";
		public static String DictionaryFile = @"..\..\Slowa.txt";

		//Pojemność tabliczki
		public static readonly int MaxLetterNumber = 7;

		//Maksymalna liczba graczy
		public static readonly int MaxPlayersNumber = 4;

		//Bonus punktowy za wykorzystanie wszystkich kostek z pelnej tabliczki
		public static readonly int SpecialBonus = 50;

		/// <summary>
		/// Funkcja zwracająca wartość litery
		/// </summary>
		/// <param name="Letter"></param>
		/// <returns></returns>
		public static int GetLetterValue(char Letter)
		{
			switch(Letter)
			{
				case ' ': return 0;
				case 'a': return 1;
				case 'i': return 1;
				case 'o': return 1;
				case 'e': return 1;
				case 'n': return 1;
				case 'y': return 1;
				case 'z': return 1;
				case 'w': return 2;
				case 'r': return 2;
				case 'c': return 2;
				case 'm': return 2;
				case 's': return 2;
				case 'k': return 2;
				case 'p': return 2;
				case 't': return 2;
				case 'u': return 3;
				case 'l': return 3;
				case 'd': return 3;
				case 'ł': return 3;
				case 'j': return 3;
				case 'b': return 3;
				case 'g': return 3;
				case 'ą': return 3;
				case 'h': return 3;
				case 'ś': return 3;
				case 'ż': return 4;
				case 'ę': return 4;
				case 'f': return 5;
				case 'ó': return 6;
				case 'ń': return 7;
				case 'ć': return 8;
				case 'ź': return 9;
				default:
					throw new ArgumentException("Char " + Letter + " not known");
			}
		}
	}
}