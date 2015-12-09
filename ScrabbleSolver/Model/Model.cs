using System;
using System.Collections.Generic;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model
{
    /// <summary>
    /// Klasa modelu gry.
    /// </summary>
    class Model
    {
        private readonly List<Player.Player> Players;
        private readonly Board.Board GameBoard;

        public Model(String BoardPath)
        {
            Players = new List<Player.Player>();
            GameBoard = new Board.Board(BoardPath);
        }
    }
}
