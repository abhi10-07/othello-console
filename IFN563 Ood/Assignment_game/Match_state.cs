﻿using System;
using static System.Console;
using System.IO;

namespace Assignment_game
{
    /* class to store move, save and load match*/
    class Match_state
    {
        string SAVEPATH = System.IO.Path.Combine(@"C:\Users\Abhi\OneDrive - Queensland University of Technology\IFN563 Ood", "saved_games");
        string TEMPSAVEFILE = "current_game.txt";

        string[] files;
        const char DELIM = ',';
        
        //Method to save game
        public void save_game(string filename, char overwrite = 'N')
        {
            string tempfilepath = System.IO.Path.Combine(SAVEPATH, TEMPSAVEFILE);

            string filepath = System.IO.Path.Combine(SAVEPATH, filename);
 
            if (!System.IO.File.Exists(filepath) || overwrite == 'Y')
            {
                File.Copy(tempfilepath, filepath, true);
                WriteLine("\n File {0} saved.", filename);
            }
            else
            {
                if(overwrite == 'N')
                {
                    WriteLine("\nFile \"{0}\" already exists.", filename);
                    WriteLine("\n Overwrite file? Y/N ");
                    overwrite = Convert.ToChar(ReadLine());
                    if (overwrite == 'Y') 
                    {
                        File.Copy(tempfilepath, filepath, true);
                        WriteLine("\n File {0} saved.", filename);
                    }                        
                }

                return;
            }
        }

        //Method to display save game list
        public string[] load_save_list(bool display = true, int userLoad = 0)
        {
            string[] result = new string[10];
            if (Directory.Exists(SAVEPATH))
            {
                files = Directory.GetFiles(SAVEPATH);
                if (files.Length == 0)
                    Write("\nNo files");
                else
                {
                    if (display)
                        Write("\nSaved games: ");
                    for (int x = 0; x < files.Length; x++)
                    {
                        if (display)
                        {
                            result[x] = files[x];
                            Write("\n" + (x + 1) + ". " + files[x]);
                        }

                        else
                        {
                            if (x == (userLoad - 1))
                                result[x] = files[x];
                        }
                    }
                    if (display)
                        Write("\nSelect digit/s to load game: ");
                }
            }
            else
                Write("\nNo saved games");

            return result;

        }

        public void load_game_from_list(string filePath)
        {
            FileStream inFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] moves;
            recordIn = reader.ReadLine();
            while (recordIn != null)
            {
                moves = recordIn.Split(DELIM);
                for (int x = 0; x < moves.Length; x++)
                {
                    Write("\n" + moves[x]);
                }
                recordIn = reader.ReadLine();
            }

        }

        public void change_moves(string filePath, string type, int move_count = 0)
        {
            FileStream inFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] moves;
            
            recordIn = reader.ReadLine();

            while (recordIn != null)
            {
                moves = recordIn.Split(DELIM);
                int iteration = moves.Length;
                if (type == "Redo")
                    iteration = iteration + move_count;
                else
                    iteration = iteration - move_count;

                for (int x = 0; x < moves.Length; x++)
                {
                    Write("\n" + moves[x]);
                }
                recordIn = reader.ReadLine();
            }

        }

    }  // class Match_state
}
