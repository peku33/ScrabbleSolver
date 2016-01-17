using System;
using System.Collections.Generic;
using ScrabbleSolver.Board;
using ScrabbleSolver.Common;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Events
{
	public class UpdateViewEvent : ApplicationEvent
	{
		// Dictionary that contains current game state.
		private readonly Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, String>> _GameInfo;

		// List that represents held characters.
		private readonly Dictionary<PlayerIdEnum, List<Tile>> _HeldCharacters;

		// List that represents board cells.
		private readonly List<Cell> _BoardCells;

		public UpdateViewEvent(Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, String>> GameInfo, Dictionary<PlayerIdEnum, List<Tile>> HeldCharacters, List<Cell> BoardCells)
		{
			this._GameInfo = GameInfo;
			this._HeldCharacters = HeldCharacters;
			this._BoardCells = BoardCells;
		}

		public List<Cell> BoardCells
		{
			get { return _BoardCells; }
		}

		public Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, String>> GameInfo
		{
			get { return _GameInfo; }
		}

		public Dictionary<PlayerIdEnum, List<Tile>> HeldCharacters
		{
			get { return _HeldCharacters; }
		}
	}
}
