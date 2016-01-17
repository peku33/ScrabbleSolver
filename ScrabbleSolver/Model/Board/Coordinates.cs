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

		public override bool Equals(object o)
		{
			// If parameter is null return false:
			if ((object)o == null)
			{
				return false;
			}

			Coordinates p = (Coordinates) o;
			// Return true if the fields match:
			return (XCoordinate == p.XCoordinate) && (YCoordinate == p.YCoordinate);
		}
	}
}
