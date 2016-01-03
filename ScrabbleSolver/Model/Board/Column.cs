using System;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca kolumnę planszy gry
	/// </summary>
	public class Column : Container
	{
		//Numer kolumny
		private readonly int XCoordinate;

		public Column(int x) : base()
		{
			this.XCoordinate = x;
		}

		public int GetXCoordinate()
		{
			return this.XCoordinate;
		}
	}
}
