using System;
using System.Collections.Generic;
using ScrabbleSolver.Model.Items;
using ScrabbleSolver.Model.Player;

namespace ScrabbleSolver.Model
{
    /// <summary>
    /// Klasa modelu gry.
    /// </summary>
    public class Model
    {
        private readonly Queue<Player.Player> Players;
        private readonly Board.Board GameBoard;
        private readonly Dictionary.Dictionary GameDictionary;

        public Model(Dictionary.Dictionary GameDictionary)
        {
            this.Players = new Queue<Player.Player>();
            this.GameBoard = new Board.Board(Configuration.BoardFile);
            this.GameDictionary = GameDictionary;

            TilesSet.Init();
        }

        public void InitPlayers(int HumanPlayers, int CPUPlayers) //TODO przerobic funkcje tak, aby dalo sie inicjalizowac graczy z wybranymi nickami
        {
            if(HumanPlayers + CPUPlayers > Configuration.MaxPlayersNumber)
            {
                throw new SystemException("too much players!");
            }
            for(int i = 0; i < HumanPlayers; ++i)
            {
                Players.Enqueue(new HumanPlayer("Gracz" + i, GameBoard, GameDictionary)); 
            }
            for(int i = 0; i < CPUPlayers; ++i)
            {
                Players.Enqueue(new AIPlayer(GameBoard, GameDictionary));
            }

            foreach(Player.Player Player in Players)
            {
                Player.GetNewTiles();
            }
        }

        public Dictionary.Dictionary GetDictionary()
        {
            return this.GameDictionary;
        }

        public void NextTurn()
        {
            Player.Player P = Players.Dequeue();
            P.MakeMove();
            Players.Enqueue(P);

            TestDisplay(); //TYLKO DO TESTOW
        }

        public bool isEnd()
        {
            foreach (Player.Player Player in Players)
            {
                if(Player.HasFinished())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Konsolowe wysiwetlanie planszy do testow
        /// </summary>
        public void TestDisplay()
        {
            GameBoard.ConsoleDisplay();

            foreach (Player.Player TempPlayer in Players)
            {
                System.Console.WriteLine(TempPlayer.GetName() + ": " + TempPlayer.GetPointsNumber() + " Rack: " + TempPlayer.GetLettersString());
            }

            System.Console.WriteLine();
        }
    }
}
