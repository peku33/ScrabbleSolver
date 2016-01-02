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
		//Czy blank
		private readonly bool Blank;

		public Tile(char Letter)
		{
			this.Letter = Letter;
			this.Value = Configuration.GetLetterValue(Letter);
			if(this.Letter.Equals(' '))
			{
				this.Blank = true;
			}
			else
			{
				this.Blank = false;
			}
		}

		public Tile(char Letter, bool Blank)
		{
			this.Letter = Letter;
			this.Value = Blank ? 0 : Configuration.GetLetterValue(Letter);
			this.Blank = Blank;
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
			return Blank;
		}
	}

}
