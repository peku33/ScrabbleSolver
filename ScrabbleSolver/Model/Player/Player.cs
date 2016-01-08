using System;
using ScrabbleSolver.Board;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
	/// <summary>
	/// Klasa reprezentująca gracza
	/// </summary>
	public abstract class Player
	{
		// Ilość punktów zgromadzonych przez gracza
		protected int PointsNumber;

		// Nazwa gracza
		protected String Name;

		// Tabliczka z kostkami
		protected readonly Rack Rack;

		protected readonly Model GameModel;

		protected Player(Model GameModel)
		{
			Rack = new Rack();
			PointsNumber = 0;
			this.GameModel = GameModel;
		}

		/// <summary>
		/// Wykonanie ruchu
		/// </summary>
		public abstract void MakeMove();

		/// <summary>
		/// Wykonanie pierwszego ruchu
		/// </summary>
		public abstract void MakeFirstMove();

		/// <summary>
		/// Wymiana kostki
		/// </summary>
		public abstract void ReplaceTile();

		/// <summary>
		/// Spasowanie ruchu
		/// </summary>
		public abstract void Pass();

		public int GetPointsNumber()
		{
			return this.PointsNumber;
		}

		public String GetName()
		{
			return this.Name;
		}

		public bool HasFinished()
		{
			return Rack.Count == 0;
		}

		/// <summary>
		/// Losowanie nowych kostek
		/// </summary>
		public void GetNewTiles()
		{
			TilesSet TilesSet = GameModel.GetTilesSet();

			while (!TilesSet.IsEmpty() && !Rack.IsFull())
			{
				Rack.Add(TilesSet.GetRandomTile());
			}
		}

		public int GetLettersNumber()
		{
			return Rack.Count;
		}

		/// <summary>
		/// TYLKO DO TESTOW - zwraca String z literami znajdujacymi sie w tabliczce
		/// </summary>
		/// <returns></returns>
		public String GetLettersString()
		{
			return Rack.GetTileString();
		}

	}
}
