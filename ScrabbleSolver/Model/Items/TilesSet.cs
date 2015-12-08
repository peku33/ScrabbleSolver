using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrabbleSolver.Model.Items
{
    /// <summary>
    /// Klasa reprezentująca zestaw kostek dostępnych w grze
    /// 
    /// </summary>
    class TilesSet
    {
        //Kolekcja przechowująca wszystkie kostki, które nie zostały jeszcze wylosowane
        private static readonly List<Tile> Tiles = new List<Tile>();

        //Instancja klasy random używana do losowania kostek z zestawu
        static Random rand = new Random();

        protected TilesSet()
        {

        }

        public bool IsEmpty()
        {
            return Tiles.Count == 0;
        }

        /// <summary>
        /// Losowanie kostki z zestawu 
        ///
        /// </summary>
        /// <returns>Losowo wybrana kostka</returns>
        public Tile GetTile()
        {
            return Tiles[rand.Next(Tiles.Count)];
        }


    }
}
