using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ScrabbleSolver.Board
{
    /// <summary>
    /// Klasa reprezentująca planszę gry
    /// </summary>
    public class Board
    {
        //Liczba kostek mieszczących się w jednym rzędzie planszy
        private int BoardSide;

        //Ścieżka do pliku mapy
        private readonly String BoardPath;

        //Lista wierszy planszy
        private readonly List<Row> Rows;

        //Lista kolumn planszy
        private readonly List<Column> Columns;

        private bool Empty;

        public Board(String BoardPath)
        {
            this.BoardPath = BoardPath;
            Rows = new List<Row>();
            Columns = new List<Column>();
            Empty = true;

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

            for(int i = 0; i < BoardSide; ++i)
            {
                Rows.Add(new Row(i));
                Columns.Add(new Column(i));
            }
            
            while(!BoardFileReader.EndOfStream)
            {
                if(xCoordinate > BoardSide)
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

                Rows[yCoordinate].Add(tempCell);
                Columns[xCoordinate].Add(tempCell);

                if(++xCoordinate == BoardSide)
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

            if(Values.Length != 1)
            {
                throw new SystemException("Board file format error: Board size not found");
            }

            BoardSide = Int32.Parse(Values[0]);
        }

        /// <summary>
        /// Wyswietlanie aktualnego stanu planszy na konsoli na potrzeby testowania
        /// </summary>
        public void ConsoleDisplay()
        {
            System.Console.Write("  ");

            for(int i = 0; i < BoardSide; ++i)
            {
                if(i > 9)
                    System.Console.Write(i);
                else
                    System.Console.Write(i + " ");
            }

            System.Console.WriteLine();

            foreach(Row TempRow in Rows)
            {
                if(TempRow.GetYCoordinate() > 9)
                {
                    System.Console.Write(TempRow.GetYCoordinate());
                }
                else
                {
                    System.Console.Write(TempRow.GetYCoordinate() + " ");
                }

                foreach(Cell TempCell in TempRow)
                {
                    if(TempCell.IsVisited())
                    {
                        System.Console.Write(TempCell.GetTile().GetLetter());
                        System.Console.Write(" ");
                    }
                    else
                    {
                        /*if(TempCell.GetLetterMultiplier() != 1)
                        {
                            System.Console.Write(TempCell.GetLetterMultiplier() + "l");
                        }
                        else if(TempCell.GetWordMultiplier() != 1)
                        {
                            System.Console.Write(TempCell.GetWordMultiplier() + "x");
                        }
                        else
                        {*/
                            System.Console.Write("*");
                            System.Console.Write(" ");
                        //}
                    }
                }

                System.Console.Write("\n");
            }
        }

        public bool IsEmpty()
        {
            return Empty;
        }

        public void SetEmpty(bool Empty)
        {
            this.Empty = Empty;
        }

        public int GetBoardSide()
        {
            return this.BoardSide;
        }

        public List<Row> GetRows()
        {
            return this.Rows;
        }

        public List<Column> GetColumns()
        {
            return this.Columns;
        }

        public Row FindRow(Cell Cell)
        {
            return Rows.FirstOrDefault(TempRow => TempRow.Contains(Cell));
        }

        public Row FindRow(int Index)
        {
            return Rows.FirstOrDefault(TempRow => TempRow.GetYCoordinate() == Index);
        }

        public Column FindColumn(Cell Cell)
        {
            return Columns.FirstOrDefault(TempColumn => TempColumn.Contains(Cell));
        }

        public Column FindColumn(int Index)
        {
            return Columns.FirstOrDefault(TempColumn => TempColumn.GetXCoordinate() == Index);
        }


    }
}
