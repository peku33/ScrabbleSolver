using System;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca współrzędne na planszy gry
    /// 
    /// </summary>
    public class Coordinates
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coordinates) obj);
        }

        //Współrzędna x
        private int XCoordinate;

        //Współrzędna y
        private int YCoordinate;

        public Coordinates(int x, int y)
        {
            this.XCoordinate = x;
            this.YCoordinate = y;
        }

        public static bool operator ==(Coordinates c1, Coordinates c2)
        {
            return (c1.GetXCoordinate() == c2.GetXCoordinate() && 
                c1.GetYCoordinate() == c2.GetYCoordinate());
        }

        public static bool operator !=(Coordinates c1, Coordinates c2)
        {
            return !(c1 == c2);
        }

        public bool Equals(Coordinates c)
        {
            return c == this;
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
            return GetYCoordinate() ^ GetXCoordinate();
        }
    }
}
