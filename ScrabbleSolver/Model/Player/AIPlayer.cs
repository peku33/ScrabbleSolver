using System;

namespace ScrabbleSolver.Model.Player
{
    /// <summary>
    /// Klasa reprezentująca gracza komputerowego.
    /// </summary>
    class AIPlayer : Player
    {

        public AIPlayer() : base()
        {
            this.Name = "CPU";
        }

        public override void MakeMove()
        {
            
        } 
    }
}
