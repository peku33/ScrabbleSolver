using System;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca ludzkiego gracza.
	/// </summary>
	class HumanPlayer : Player
	{

		public HumanPlayer(String PlayerName, Model Model) : base(Model)
		{
			this.Name = PlayerName;
		}

		public override void MakeMove()
		{

		}

		public override void MakeFirstMove()
		{

		}

		public override void ReplaceTile()
		{

		}

		public override void Pass()
		{
			GameModel.PlayerPassed();
		}
	}
}
