using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca pole gry
    /// 
    /// </summary>
    public class Cell
    {
        //Współrzędne pola gry.
        private readonly Coordinates CellCoordinates;

        private readonly int WordMultiplier;

        private readonly int LetterMultiplier;

        public Cell(Coordinates coordinates, int WordMultiplier, int LetterMultiplier)
        {
            this.CellCoordinates = coordinates;
            this.WordMultiplier = WordMultiplier;
            this.LetterMultiplier = LetterMultiplier;
        }
    }
}
