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

		public TilesSet(Language.Language Language)
		{
			foreach(char C in Language.GetLetters())
			{
				for(int i = 0; i < Language.GetLetterNumber(C); ++i)
				{
					Add(new Tile(C));
				}
			}
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
