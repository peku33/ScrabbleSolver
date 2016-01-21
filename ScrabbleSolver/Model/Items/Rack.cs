using System;
using System.Collections.Generic;

namespace ScrabbleSolver.Model.Items
{
	/// <summary>
	/// Klasa reprezentująca tabliczkę z kostkami.
	/// </summary>
	public class Rack : System.Collections.Generic.List<Tile>
	{
		//Instancja klasy random używana do losowania kostek z tabliczki
		private static Random Rand = new Random();

		public Rack(Rack toCopyRack)
		{
			foreach(Tile TempTile in toCopyRack)
			{
				Add(TempTile);
			}
		}

		public Rack() : base()
		{
		}

		public String GetTileString()
		{
			String Result = String.Empty;

			foreach(Tile TempTile in this)
			{
				Result += TempTile.GetLetter();
			}

			return Result;
		}

		public bool Remove(char Letter)
		{
			foreach(Tile TempTile in this)
			{
				if(TempTile.GetLetter().Equals(Letter))
				{
					return Remove(TempTile);
				}
			}
			foreach(Tile TempTile in this)
			{
				if(TempTile.IsBlank())
				{
					return Remove(TempTile);
				}
			}

			return false;
		}

		public bool IsFull()
		{
			return this.Count == Configuration.MaxLetterNumber;
		}

		/// <summary>
		/// Wyjecie losowej kostki z tabliczki.
		/// </summary>
		/// <returns></returns>
		public Tile GetRandomTile()
		{
			Tile TempTile = this[Rand.Next(Count)];
			Remove(TempTile);
			return TempTile;
		}

		public bool Contains(Char Letter)
		{
			foreach(Tile TempTile in this)
			{
				if(TempTile.GetLetter().Equals(Letter))
				{
					return true;
				}
			}
			return false;
		}
	}
}
