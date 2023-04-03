using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace MyProject
{
    public class Board {
        public int Columns = 7;
        public int Rows = 6;
        public string[,] BoardArray = new string[7,9];

        public void CreateBoard()
        {
            //fill array with spaces
            for (int i = 0; i < Rows + 1; i++)
            {
                for (int j = 0; j < Columns + 2; j++)
                {
                    BoardArray[i, j] = " ";
                }
            }
            //build the bottom of the board
            for (int i = 0; i < Columns + 2; i++)
            {
                BoardArray[6, i] = "—";
            }
            //build the sides of the board
            for (int i = 0; i < Rows; i++) 
            {
                BoardArray[i, 0] = "|";
                BoardArray[i, 8] = "|";
            }
            //build corners of board
            BoardArray[6, 0] = "└";
            BoardArray[6, 8] = "┘";
        }
        public void Display()
        {
            //print gameboard array
            Console.WriteLine();
            for (int i = 0; i < Rows + 1; i++)
            {
                for (int j = 0; j < Columns + 2; j++)
                {
                    Console.Write(BoardArray[i, j] + " ");
                }
                Console.WriteLine();

            }
        }

        public virtual bool PlayerTurn()
        {
            return false;
        }

    }
    public abstract class Player1 : Board
    {
        public bool Turn;
        public virtual bool PlayerTurn()
        {
            // implementation for Player1's turn
            Turn = true;
        }
    }

    public abstract class Player2 : Board
    {
        public bool Turn;
        public virtual bool PlayerTurn()
        {
            // implementation for Player2's turn
            Turn = true;
        }
    }

    public class Controls : Board
    {
        public override bool PlayerTurn()
        {
            // call the appropriate player's turn method based on whose turn it is
            if (Player1.Turn)
            {
                Player1.PlayerTurn();
            }
            else if (Player2.Turn)
            {
                Player2.PlayerTurn();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Board game = new Board();
            game.CreateBoard();
            game.Display();
            game.BoardArray[4, 4] = "x";
            game.Display();

            
        }
    }

}