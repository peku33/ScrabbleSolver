using System;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca współrzędne na planszy gry
	/// </summary>
	public class Coordinates
	{
		private int XCoordinate;
		private int YCoordinate;

		public Coordinates(int x, int y)
		{
			this.XCoordinate = x;
			this.YCoordinate = y;
		}

		public int GetXCoordinate()
		{
			return this.XCoordinate;
		}

		public int GetYCoordinate()
		{
			return this.YCoordinate;
		}
	}
}
