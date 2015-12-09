using System;

namespace ScrabbleSolver.Model.Items
{
    /// <summary>
    /// Klasa reprezentująca kostkę - instancję litery, z której układane będą słowa w grze
    /// 
    /// </summary>
    public class Tile
    {
        //Znak znajdujacy sie na kostce
        private readonly char Letter;
        //Wartość kostki
        private readonly int Value;

        public Tile(char Letter)
        {
            this.Letter = Letter;

            switch (Letter)
            {
                case '0' : //blank
                    Value = 0;
                    break;
                case 'a':
                    Value = 1;
                    break;
                case 'i':
                    Value = 1;
                    break;
                case 'o':
                    Value = 1;
                    break;
                case 'e':
                    Value = 1;
                    break;
                case 'n':
                    Value = 1;
                    break;
                case 'y':
                    Value = 1;
                    break;
                case 'z':
                    Value = 1;
                    break;
                case 'w':
                    Value = 2;
                    break;
                case 'r':
                    Value = 2;
                    break;
                case 'c':
                    Value = 2;
                    break;
                case 'm':
                    Value = 2;
                    break;
                case 's':
                    Value = 2;
                    break;
                case 'k':
                    Value = 2;
                    break;
                case 'p':
                    Value = 2;
                    break;
                case 't':
                    Value = 2;
                    break;
                case 'u':
                    Value = 3;
                    break;
                case 'l':
                    Value = 3;
                    break;
                case 'd':
                    Value = 3;
                    break;
                case 'ł':
                    Value = 3;
                    break;
                case 'j':
                    Value = 3;
                    break;
                case 'b':
                    Value = 3;
                    break;
                case 'g':
                    Value = 3;
                    break;
                case 'ą':
                    Value = 3;
                    break;
                case 'h':
                    Value = 3;
                    break;
                case 'ś':
                    Value = 3;
                    break;
                case 'ż':
                    Value = 4;
                    break;
                case 'ę':
                    Value = 4;
                    break;
                case 'f':
                    Value = 5;
                    break;
                case 'ó':
                    Value = 6;
                    break;
                case 'ń':
                    Value = 7;
                    break;
                case 'ć':
                    Value = 8;
                    break;
                case 'ź':
                    Value = 9;
                    break;
                default: throw new System.ArgumentException("Char " + Letter + " not known");
            }
        }
    }

}
