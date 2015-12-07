using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca kolumnę planszy gry
    /// 
    /// </summary>
    public class Column : Container
    {
        private readonly int yCoordinate;

        protected Column(int y) :base()
        {
            this.yCoordinate = y;
        }

        public int GetYCoordinate()
        {
            return this.yCoordinate;
        }
    }
}
