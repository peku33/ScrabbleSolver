using System;
using System.Collections.Generic;

namespace ScrabbleSolver.Model.Items
{
	/// <summary>
	/// Klasa reprezentująca zestaw kostek dostępnych w grze
	/// </summary>
	public static class TilesSet
	{
		//Kolekcja przechowująca wszystkie kostki, które nie zostały jeszcze wylosowane
		private static readonly List<Tile> Tiles = new List<Tile>();

		//Instancja klasy random używana do losowania kostek z zestawu
		private static Random rand = new Random();

		public static bool IsEmpty()
		{
			return Tiles.Count == 0;
		}

		/// <summary>
		/// Losowanie kostki z zestawu. Kostka jest usuwana z zestawu po wylosowaniu.
		/// </summary>
		/// <returns>Losowo wybrana kostka</returns>
		public static Tile GetRandomTile()
		{
			if(IsEmpty())
			{
				return null;
			}

			Tile TempTile = Tiles[rand.Next(Tiles.Count)];
			Tiles.Remove(TempTile);

			return TempTile;
		}

		public static void Add(Tile NewTile)
		{
			Tiles.Add(NewTile);
		}

		/// <summary>
		/// Inicjalizacja zestawu kostek
		/// </summary>
		public static void Init()
		{
			Tiles.Clear();
			int i;

			for(i = 0; i < Configuration.ANumber; ++i) Tiles.Add(new Tile('a'));
			for(i = 0; i < Configuration._ANumber; ++i) Tiles.Add(new Tile('ą'));
			for(i = 0; i < Configuration.BNumber; ++i) Tiles.Add(new Tile('b'));
			for(i = 0; i < Configuration.CNumber; ++i) Tiles.Add(new Tile('c'));
			for(i = 0; i < Configuration._CNumber; ++i) Tiles.Add(new Tile('ć'));
			for(i = 0; i < Configuration.DNumber; ++i) Tiles.Add(new Tile('d'));
			for(i = 0; i < Configuration.ENumber; ++i) Tiles.Add(new Tile('e'));
			for(i = 0; i < Configuration._ENumber; ++i) Tiles.Add(new Tile('ę'));
			for(i = 0; i < Configuration.FNumber; ++i) Tiles.Add(new Tile('f'));
			for(i = 0; i < Configuration.GNumber; ++i) Tiles.Add(new Tile('g'));
			for(i = 0; i < Configuration.HNumber; ++i) Tiles.Add(new Tile('h'));
			for(i = 0; i < Configuration.INumber; ++i) Tiles.Add(new Tile('i'));
			for(i = 0; i < Configuration.JNumber; ++i) Tiles.Add(new Tile('j'));
			for(i = 0; i < Configuration.KNumber; ++i) Tiles.Add(new Tile('k'));
			for(i = 0; i < Configuration.LNumber; ++i) Tiles.Add(new Tile('l'));
			for(i = 0; i < Configuration._LNumber; ++i) Tiles.Add(new Tile('ł'));
			for(i = 0; i < Configuration.MNumber; ++i) Tiles.Add(new Tile('m'));
			for(i = 0; i < Configuration.NNumber; ++i) Tiles.Add(new Tile('n'));
			for(i = 0; i < Configuration._NNumber; ++i) Tiles.Add(new Tile('ń'));
			for(i = 0; i < Configuration.ONumber; ++i) Tiles.Add(new Tile('o'));
			for(i = 0; i < Configuration._ONumber; ++i) Tiles.Add(new Tile('ó'));
			for(i = 0; i < Configuration.PNumber; ++i) Tiles.Add(new Tile('p'));
			for(i = 0; i < Configuration.RNumber; ++i) Tiles.Add(new Tile('r'));
			for(i = 0; i < Configuration.SNumber; ++i) Tiles.Add(new Tile('s'));
			for(i = 0; i < Configuration._SNumber; ++i) Tiles.Add(new Tile('ś'));
			for(i = 0; i < Configuration.TNumber; ++i) Tiles.Add(new Tile('t'));
			for(i = 0; i < Configuration.UNumber; ++i) Tiles.Add(new Tile('u'));
			for(i = 0; i < Configuration.WNumber; ++i) Tiles.Add(new Tile('w'));
			for(i = 0; i < Configuration.YNumber; ++i) Tiles.Add(new Tile('y'));
			for(i = 0; i < Configuration.ZNumber; ++i) Tiles.Add(new Tile('z'));
			for(i = 0; i < Configuration._ZNumber; ++i) Tiles.Add(new Tile('ż'));
			for(i = 0; i < Configuration.__ZNumber; ++i) Tiles.Add(new Tile('ź'));
			for(i = 0; i < Configuration.BlankNumber; ++i) Tiles.Add(new Tile(' ', true));
		}
	}
}
