using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Description:
 * Is the blueprint for our catagory object. Contains data is the name,
 * the weight, a list of grades, percentage grade, and letter grade. Allows
 * new grade to be inputed directly or from user input. 
 */

namespace SchoolHelper
{
    class Catagory
    {
        private string name;
        private int weight;
        private List<Grade> grades = new List<Grade>();
        private double percGrade; // Grade total for the catagory
        private char letterGrade; // Letter grade for the catagory
        private int expAssignments; // Number of assignments to be expected, -1 is unknown
        private double percComplete;

        // Default Constructor
        public Catagory()
        {
            name = "No Name";
            weight = -1;
            expAssignments = -1;
        }


        // Property functions
        public string Name
        {
            get => name;
            set => name = value;
        }
        public int Weight
        {
            get => weight;
            set => weight = value;
        }
        public List<Grade> Grades
        {
            get => grades;
            set => grades = value;
        }
        public int ExpAssignments
        {
            get => expAssignments;
            set => expAssignments = value;
        }
        public Grade GetGrade(int gradeIndex)
        {
            return grades[gradeIndex];
        }
        public int GetNumGrades()
        {
            return grades.Count();
        }
        public double GetPercGrade()
        {
            CalcPercGrade();

            return percGrade;
        }
        public double GetPercGradeToCourse()
        {
            CalcPercGrade();
            return (percGrade * ((double)weight / 100));
        }
        public double GetPercComplete()
        {
            if (expAssignments == -1)
            {
                return -1;
            }
            else
            {
                return (((double)grades.Count() / expAssignments) * 100);
            }
        }
        public char LetterGrade
        {
            get => letterGrade;
            set => letterGrade = value;
        }
        // Adds a new grade into the grade list
        public void CreateGrade(Grade newGrade)
        {
            grades.Add(newGrade);
        }


        // Calculates the overall grade for the catagory
        private void CalcPercGrade()
        {
            int totalPointsEarned = 0;
            int totalPointsTotal = 0;

            // Calculates the total points earned and points total
            foreach (Grade grade in grades)
            {
                totalPointsEarned += grade.PointsEarned;
                totalPointsTotal += grade.PointsTotal;
            }

            // Calculates the percentage
            percGrade = Convert.ToDouble(totalPointsEarned) / totalPointsTotal;
            percGrade *= 100;
        }



        public int InputExpAssignments()
        {
            // Makes a border
            ConsoleColor borderColor = Program.currColor;
            Draw.Border("Input Expected Assignments Screen:", 4, borderColor);

            // Prints the current info
            Console.Write("Current Info: ");
            if (expAssignments != -1)
            {
                Console.Write(expAssignments);
            }
            else
            {
                Console.Write("UNKNOWN");
            }

            // Asks and scans input
            Console.SetCursorPosition(2, 8);
            Console.Write("Enter new value: ");
            expAssignments = Convert.ToInt16(Console.ReadLine());

            Console.SetCursorPosition(2, 10);
            Console.Write("Updated!");

            Console.SetCursorPosition(2, Program.WindowHeight - 1);
            Program.PrintPressAnyKeyToContinue();

            return expAssignments;
        }


        // Adds a grade from user input
        public Grade AddNewGrade()
        {
            Grade newGrade = new Grade();

            // Ask and scan the information for our new grade object
            Draw.Border("Add a New Grade Screen", 4, Program.currColor);

            Console.Write("Enter the name: ");
            newGrade.Name = Console.ReadLine().ToUpper();

            Console.SetCursorPosition(2, 8);
            Console.Write("Enter your points earned: ");
            newGrade.PointsEarned = Convert.ToInt16(Console.ReadLine());

            Console.SetCursorPosition(2, 10);
            Console.Write("Enter your points total: ");
            newGrade.PointsTotal = Convert.ToInt16(Console.ReadLine());

            // Adds the course to the vector
            CreateGrade(newGrade);

            return newGrade;
        }


        public bool DeleteGrade(int gradeIndex)
        {
            // Checks if the user wants to delete the course
            Draw.Rectangle(0, 0, Program.WindowWidth, Program.WindowHeight, Program.currColor);
            Console.SetCursorPosition(2, 1);
            Console.WriteLine("DELETED GRADES CAN NOT BE BROUGHT BACK:");
            Console.SetCursorPosition(2, 3);
            Console.Write("Are you sure (y/n)? ");
            char userInput = Program.getYesOrNo();

            // Deletes the course
            if (userInput == 'y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // Creates logic and choice vectors
        public List<string> CreateChoices()
        {
            List<string> choices = new List<string>();

            foreach(Grade grade in grades)
            {
                choices.Add(grade.Name);
            }

            // Adds the exit choice
            choices.Add("Exit");

            return choices;
        }
        // For this function, we will actually be return grade index instead of screens
        public List<int> CreateLogic()
        {
            List<int> logic = new List<int>();

            // Adds logic of the index in the course (so 0, 1, 2, 3, 4, etc)
            for (int i = 0; i < grades.Count(); ++i)
            {
                logic.Add(i);
            }

            // Adds the exit index
            logic.Add(-1);

            return logic;
        }
    }
}
