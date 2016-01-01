using System;

namespace ScrabbleSolver.Model.Player
{
    /// <summary>
    /// Klasa reprezentująca ludzkiego gracza.
    /// </summary>
    class HumanPlayer : Player
    {

        public HumanPlayer(String PlayerName, Board.Board GameBoard, Dictionary.Dictionary GameDictionary) : base(GameBoard, GameDictionary)
        {
            this.Name = PlayerName;
        }

        public override void MakeMove()
        {
            
        }

        public override void ReplaceTile()
        {
            
        }
    }
}
