using System;

namespace ScrabbleSolver.Model.Items
{
	/// <summary>
	/// Klasa reprezentująca kostkę - instancję litery, z której układane będą słowa w grze
	/// 
	/// </summary>
	public class Tile
	{
		//Znak znajdujacy sie na kostce
		private readonly char Letter;
		//Wartość kostki
		private readonly int Value;

		public Tile(char Letter)
		{
			this.Letter = Letter;
			this.Value = Configuration.GetLetterValue(Letter);
		}

		public int GetValue()
		{
			return this.Value;
		}

		public char GetLetter()
		{
			return this.Letter;
		}

		public bool IsBlank()
		{
			return Letter.Equals(' ');
		}
	}

}
