using System;
using System.Collections.Generic;

namespace ScrabbleSolver.Model.Items
{
    /// <summary>
    /// Klasa reprezentująca zestaw kostek dostępnych w grze
    /// 
    /// </summary>
    public static class TilesSet
    {
        //Stałe określające liczbę kostek z daną literą dostępnych w grze
        private static readonly int ANumber = 9;
        private static readonly int INumber = 8;
        private static readonly int ONumber = 7;
        private static readonly int ENumber = 6;
        private static readonly int NNumber = 6;
        private static readonly int YNumber = 5;
        private static readonly int ZNumber = 5;
        private static readonly int WNumber = 5;
        private static readonly int RNumber = 4;
        private static readonly int CNumber = 4;
        private static readonly int MNumber = 3;
        private static readonly int SNumber = 3;
        private static readonly int KNumber = 3;
        private static readonly int PNumber = 3;
        private static readonly int TNumber = 2;
        private static readonly int UNumber = 2;
        private static readonly int LNumber = 2;
        private static readonly int DNumber = 2;
        private static readonly int _LNumber = 2; //ł
        private static readonly int JNumber = 2;
        private static readonly int BNumber = 2;
        private static readonly int GNumber = 2;
        private static readonly int _ANumber = 1; //ą
        private static readonly int HNumber = 1;
        private static readonly int _SNumber = 1; //ś
        private static readonly int _ZNumber = 1; //ż
        private static readonly int _ENumber = 1; //ę
        private static readonly int FNumber = 1;
        private static readonly int _ONumber = 1; //ó
        private static readonly int _NNumber = 1; //ń
        private static readonly int _CNumber = 1; //ć
        private static readonly int __ZNumber = 1;//ź
        private static readonly int BlankNumber = 2; //Blank

        //Kolekcja przechowująca wszystkie kostki, które nie zostały jeszcze wylosowane
        private static readonly List<Tile> Tiles = new List<Tile>();

        //Instancja klasy random używana do losowania kostek z zestawu
        private static Random rand = new Random();

        public static bool IsEmpty()
        {
            return Tiles.Count == 0;
        }

        /// <summary>
        /// Losowanie kostki z zestawu 
        ///
        /// </summary>
        /// <returns>Losowo wybrana kostka</returns>
        public static Tile GetTile()
        {
            return Tiles[rand.Next(Tiles.Count)];
        }

        /// <summary>
        /// Inicjalizacja zestawu kostek
        /// </summary>
        public static void Init()
        {
            int i;

            for (i = 0; i < ANumber; ++i) Tiles.Add(new Tile('a'));
            for (i = 0; i < _ANumber; ++i) Tiles.Add(new Tile('ą'));
            for (i = 0; i < BNumber; ++i) Tiles.Add(new Tile('b'));
            for (i = 0; i < CNumber; ++i) Tiles.Add(new Tile('c'));
            for (i = 0; i < _CNumber; ++i) Tiles.Add(new Tile('ć'));
            for (i = 0; i < DNumber; ++i) Tiles.Add(new Tile('d'));
            for (i = 0; i < ENumber; ++i) Tiles.Add(new Tile('e'));
            for (i = 0; i < _ENumber; ++i) Tiles.Add(new Tile('ę'));
            for (i = 0; i < FNumber; ++i) Tiles.Add(new Tile('f'));
            for (i = 0; i < GNumber; ++i) Tiles.Add(new Tile('g'));
            for (i = 0; i < HNumber; ++i) Tiles.Add(new Tile('h'));
            for (i = 0; i < INumber; ++i) Tiles.Add(new Tile('i'));
            for (i = 0; i < JNumber; ++i) Tiles.Add(new Tile('j'));
            for (i = 0; i < KNumber; ++i) Tiles.Add(new Tile('k'));
            for (i = 0; i < LNumber; ++i) Tiles.Add(new Tile('l'));
            for (i = 0; i < _LNumber; ++i) Tiles.Add(new Tile('ł'));
            for (i = 0; i < MNumber; ++i) Tiles.Add(new Tile('m'));
            for (i = 0; i < NNumber; ++i) Tiles.Add(new Tile('n'));
            for (i = 0; i < _NNumber; ++i) Tiles.Add(new Tile('ń'));
            for (i = 0; i < ONumber; ++i) Tiles.Add(new Tile('o'));
            for (i = 0; i < _ONumber; ++i) Tiles.Add(new Tile('ó'));
            for (i = 0; i < PNumber; ++i) Tiles.Add(new Tile('p'));
            for (i = 0; i < RNumber; ++i) Tiles.Add(new Tile('r'));
            for (i = 0; i < SNumber; ++i) Tiles.Add(new Tile('s'));
            for (i = 0; i < _SNumber; ++i) Tiles.Add(new Tile('ś'));
            for (i = 0; i < TNumber; ++i) Tiles.Add(new Tile('t'));
            for (i = 0; i < UNumber; ++i) Tiles.Add(new Tile('u'));
            for (i = 0; i < WNumber; ++i) Tiles.Add(new Tile('w'));
            for (i = 0; i < YNumber; ++i) Tiles.Add(new Tile('y'));
            for (i = 0; i < ZNumber; ++i) Tiles.Add(new Tile('z'));
            for (i = 0; i < _ZNumber; ++i) Tiles.Add(new Tile('ż'));
            for (i = 0; i < __ZNumber; ++i) Tiles.Add(new Tile('ź'));
            for (i = 0; i < BlankNumber; ++i) Tiles.Add(new Tile(' '));
        }
    }
}
