﻿using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Board
{
	/// <summary>
	/// Klasa reprezentująca pole gry 
	/// </summary>
	public class Cell
	{
		//Współrzędne pola gry.
		private readonly Coordinates CellCoordinates;

		//Premia słowna
		private int WordMultiplier;

		//Premia literowa
		private int LetterMultiplier;

		//Kostka znajdujaca sie na polu
		private Tile Tile;

		//Czy na polu znajduje sie juz jakas litera (jesli tak, to premie nie działają)
		private bool Visited;

		public Cell(Coordinates coordinates, int WordMultiplier, int LetterMultiplier)
		{
			this.CellCoordinates = coordinates;
			this.WordMultiplier = WordMultiplier;
			this.LetterMultiplier = LetterMultiplier;
			this.Tile = null;
			this.Visited = false;
		}

		public Cell(Coordinates CellCoordinates, int WordMultiplier, int LetterMultiplier, Tile Tile, bool Visited)
		{
			this.CellCoordinates = CellCoordinates;
			this.WordMultiplier = WordMultiplier;
			this.LetterMultiplier = LetterMultiplier;
			this.Tile = Tile;
			this.Visited = Visited;
		}

		public Cell(Cell ToCopyCell)
		{
			this.CellCoordinates = new Coordinates(ToCopyCell.GetXColumnCoordinate(), ToCopyCell.GetYRowCoordinate());
			this.WordMultiplier = ToCopyCell.GetWordMultiplier();
			this.LetterMultiplier = ToCopyCell.GetLetterMultiplier();
			if(ToCopyCell.GetTile() != null)
			{
				this.Tile = new Tile(ToCopyCell.GetTile());
			}
			else
			{
				this.Tile = null;
			}
			this.Visited = ToCopyCell.IsVisited();
		}

		public Tile GetTile()
		{
			return this.Tile;
		}

		public int GetXColumnCoordinate()
		{
			return CellCoordinates.GetXCoordinate();
		}

		public int GetYRowCoordinate()
		{
			return CellCoordinates.GetYCoordinate();
		}

		public bool IsVisited()
		{
			return this.Visited;
		}

		/// <summary>
		/// Sprawdzenie czy na podanym polu jest ustawiony blank
		/// </summary>
		/// <returns></returns>
		public bool IsBlank()
		{
			if(IsVisited())
			{
				return Tile.IsBlank();
			}
			return false;
		}

		public void SetVisited(bool Visited)
		{
			this.Visited = Visited;
		}

		public void SetTile(Tile NewTile)
		{
			this.Tile = NewTile;
		}

		public int GetLetterMultiplier()
		{
			return this.LetterMultiplier;
		}

		public int GetWordMultiplier()
		{
			return this.WordMultiplier;
		}

		public void SetLetterMultiplier(int Multiplier)
		{
			this.LetterMultiplier = Multiplier;
		}

		public void SetWordMultiplier(int Multiplier)
		{
			this.WordMultiplier = Multiplier;
		}
	}
}
