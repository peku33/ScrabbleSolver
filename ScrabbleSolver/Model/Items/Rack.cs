using System;
using System.Collections.Generic;

namespace ScrabbleSolver.Model.Items
{
    /// <summary>
    /// Klasa reprezentująca tabliczkę z kostkami.
    /// </summary>
    public class Rack
    {
        //Pojemność tabliczki
        private static readonly int MaxLetterNumber = 7;

        //Kostki znajdujące się w tabliczce.
        private List<Tile> Tiles;

        public Rack()
        {
            Tiles = new List<Tile>();
        }
    }
}
