using System;
using static System.Console;
using System.IO;

/**
 * @author: Abhi Sapkal
 * @project: Othello console game
 */

namespace Assignment_game
{
    class Program
    {
        static void Main(string[] args)
        {
            Ui_config ui_obj = new Ui_config();
            Game_mode gm_obj = new Game_mode();
            Game g_obj = new Game();
            string steps;

            // Step1: Ask user for new or load game
            ui_obj.displayCategory(1);
            steps = ReadLine().ToUpper();

            // Step2: Based on selection next step
            gm_obj.user_command(steps);
            steps = ReadLine().ToUpper();

            // Step3: Ask players details
            gm_obj.user_command(steps);

            string[,] board = g_obj.default_board();
            string player = "B";
            string other_player = "W";

            string player_name = "Abhi";
            string other_player_name = "Chandu";



            while (!g_obj.is_board_full(board))
            {
                g_obj.draw_board(board);

                if ((g_obj.get_possible_moves(board, player).Count == 0) && (g_obj.get_possible_moves(board, other_player).Count == 0))
                    break;

                string[,] temp = g_obj.prompt_move(board, player, player_name);
                if (temp.Length != 0)
                    board = temp;

                SwapValues(ref player, ref other_player);
                SwapValues(ref player_name, ref other_player_name);
            }

            int[] score = g_obj.get_score(board);

            if (score[1] > score[0])
                WriteLine("Black wins!"); 
            else if (score[0] > score[1])
                WriteLine("White wins!");
            else
                WriteLine("Tie?");

            // Allow user to enter name for players:
            // if one player: p1 = Human name; p2 = Computer
            // if two player: p1 = Human name; p2 = Human name

        }// End main

        static void SwapValues(ref string x, ref string y)
        {

            string tempswap = x;
            x = y;
            y = tempswap;
        }


    } // class program


    //https://csharp.hotexamples.com/examples/Othello/Board/-/php-board-class-examples.html
}
