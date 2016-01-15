using System;
using System.Collections.Generic;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Events
{
	public class ReplaceTileEvent : ApplicationEvent
	{
		// Zawartosc kostki ktora ma zostac wymieniona
		private readonly String ReplacedTileString;

		public ReplaceTileEvent(String ReplacedTileString)
		{
			this.ReplacedTileString = ReplacedTileString;
		} 

		public String GetReplacedTile()
		{
			return ReplacedTileString;
		}
	}
}
