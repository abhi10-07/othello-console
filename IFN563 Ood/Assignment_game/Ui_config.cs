using System;
using static System.Console;
namespace Assignment_game
{
    class Ui_config
    {
        public string[] codesArray = new string[8]
        {
            "N", "S", "L", "R", "U", "Q", "O", "T"
        };

        public string[] descriptionArray = new string[8]
        {
            "New game",
            "Save game",
            "Load game",
            "Redo",
            "Undo",
            "Quit game",
            "One player",
            "Two player"
        };

        public string[] step1 = new string[]
        {
            "N", "L"
        };

        public string[] step2 = new string[]
        {
            "O", "T"
        };
        
        public string[] step3 = new string[]
        {
            "S", "L", "R", "U", "Q"
        };

        public void displayCategory(int stepNumber, string displayType = "table")
        {
            string[] stepArray  = new string[5];
            if (stepNumber == 1)
                stepArray = step1;
            else if(stepNumber == 2)
                stepArray = step2; 
            else if(stepNumber == 3)
                stepArray = step3;


            WriteLine("\nSelect an option to play the game: ");
            for (int i = 0; i < codesArray.Length; i++)
            {
                if (Array.IndexOf(stepArray, codesArray[i]) >= 0) 
                {
                    if (displayType == "table")
                    {
                        WriteLine("-------------------------------------------");
                        WriteLine("{0,10}{1,10}{2,10}", codesArray[i], '|', descriptionArray[i]);
                    }
                    else if (displayType == "line")
                    {
                        Write(descriptionArray[i] + ": " + codesArray[i] );
                        Write(" | ");
                    }
                    
                }
                   
            }

        } // method displayCategory

        public bool check_user_selection(string step)
        {
            bool valid = false;
            if (Array.IndexOf(codesArray, step) >= 0)
                valid = true;

            return valid;
        }
    }
}
