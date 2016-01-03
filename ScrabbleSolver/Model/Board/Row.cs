using System;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca wiersz planszy gry
	/// </summary>
	public class Row : Container
	{
		//Numer wiersza
		private readonly int YCoordinate;

		public Row(int y) : base()
		{
			this.YCoordinate = y;
		}

		public int GetYCoordinate()
		{
			return this.YCoordinate;
		}
	}
}
