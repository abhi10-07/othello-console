using System;
using static System.Console;
using System.Collections.Generic;

namespace Assignment_game
{
    class Game
    {
        const int BOARD_SIZE = 8;
        string[,] board = new string[BOARD_SIZE, BOARD_SIZE];
        protected List<string> history = new List<string>();

        Ui_config ui_obj = new Ui_config();

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
            WriteLine("White(W) " + score[0] + " - " + score[1] +  " Black(B)");

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

        public bool is_legal_move(string[,] b, int r, int c, string player)
        {
            return b[r, c] == " ";
        }

        public string[,] prompt_move(string[,] b, string player, string name)
        {
            WriteLine(name + "'s (" + player + ") turn");
            List<string> possibilities = new List<string>();
            List<string> flip = new List<string>();
            string menu_step;
            possibilities = get_possible_moves(b, player);

            if (possibilities.Count == 0)
                return b; // game over

            WriteLine("Possible valid moves:");
            foreach (var item in possibilities)
            {
                Write(" (" + item + ")");
            }

            WriteLine("\nAt any point enter 99 for the MENU");

            bool check = true;
            int xmove = -1, ymove = 1;
            do
            {               
                Write("\nEnter a x coordinate(column " + char.ConvertFromUtf32(0x2192) + "):");
                xmove = Convert.ToInt32(ReadLine());

                if(xmove == 99)
                {
                    do
                    {
                        ui_obj.displayCategory(3);
                        menu_step = ReadLine().ToUpper();

                    } while (!ui_obj.check_user_selection(menu_step));
                    
                }

                Write("Enter a y coordinate(row) " + char.ConvertFromUtf32(0x2193) + "):");
                ymove = Convert.ToInt32(ReadLine());

                if (ymove == 99)
                {
                    ui_obj.displayCategory(3);
                }

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

            flip = get_pieces_to_flip(b, xmove, ymove, player);
            board[xmove, ymove] = player;
            store_match_history(xmove, ymove);

            board = flip_pieces(board, flip, player);

            return board;
        }

        public List<string> get_possible_moves(string[,] b, string player)
        {
            List<string> moves = new List<string>();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (!is_legal_move(b, j, i, player))
                        continue;
                    else
                    {
                        List<string> temp = get_pieces_to_flip(b, i, j, player);
                        if (temp.Count > 0)
                            moves.Add(i + "," + j);
                    }
                }
            }

            return moves;
        }

        public List<string> get_pieces_to_flip(string[,] b, int i, int j, string player)
        {
            List<string> flip = new List<string>();

            flip.AddRange(get_included_pieces(b, i, j, 1, 1, player));
            flip.AddRange(get_included_pieces(b, i, j, 1, -1, player));
            flip.AddRange(get_included_pieces(b, i, j, -1, 1, player));
            flip.AddRange(get_included_pieces(b, i, j, 0, 1, player));
            flip.AddRange(get_included_pieces(b, i, j, 0, -1, player));
            flip.AddRange(get_included_pieces(b, i, j, 1, 0, player));
            flip.AddRange(get_included_pieces(b, i, j, -1, 0, player));
            flip.AddRange(get_included_pieces(b, i, j, -1, -1, player));

            List<string> uniqueList = new List<string>();
            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();

            foreach (string currValue in flip)
            {
                if (!uniqueStore.ContainsKey(currValue))
                {
                    uniqueStore.Add(currValue, 0);
                    uniqueList.Add(currValue);
                }
            }

            return uniqueList;
        }

        public List<string> get_included_pieces(string[,] b, int xstart, int ystart, int xdir, int ydir, string player)
        {
            List<string> included = new List<string>();
            string other_player = "";

            if (player == "B")
                other_player = "W";
            else
                other_player = "B";

            // distance in 7 spaces
            int xcurrent, ycurrent;
            for (int k = 1; k < BOARD_SIZE; k++)
            {
                xcurrent = xstart + k * xdir;
                ycurrent = ystart + k * ydir;

                if (xcurrent < 0 || xcurrent >= BOARD_SIZE || ycurrent < 0 || ycurrent >= BOARD_SIZE)
                {
                    included.Clear();
                    return included;
                }


                if (b[xcurrent, ycurrent] == other_player)
                    included.Add(xcurrent + "," + ycurrent);
                else if (b[xcurrent, ycurrent] == player)
                    return included;
                else
                {
                    included.Clear();
                    return included;
                }
            }

            included.Clear();
            return included;
        }

        public string[,] flip_pieces(string[,] b, List<string> flip, string player)
        {
            foreach (var item in flip)
            {
                string[] coordinates = new string[2];
                coordinates = item.Split(',');
                if (coordinates.Length == 2)
                {
                    //WriteLine("flip co " + Convert.ToInt32(coordinates[0]) + " | " + Convert.ToInt32(coordinates[1]));
                    b[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])] = player;
                }
            }

            return b;
        }

        // store user moves in Array
        public void store_match_history(int xmove, int ymove)
        {
            string temp = xmove + "," + ymove;
            history.Add(temp);
        }

    }
}
