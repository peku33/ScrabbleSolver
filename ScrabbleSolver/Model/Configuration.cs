using System;

namespace ScrabbleSolver.Model
{
	/// <summary>
	/// Klasa przechowujaca podstawowa konfiguracje aplikacji.
	/// </summary>
	public static class Configuration
	{
		//Ścieżki do plików inicjalizacyjnych
		public static String BoardFile = @"..\..\Board.txt";
		public static String DictionaryFile = @"..\..\Slowa.txt";

		//Pojemność tabliczki
		public static readonly int MaxLetterNumber = 7;

		//Maksymalna liczba graczy
		public static readonly int MaxPlayersNumber = 4;

		//Bonus punktowy za wykorzystanie wszystkich kostek z pelnej tabliczki
		public static readonly int SpecialBonus = 50;
	}
}