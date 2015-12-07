using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca rząd (!= wiersz) pól.
    /// 
    /// </summary>
    public abstract class Container
    {
        //Lista przechowujaca pola gry.
        private readonly List<Field> Fields;

        protected Container()
        {
            this.Fields = new List<Field>();
        }

        public Field Get(int index)
        {
            if (this.Fields.Count > index)
            {
                return this.Fields[index];
            }

            return null;
        }

        public void Add(Field value)
        {
            this.Fields.Add(value);
        }

    }
}
