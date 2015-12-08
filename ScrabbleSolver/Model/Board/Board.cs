using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca planszę gry
    /// </summary>
    public class Board
    {
        //Liczba kostek mieszczących się w jednym rzędzie planszy
        private static readonly int BoardSize = 15;
        
        //Ścieżka do pliku mapy
        private readonly String BoardPath;

        //Lista wierszy planszy
        private readonly List<Row> rows;

        //Lista kolumn planszy
        private readonly List<Column> columns;

        protected Board(String BoardPath)
        {
            this.BoardPath = BoardPath;
            rows = new List<Row>();
            columns = new List<Column>();

            Init(BoardPath);
        }

        /// <summary>
        /// Wczytanie mapy gry z pliku 
        /// </summary>
        /// <param name="BoardPath"> Ścieżka do pliku z mapą gry</param>
        private void Init(String BoardPath)
        {
            StreamReader BoardFileReader = new System.IO.StreamReader(BoardPath);
            String Line;
            String[] Values;
            Cell tempCell;
            int xCoordinate = 0;
            int yCoordinate = 0;

            for (int i = 0; i < BoardSize; ++i)
            {
                rows.Add(new Row(i));
                columns.Add(new Column(i));
            }

            while (!BoardFileReader.EndOfStream)
            {
                Line = BoardFileReader.ReadLine();

                Values = Line.Split(' ');

                if (Values.Length < 2)
                {
                    throw new SystemException("Board file format error");
                }

                tempCell = new Cell(new Coordinates(xCoordinate, yCoordinate), 
                    Int32.Parse(Values[0]), Int32.Parse(Values[1]));

                rows[xCoordinate].Add(tempCell);
                columns[yCoordinate].Add(tempCell);

                if(++xCoordinate == 15)
                {
                    xCoordinate = 0;
                    ++yCoordinate;
                }
            }
        }
    }
}
