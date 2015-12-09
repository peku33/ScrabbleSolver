using System;
using System.Collections.Generic;
using System.IO;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca planszę gry
    /// </summary>
    public class Board
    {
        //Liczba kostek mieszczących się w jednym rzędzie planszy
        private int BoardSize;

        //Ścieżka do pliku mapy
        private readonly String BoardPath;

        //Lista wierszy planszy
        private readonly List<Row> rows;

        //Lista kolumn planszy
        private readonly List<Column> columns;

        public Board(String BoardPath)
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
            int xCoordinate = 0;
            int yCoordinate = 0;

            ReadBoardSize(BoardFileReader);

            for(int i = 0; i < BoardSize; ++i)
            {
                rows.Add(new Row(i));
                columns.Add(new Column(i));
            }


            while(!BoardFileReader.EndOfStream)
            {
                if(yCoordinate > BoardSize)
                {
                    throw new SystemException("Board file format exception: too long file");
                }

                Line = BoardFileReader.ReadLine();

                Values = Line.Split(' ');

                if(Values.Length < 2)
                {
                    throw new SystemException("Board file format error");
                }

                Cell tempCell = new Cell(new Coordinates(xCoordinate, yCoordinate),
                    Int32.Parse(Values[0]), Int32.Parse(Values[1]));

                rows[xCoordinate].Add(tempCell);
                columns[yCoordinate].Add(tempCell);

                if(++xCoordinate == BoardSize)
                {
                    xCoordinate = 0;
                    ++yCoordinate;
                }
            }
        }

        /// <summary>
        /// Wczytanie z pliku rozmiaru planszy (pierwsza linijka w pliku konfiguracyjnym)
        /// </summary>
        /// <param name="BoardFileReader"></param>
        private void ReadBoardSize(StreamReader BoardFileReader)
        {
            String Line = BoardFileReader.ReadLine();
            String[] Values = Line.Split(' ');

            if (Values.Length != 1)
            {
                throw new SystemException("Board file format error: Board size not found");
            }

            BoardSize = Int32.Parse(Values[0]);
        }
    }
}
