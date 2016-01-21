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
		private char Letter;
		//Czy blank
		private bool Blank;

		private bool Empty;

		public Tile(char Letter)
		{
			this.Letter = Letter;
			this.Blank = this.Letter.Equals(' ');
			this.Empty = false;
		}

		public Tile(char Letter, bool blank)
		{
			this.Letter = Letter;
			this.Blank = blank;
			this.Empty = false;
		}

		public Tile(Tile ToCopyTile)
		{
			Letter = ToCopyTile.GetLetter();
			Blank = ToCopyTile.IsBlank();
			Empty = ToCopyTile.IsEmpty();
		}

		public Tile(bool empty)
		{
			this.Empty = empty;
		}

		public char GetLetter()
		{
			return this.Letter;
		}

		public bool IsBlank()
		{
			return Blank;
		}

		public bool IsEmpty()
		{
			return this.Empty;
		}

		public void SetIsEmpty(bool IsEmpty)
		{
			this.Empty = IsEmpty;
		}

		public void SetLetter(char Letter)
		{
			this.Letter = Letter;
		}

		public void SetIsBlank(bool IsBlank)
		{
			this.Blank = IsBlank;
		}
	}

}
