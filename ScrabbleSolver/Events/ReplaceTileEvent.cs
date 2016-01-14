using System;
using System.Collections.Generic;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Events
{
	public class ReplaceTileEvent : ApplicationEvent
	{
		// Lita reprezentujaca kostki ktore pozostaly w tabliczce gracza
		private readonly List<Tile> PlayerTiles;
		// Kostka ktora ma zostac wymieniona
		private readonly Tile ReplacedTile;

		public ReplaceTileEvent(List<Tile> Tiles, Tile ReplacedTile)
		{
			PlayerTiles = Tiles;
			this.ReplacedTile = ReplacedTile;
		} 

		public List<Tile> GetPlayerTiles()
		{
			return PlayerTiles;
		}

		public Tile GetReplacedTile()
		{
			return ReplacedTile;
		}
	}
}
