using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrabbleSolver.Board;
using ScrabbleSolver.Common;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Events
{
	class UpdateViewEvent : ApplicationEvent
	{
		// Dictionary that contains current game state.
		private readonly Dictionary<GameInfoType, String> _GameInfo;

		// List that represents held characters.
		private readonly List<Tile> _HeldCharacters;

		// List that represents board cells.
		private readonly List<Cell> _BoardCells;

		public UpdateViewEvent(Dictionary<GameInfoType, string> GameInfo, List<Tile> HeldCharacters, List<Cell> BoardCells)
		{
			this._GameInfo = GameInfo;
			this._HeldCharacters = HeldCharacters;
			this._BoardCells = BoardCells;
		}

		public List<Cell> BoardCells
		{
			get { return _BoardCells; }
		}

		public Dictionary<GameInfoType, string> GameInfo
		{
			get { return _GameInfo; }
		}

		public List<Tile> HeldCharacters
		{
			get { return _HeldCharacters; }
		}
	}
}
