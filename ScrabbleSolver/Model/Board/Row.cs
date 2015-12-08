using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca wiersz planszy gry
    /// 
    /// </summary>
    public class Row : Container
    {
        //Numer wiersza
        private readonly int xCoordinate;

        public Row(int x) : base()
        {
            this.xCoordinate = x;
        }

        public int GetYCoordinate()
        {
            return this.xCoordinate;
        }
    }
}
