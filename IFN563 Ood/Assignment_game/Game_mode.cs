using System;
using static System.Console;
using System.IO;

namespace Assignment_game
{
    class Game_mode : Game
    {
        public string player;
        public string other_player;
        public string player_name;
        public string other_player_name;

        Match_state ms_obj = new Match_state();
        Ui_config ui_obj = new Ui_config();

        public string[,] bd;

        public void user_command(string userEntry)
        {
            bd = default_board();
            switch (userEntry)
            {
                case "N": // New game
                    {
                        ui_obj.displayCategory(2);
                        break;
                    }

                case "O": // initialize game
                    {
                        Write("\n Enter player name:");
                        player_name = ReadLine().ToUpper();
                        player = "B";
                        other_player = "W";
                        other_player_name = "Computer";

                        break;
                    }
                case "T":
                    {
                        Write("\n Enter player1 name:");
                        player_name = ReadLine().ToUpper();
                        player = "B";
                        Write("\n Enter player2 name:");
                        other_player = "W";
                        other_player_name = ReadLine().ToUpper();

                        break;
                    }
                case "S": // save game
                    {
                        Write("\n Enter file name to save game");
                        string tempfile = ReadLine() + ".txt";
                        ms_obj.save_game(tempfile);
                        break;
                    }

                case "L": // load game
                    {
                        ms_obj.load_save_list();
                        int userload = Convert.ToInt32(Console.ReadLine());
                        string[] filepath = ms_obj.load_save_list(false, userload);
                        ms_obj.load_game_from_list(filepath[0]);
                        break;
                    }

                case "R": // change move: redo and undo
                case "U": // change move: redo and undo
                    {
                        break;
                    }

                default:
                    {
                        player_name = "Human";
                        player = "B";
                        other_player = "W";
                        other_player_name = "Computer";
                        break;
                    }
            }
        }
    }
}
