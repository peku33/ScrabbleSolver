using System;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Model.Player
{
    /// <summary>
    /// Klasa reprezentująca gracza
    /// </summary>
    abstract class Player
    {
        // Ilość punktów zgromadzonych przez gracza
        private int PointsNumber;

        // Nazwa gracza
        protected String Name;

        // Tabliczka z kostkami
        private readonly Rack Rack;

        protected Player()
        {
            Rack = new Rack();
            PointsNumber = 0;
        }

        // Wykonanie ruchu
        abstract public void MakeMove();
    }
}
