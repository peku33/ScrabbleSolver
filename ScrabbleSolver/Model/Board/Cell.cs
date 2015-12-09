using System;

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

        //Premia słowna
        private readonly int WordMultiplier;

        //Premia literowa
        private readonly int LetterMultiplier;

        //Czy na polu znajduje sie juz jakas litera (jesli tak, to premie nie działają)
        private bool visited;

        public Cell(Coordinates coordinates, int WordMultiplier, int LetterMultiplier)
        {
            this.CellCoordinates = coordinates;
            this.WordMultiplier = WordMultiplier;
            this.LetterMultiplier = LetterMultiplier;
            this.visited = false;
        }
    }
}
