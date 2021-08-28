using System;
using static System.Console;
using System.Collections.Generic;

namespace Assignment_game
{
    class Game
    {
        const int BOARD_SIZE = 8;
        string[,] board = new string[BOARD_SIZE, BOARD_SIZE];

        // initial empty board
        public string[,] default_board(int len = BOARD_SIZE)
        {
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    board[i, j] = " ";
                }
            }

            board[3, 3] = "W";
            board[3, 4] = "B";
            board[4, 3] = "B";
            board[4, 4] = "W";

            return board;

        }

        public void draw_board(string[,] b)
        {
            WriteLine(" | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |");
            WriteLine("-------------------------------------");
            string row = "";

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                row = i + "| ";
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    row += b[j, i] + " | ";
                }

                WriteLine(row + i);
                WriteLine("-----------------------------------");
            }

            WriteLine(" | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |");
            WriteLine("-----------------------------------");

            int[] score = get_score(b);
            WriteLine("\nCurrent score:");
            WriteLine("White(W):" + score[0]);
            WriteLine("Black(B):" + score[1]);

        }

        public int[] get_score(string[,] b)
        {
            int[] piece = new int[2];
            piece[0] = 0; // 0 index for white
            piece[1] = 0; // 1 index for black

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (b[i, j] == "W")
                        ++piece[0];
                    else if (b[i, j] == "B")
                        ++piece[1];
                }
            }

            return piece;
        }

        public bool is_board_full(string[,] b)
        {
            bool full = true;
            WriteLine("calling after?");
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (b[i, j] == " ")
                        full = false;
                }
            }

            return full;
        }

        public string[,] prompt_move(string[,] b, string player, string player_name)
        {
            WriteLine(player_name + "'s (" + player + ") turn");
            List<string> possibilities = new List<string>();
            List<string> flip = new List<string>();
            possibilities = get_possible_moves(b, player);

            if (possibilities.Count == 0)
                return b; // game over

            bool check = true;
            int xmove = -1, ymove = 1;
            do
            {
                Write("Enter a x coordinate(column):");
                xmove = Convert.ToInt32(ReadLine());

                Write("Enter a y coordinate(row):");
                ymove = Convert.ToInt32(ReadLine());

                string move = xmove + "," + ymove;

                foreach (var item in possibilities)
                {
                    //WriteLine(item);
                    //if (possibilities.Equals(item.Trim()))
                    check = String.Equals(item, move);
                    if (check)
                        break;
                }

            } while (!check);

            WriteLine("Player move: " + xmove +" | "+ ymove + " | " + player);
            flip = piece_to_flip(b, xmove, ymove, player);
            board[xmove, ymove] = player;

            board = flip_pieces(b, flip, player);

            return b;

        }

        public List<String> get_possible_moves(string[,] b, string player)
        {
            //string[] moves = new string[b.Length*b.Length];
            List<string> moves = new List<string>();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (!is_legal_move(b, i, j, player))
                        continue;
                    else
                    {
                        List<string> flipLength = piece_to_flip(b, i, j, player);
                        if (flipLength.Count > 0)
                        {
                            moves.Add(i + "," + j);
                        } 
                    }
                }
            }

            return moves;
        }

        public bool is_legal_move(string[,] b, int x_co, int y_co, string player)
        {
            return b[x_co, y_co] == " ";
        }

        public List<string> get_included_pieces(string[,] b, int xstart, int ystart, int xdir, int ydir, string player)
        {
            List<string> included = new List<string>();
            string other_player;
            int xcurr, ycurr;

            if (player == "W")
                other_player = "B";
            else
                other_player = "W";

            for (int k = 1; k<BOARD_SIZE; k++)
            {
                xcurr = xstart + k * xdir;
                ycurr = ystart + k * ydir;

                if (xcurr < 0 || xcurr >= BOARD_SIZE || ycurr < 0 || ycurr >= BOARD_SIZE)
                    return included;

                if (String.Equals(b[ycurr, xcurr].Trim(), other_player.Trim()) && b[ycurr, xcurr] != " ")
                {
                    included.Add(xcurr + "," + ycurr);
                }   
                else if (b[ycurr, xcurr] == player)
                    return included;
                else
                    return included;
            }
            
            return included;
        }

        public List<String> piece_to_flip(string[,] b, int x, int y, string player)
        {
            List<string> flip = new List<string>();

            // check for (direction) all 8 pieces around the main. ;
            flip.AddRange(get_included_pieces(b, x, y, 1, 1, player));
            flip.AddRange(get_included_pieces(b, x, y, 1, -1, player));
            flip.AddRange(get_included_pieces(b, x, y, 1, -1, player));
            flip.AddRange(get_included_pieces(b, x, y, -1, 1, player));
            flip.AddRange(get_included_pieces(b, x, y, 0, 1, player));
            flip.AddRange(get_included_pieces(b, x, y, 0,-1, player));
            flip.AddRange(get_included_pieces(b, x, y, 1, 0, player));
            flip.AddRange(get_included_pieces(b, x, y, -1, 0, player));
            flip.AddRange(get_included_pieces(b, x, y, -1, -1, player));


            return flip;
        }

        public string[,] flip_pieces(string[,] b, List<string> flip, string player)
        {
            foreach (var item in flip)
            {
                string[] coordinates = new string[2];
                coordinates = item.Split(',');
                if(coordinates.Length == 2)
                {
                    WriteLine("flip co " + Convert.ToInt32(coordinates[0]) + " | " + Convert.ToInt32(coordinates[1]));
                    b[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])] = player;
                }
                
            }

            return b;
        }

        public string[,] comp_move(string[,] b, string player)
        {
            List<string> possibilities = new List<string>();
            possibilities = get_possible_moves(b, player);

            if (possibilities.Count == 0)
                return b; // game over

            string[] coordinates = new string[2];

            // get any random index from possiblities and make a move
            foreach (var item in possibilities)
            {
                coordinates = item.Split(',');
                if (coordinates.Length == 2)
                {
                    WriteLine("flip co " + Convert.ToInt32(coordinates[0]) + " | " + Convert.ToInt32(coordinates[1]));
                    b[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])] = player;
                }
            }


            

            return b;
        }




    }
}
