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
        private readonly int xCoordinate;

        protected Row(int x) : base()
        {
            this.xCoordinate = x;
        }

        public int GetYCoordinate()
        {
            return this.xCoordinate;
        }
    }
}
