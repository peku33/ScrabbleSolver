using System;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca współrzędne na planszy gry
	/// </summary>
	public class Coordinates
	{
		private int XColumnCoordinate;
		private int YRowCoordinate;

		public Coordinates()
		{
		}

		public Coordinates(int xColumn, int yRow)
		{
			this.XColumnCoordinate = xColumn;
			this.YRowCoordinate = yRow;
		}

		public int GetXCoordinate()
		{
			return this.XColumnCoordinate;
		}

		public int GetYCoordinate()
		{
			return this.YRowCoordinate;
		}

		public override int GetHashCode()
		{
			return XColumnCoordinate ^ YRowCoordinate;
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
			return (XColumnCoordinate == p.XColumnCoordinate) && (YRowCoordinate == p.YRowCoordinate);
		}
	}
}
