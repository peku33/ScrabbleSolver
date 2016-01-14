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

		public Coordinates()
		{
		}

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

		public override int GetHashCode()
		{
			return XCoordinate ^ YCoordinate;
		}

		public bool Equals(Coordinates p)
		{
			// If parameter is null return false:
			if ((object)p == null)
			{
				return false;
			}

			// Return true if the fields match:
			return (XCoordinate == p.XCoordinate) && (YCoordinate == p.YCoordinate);
		}
	}
}
