using System.Collections.Generic;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca rząd (!= wiersz) pól.
    /// 
    /// </summary>
    public abstract class Container
    {
        //Lista przechowujaca pola gry.
        private readonly List<Cell> Cells;

        public Container()
        {
            this.Cells = new List<Cell>();
        }

        public Cell Get(int index)
        {
            if (this.Cells.Count > index)
            {
                return this.Cells[index];
            }

            return null;
        }

        public void Add(Cell value)
        {
            this.Cells.Add(value);
        }

    }
}
