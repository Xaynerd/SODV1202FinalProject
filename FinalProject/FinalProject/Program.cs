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
            bool found = false;                             //used to break out of loop when it finds the lowest most spot
            int filled = 0;                                 //used to check how full a column is
            string playerPiece = player.GetPlayerPiece();   //retrieves the player's piece
            for (int i = 5; i >= 0; i--)
            {
                if (found) {
                    break; //breaks out of loop
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
                            Random ran = new Random(); //thought it would be fun to add a random element :)
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
        public bool CheckWin(Player player)
        {
            //i was dreading this part
            string playerPiece = player.GetPlayerPiece();

            //check rows
            for (int row = 0; row < 6; row++)//will not search _ row
            {
                for (int col = 1; col < 5; col++) //wont bother searching the | columns as that would be a waste of computing power, even if it doesnt matter too much at this level
                {
                    if (BoardArray[row, col] == playerPiece &&
                        BoardArray[row, col + 1] == playerPiece &&
                        BoardArray[row, col + 2] == playerPiece &&
                        BoardArray[row, col + 3] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            //check columns
            for (int row = 0; row < 3; row++)
            {
                for (int col = 1; col < 8; col++)
                {
                    if (BoardArray[row, col] == playerPiece &&
                        BoardArray[row + 1, col] == playerPiece &&
                        BoardArray[row + 2, col] == playerPiece &&
                        BoardArray[row + 3, col] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            //check diagonal top left to bottom right
            for (int row = 0; row < 4; row++)
            {
                for (int col = 1; col < 5; col++)
                {
                    if (BoardArray[row, col] == playerPiece &&
                        BoardArray[row + 1, col + 1] == playerPiece &&
                        BoardArray[row + 2, col + 2] == playerPiece &&
                        BoardArray[row + 3, col + 3] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            //check diagonal top right to bottom left
            for (int row = 0; row < 4; row++)
            {
                for (int col = 3; col < 8; col++)
                {
                    if (BoardArray[row, col] == playerPiece &&
                        BoardArray[row + 1, col - 1] == playerPiece &&
                        BoardArray[row + 2, col - 2] == playerPiece &&
                        BoardArray[row + 3, col - 3] == playerPiece)
                    {
                        return true;
                    }
                }
            }

            //no win found
            return false;
        }


    }


    public class Controls : Board
    {
        public int GetMove()
        {   //loop until a number between 1-7 is given
            while (true) //https://stackoverflow.com/questions/28955029/how-do-i-convert-a-console-readkey-to-an-int-c-sharp
            {            //took a minute looking around for this one
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int number;
                bool isNumber = int.TryParse(keyInfo.KeyChar.ToString(), out number); //was having a problem with it crashing if you input something that wasnt a number
                if (isNumber && number > 0 && number < 8) 
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
        public bool PlayerTurn()
        {
            return Turn;
        }
        public void EndTurn()//could maybe get away with combining these somehow, but i just cant think of it so them being split up works just fine
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
    }

    public class Player2 : Player
    {
        public override string GetPlayerPiece()
        {
            return "o";
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
            int winner = 0;

            game.CreateBoard();
            game.Display();

            while (gameRunning)
            {
                //Console.Clear();  there is an error message in one of the class methods that i want seen, so this wont do, unless i add a delay somewhere it will never be seen :(      
                if (player1.PlayerTurn() == true)
                {
                    Console.WriteLine("PLAYER 1 TURN");
                    game.Move(control.GetMove(), player1);
                    player1.EndTurn();

                    if (game.CheckWin(player1) == true)
                    {
                        winner = 1;
                        gameRunning = false;
                    } 
                    else
                    {
                        player2.StartTurn(); //prevent player2's turn from starting after player 1 wins, though i forget if it actually does start player2's turn :/
                    }

                } 
                else if (player2.PlayerTurn() == true)
                {
                    Console.WriteLine("PLAYER 2 TURN");
                    game.Move(control.GetMove(), player2);
                    player2.EndTurn();
                    
                    if (game.CheckWin(player2) == true)
                    {
                        winner = 2;
                        gameRunning = false;
                    }
                    else
                    {
                        player1.StartTurn(); //prevent player1 turn from starting after victory
                    }
                }
                game.Display();
            }

            Console.WriteLine();
            Console.WriteLine(winner == 1 ? "Congrats on your victory over player 2! You win player 1!\n" : "Congrats on your victory over player 1! You win player 2!\n");
            Console.WriteLine("Until next time o/");
            
        }
    }

}