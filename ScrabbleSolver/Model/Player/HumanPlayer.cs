using System;

namespace ScrabbleSolver.Model.Player
{
    /// <summary>
    /// Klasa reprezentująca ludzkiego gracza.
    /// </summary>
    class HumanPlayer : Player
    {

        public HumanPlayer(String PlayerName) : base()
        {
            this.Name = PlayerName;
        }

        public override void MakeMove()
        {
            
        }
    }
}
