﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* The Menu class is a screen that gives the user a list of choices,
 * the user inputs a choice, and then it returns a index for what we need to do
 * next. This class also implements the Draw class by drawing a formatted boarder.
 * This color of the boarder is based of a variable inside the program class. * 
 */

namespace SchoolHelper
{
    class Menu
    {
        // Variables
        private string userInput; // Index 0 will be the user choice. The rest is to catch mistakes
        private string title;
        private List<string> choices;
        private List<int> logic = new List<int>();
        
        // Constructor, require a title
        public Menu(string title) { this.title = title; }
         
        // Setters for choices and logic and title
        public void SetChoices(List<string> choices) { this.choices = choices; }
        public void SetLogic(List<Screens> logic)
        {
            foreach(Screens screen in logic)
            {
                this.logic.Add((int)screen);
            }
        }
        public void SetLogic(List<int> logic) { this.logic = logic; }
        public string Title
        {
            set => title = value;
        }

        // Runs the screen
        public int RunScreen()
        {
            // Prints the screen
            PrintScreen();

            // Gets the user input
            getUserInput();

            //Returns logic
            return logic[(int)(userInput[0] - 'A')];
            
        }
               
        // Gets the user inputs and checks to see if its within that range, if not it reprints the screen and gets another choice
        private void getUserInput()
        {
            // Gets the user input
            Console.SetCursorPosition(3, Program.WindowHeight - 2);
            Console.Write("> ");
            userInput = Console.ReadLine().ToUpper();

            // Checks input
            while (userInput.Length != 1 ||  userInput[0] < 'A' || userInput[0] > (char)(logic.Count() - 1 + 'A'))
            {
                // Writes a new screen and display incorrect choices
                Console.Clear();
                PrintScreen();
                Console.SetCursorPosition(3, Program.WindowHeight - 2);
                Console.Write("Incorrect choice: > ");
                userInput = Console.ReadLine().ToUpper();
            }

        }


        // Prints the screen
        private void PrintScreen()
        {
            ConsoleColor borderColor = Program.currColor;

            Draw.Border(title, 4, borderColor);

            // Displays choices
            char currLetter = 'A';
            for (int i = 0; i < choices.Count(); ++i)
            {
                // Sets the cursor pos
                Console.SetCursorPosition(2, 6 + i);

                // Prints the msg
                Console.WriteLine("[" + currLetter + "] ~ " + choices[i]);
                currLetter++;
            }
        }
    }
}
