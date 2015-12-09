using System;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca kolumnę planszy gry
    /// 
    /// </summary>
    public class Column : Container
    {
        //Numer kolumny
        private readonly int yCoordinate;

        public Column(int y) :base()
        {
            this.yCoordinate = y;
        }

        public int GetYCoordinate()
        {
            return this.yCoordinate;
        }
    }
}
