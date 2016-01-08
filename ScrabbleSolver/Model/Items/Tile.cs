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
		//Czy blank
		private readonly bool Blank;

		public Tile(char Letter)
		{
			this.Letter = Letter;

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
			this.Blank = Blank;
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
