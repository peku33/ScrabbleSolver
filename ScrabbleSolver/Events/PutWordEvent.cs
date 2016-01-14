using System;
using System.Collections.Generic;
using ScrabbleSolver.Board;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Events
{
	public class PutWordEvent : ApplicationEvent
	{
		// Lista reprezentujaca wszystkie pola planszy
		private readonly List<Cell> _BoardCells;
		// Lita reprezentujaca kostki ktore pozostaly w tabliczce gracza
		private readonly List<Tile> PlayerTiles; 

		public PutWordEvent(List<Cell> Cells, List<Tile> Tiles)
		{
			_BoardCells = Cells;
			PlayerTiles = Tiles;
		}

		public List<Cell> GetBoardCells()
		{
			return _BoardCells;
		}

		public List<Tile> GetPlayerTiles()
		{
			return PlayerTiles;
		} 
	}
}
