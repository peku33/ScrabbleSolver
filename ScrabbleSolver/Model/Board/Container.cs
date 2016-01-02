using System.Collections.Generic;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca rząd (!= wiersz) pól.
	/// 
	/// </summary>
	public abstract class Container : System.Collections.Generic.List<Cell>
	{
		public Cell Get(int index)
		{
			if(this.Count > index && index >= 0)
			{
				return this[index];
			}

			return null;
		}
	}
}
