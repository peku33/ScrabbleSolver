using System;
using System.Collections.Generic;

namespace ScrabbleSolver.Model.Items
{
	/// <summary>
	/// Klasa reprezentująca zestaw kostek dostępnych w grze
	/// </summary>
	public class TilesSet : System.Collections.Generic.List<Tile>
	{
		//Instancja klasy random używana do losowania kostek z zestawu
		private Random rand = new Random();

		public TilesSet()
		{
			int i;
			for(i = 0; i < Configuration.ANumber; ++i) Add(new Tile('a'));
			for(i = 0; i < Configuration._ANumber; ++i) Add(new Tile('ą'));
			for(i = 0; i < Configuration.BNumber; ++i) Add(new Tile('b'));
			for(i = 0; i < Configuration.CNumber; ++i) Add(new Tile('c'));
			for(i = 0; i < Configuration._CNumber; ++i) Add(new Tile('ć'));
			for(i = 0; i < Configuration.DNumber; ++i) Add(new Tile('d'));
			for(i = 0; i < Configuration.ENumber; ++i) Add(new Tile('e'));
			for(i = 0; i < Configuration._ENumber; ++i) Add(new Tile('ę'));
			for(i = 0; i < Configuration.FNumber; ++i) Add(new Tile('f'));
			for(i = 0; i < Configuration.GNumber; ++i) Add(new Tile('g'));
			for(i = 0; i < Configuration.HNumber; ++i) Add(new Tile('h'));
			for(i = 0; i < Configuration.INumber; ++i) Add(new Tile('i'));
			for(i = 0; i < Configuration.JNumber; ++i) Add(new Tile('j'));
			for(i = 0; i < Configuration.KNumber; ++i) Add(new Tile('k'));
			for(i = 0; i < Configuration.LNumber; ++i) Add(new Tile('l'));
			for(i = 0; i < Configuration._LNumber; ++i) Add(new Tile('ł'));
			for(i = 0; i < Configuration.MNumber; ++i) Add(new Tile('m'));
			for(i = 0; i < Configuration.NNumber; ++i) Add(new Tile('n'));
			for(i = 0; i < Configuration._NNumber; ++i) Add(new Tile('ń'));
			for(i = 0; i < Configuration.ONumber; ++i) Add(new Tile('o'));
			for(i = 0; i < Configuration._ONumber; ++i) Add(new Tile('ó'));
			for(i = 0; i < Configuration.PNumber; ++i) Add(new Tile('p'));
			for(i = 0; i < Configuration.RNumber; ++i) Add(new Tile('r'));
			for(i = 0; i < Configuration.SNumber; ++i) Add(new Tile('s'));
			for(i = 0; i < Configuration._SNumber; ++i) Add(new Tile('ś'));
			for(i = 0; i < Configuration.TNumber; ++i) Add(new Tile('t'));
			for(i = 0; i < Configuration.UNumber; ++i) Add(new Tile('u'));
			for(i = 0; i < Configuration.WNumber; ++i) Add(new Tile('w'));
			for(i = 0; i < Configuration.YNumber; ++i) Add(new Tile('y'));
			for(i = 0; i < Configuration.ZNumber; ++i) Add(new Tile('z'));
			for(i = 0; i < Configuration._ZNumber; ++i) Add(new Tile('ż'));
			for(i = 0; i < Configuration.__ZNumber; ++i) Add(new Tile('ź'));
			for(i = 0; i < Configuration.BlankNumber; ++i) Add(new Tile(' ', true));
		}

		public bool IsEmpty()
		{
			return Count == 0;
		}

		/// <summary>
		/// Losowanie kostki z zestawu. Kostka jest usuwana z zestawu po wylosowaniu.
		/// </summary>
		/// <returns>Losowo wybrana kostka</returns>
		public Tile GetRandomTile()
		{
			if(IsEmpty())
			{
				return null;
			}

			Tile TempTile = this[rand.Next(Count)];
			Remove(TempTile);
			return TempTile;
		}
	}
}
