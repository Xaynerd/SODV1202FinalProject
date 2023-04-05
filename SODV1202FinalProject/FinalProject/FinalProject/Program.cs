using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

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
        public void Move(int playerMove, Player player)
        {
            bool found = false;
            int filled = 0;
            string playerPiece = player.GetPlayerPiece(); //retrieves the player's piece
            for (int i = 5; i >= 0; i--)
            {
                if (found) {
                    break;
                }
                else
                {
                    if (BoardArray[i,playerMove] == " ") {
                        BoardArray[i, playerMove] = playerPiece; //replaces the bottom-most empty position with the player piece
                        found = true;
                    } else
                    {
                        filled++;
                        if (filled == 6)
                        {
                            Console.WriteLine("Sorry, this column is full already buddy. Here, I'll put it somewhere randomly for you!"); //i want to move this into main somehow so then it persists through console.clear, though if not ill just get rid of console.clear ;)
                            Random ran = new Random();
                            bool find = false;
                            while (find == false)
                            {
                                int rand = ran.Next(1, 8);
                                if (rand != playerMove) //you know it would be a lot simpler to just get them to chose again, but guh, i refuse!
                                {
                                    find = true;
                                    Move(rand, player); //makes a mystery move for the player :)
                                }
                            }
                            
                            
                        }
                    }
                }

            }
        }

    }


    public class Controls : Board
    {
        public int GetMove()
        {
            while (true) //https://stackoverflow.com/questions/28955029/how-do-i-convert-a-console-readkey-to-an-int-c-sharp
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int number;
                bool isNumber = int.TryParse(keyInfo.KeyChar.ToString(), out number);
                if (isNumber && number > 0 && number < 8) //i had it so it would check later, but it makes more sense just to do it here and loop until it gets a proper result
                {
                    return number;
                } 
                else
                {
                    Console.WriteLine("That is not a valid input, please enter a number between 1-7");
                }
            }
        }

    }

    public abstract class Player
    {
        public bool Turn { get; set; }
        public Player() //i havent fully wrapped my head aroudn classes and stuff so somethings are gonna be kinda off. like this, but it works so who cares
        {
            Turn = true;
        }
        public abstract string GetPlayerPiece();
        public abstract bool PlayerTurn();
        public void EndTurn()
        {
            Turn = false;
        }
        public void StartTurn()
        {
            Turn = true;
        }
    }
    public class Player1 : Player
    {
        public override string GetPlayerPiece()
        {
            return "x";
        }
        public override bool PlayerTurn()
        {
            return Turn;
        }
    }

    public class Player2 : Player
    {
        public override string GetPlayerPiece()
        {
            return "o";
        }
        public override bool PlayerTurn()
        {
            return Turn;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Board game = new Board();
            Controls control = new Controls();
            Player player1 = new Player1();
            Player player2 = new Player2();
            bool gameRunning = true;
            Console.WriteLine(player1.PlayerTurn());
            Console.WriteLine(player2.PlayerTurn());

            game.CreateBoard();
            game.Display();
            //game.BoardArray[5, 1] = "x";
            //game.Display();
            //game.Move(control.GetMove(), player1);
            //game.Display();

            while (gameRunning)
            {
                Console.Clear();
                game.Display();
                if (player1.PlayerTurn() == true)
                {
                    Console.WriteLine("PLAYER 1 TURN");
                    game.Move(control.GetMove(), player1);
                    player1.EndTurn();
                    player2.StartTurn();
                    //Console.WriteLine(player1.PlayerTurn());
                    //Console.WriteLine(player2.PlayerTurn());
                } else if (player2.PlayerTurn() == true)
                {
                    Console.WriteLine("PLAYER 2 TURN");
                    game.Move(control.GetMove(), player2);
                    player2.EndTurn();
                    player1.StartTurn();
                }               
            }
            
        }
    }

}