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
		private readonly bool IsBlank;

		private bool IsEmpty;
		public Tile(char Letter)
		{
			this.Letter = Letter;
			this.IsBlank = this.Letter.Equals(' ');
			this.IsEmpty = false;
		}

		public Tile(char Letter, bool isBlank)
		{
			this.Letter = Letter;
			this.IsBlank = isBlank;
			this.IsEmpty = false;
		}

		public Tile(bool IsEmpty)
		{
			this.IsEmpty = IsEmpty;
		}

		public char GetLetter()
		{
			return this.Letter;
		}

		public bool GetIsBlank()
		{
			return IsBlank;
		}

		public void SetIsEmpty(bool IsEmpty)
		{
			this.IsEmpty = IsEmpty;
		}

		public void SetLetter(char Letter)
		{
			this.Letter = Letter;
		}
	}

}
