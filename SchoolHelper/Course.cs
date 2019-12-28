using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Description:
 * Is the blueprint for a course. Contains data such as the name,
 * credits, a list of catagories, a list of cut offs for each letter grade,
 * a percentage grade, and a letter grade.
 * 
 */

namespace SchoolHelper
{
    class Course
    {
        private string name;
        private int credits;
        private List<Catagory> catagories = new List<Catagory>();
        private List<int> percCutoff; // Percentage need to get that grade (0 = A, 1 = B, etc)
        private List<double> gradeHistory = new List<double>();
        private double percGrade; // Grade total for the course
        private char letterGrade;
        private int finalWeight; // Weight of the final
        private double percCompleted; // Total completetion of the course


        public Course()
        {
            name = "No Name";
            credits = -1;
            finalWeight = 0;
            percCompleted = 0;
        }
        public Course(string name, int credits, char letterGrade)
        {
            this.name = name;
            this.credits = credits;
            this.letterGrade = letterGrade;
        }


        // Property functions
        public string Name
        {
            get => name;
            set => name = value;
        }
        public int Credits
        {
            get => credits;
            set => credits = value;
        }
        public List<Catagory> Catagories
        {
            get => catagories;
            set => catagories = value;
        }
        public char LetterGrade
        {
            get => letterGrade;
            set => letterGrade = value;
        }
        public int FinalWeight
        {
            get => finalWeight;
            set => finalWeight = value;
        }
        public double GetPercGrade()
        {
            calcCourseGrade();
            return percGrade;
        }
        public Catagory GetCatagory(int catagoryIndex)
        {
            return catagories[catagoryIndex];
        }
        public int GetNumCatagories() { return catagories.Count(); }
        public List<int> PercCutoff 
        {
            get => percCutoff;
            set => percCutoff = value;
        }
        public void AddGradeHist(double newData)
        {
            gradeHistory.Add(newData);
        }
        public string[] GetPercCutoffAsString()
        {
            string[] cutoffs = new string[4];

            for (int i = 0; i < 4; ++i)
            {
                cutoffs[i] = Convert.ToString(percCutoff[i]);
            }

            return cutoffs;
        }
        public double GetPercComplete()
        {
            // Calculates the overal completion for the course
            int totalKnownEntires = 0; // Entries that we know have a certain amount of assignments
            int totalExpected = 0;
            foreach (Catagory catagory in catagories)
            {
                if (catagory.ExpAssignments != -1)
                {
                    totalKnownEntires += catagory.GetNumGrades();
                    totalExpected += catagory.ExpAssignments;
                }
            }
            percCompleted = 100 * (double)totalKnownEntires / totalExpected;

            return percCompleted;
        }

        // Finds the letter grade for the catagory
        public char FindLetterGrade(double percGrade)
        {
            char letterGrade;
            
            // Range of grade
            if (percGrade >= percCutoff[0])
            {
                letterGrade = 'A';
            }
            else if (percGrade >= percCutoff[1])
            {
                letterGrade = 'B';
            }
            else if (percGrade >= percCutoff[2])
            {
                letterGrade = 'C';
            }
            else if (percGrade >= percCutoff[3])
            {
                letterGrade = 'D';
            }
            else
            {
                letterGrade = 'F';
            }
            
            return letterGrade;
        }


        private double calcCourseGrade()
        {
            percGrade = 100; // Just if we have no grades stored in
            int totalWeight = 0; // Weights that have grades in them 
            double percEarned = 0; // Percentage amount that we have earned   

            foreach(Catagory catagory in catagories)
            {
                // So we are not counting catagories without grades
                if (catagory.GetNumGrades() != 0)
                {
                    totalWeight += catagory.Weight;
                    percEarned += catagory.GetPercGradeToCourse();
                }
            }

            // calcs percGrade
            percGrade = (Convert.ToDouble(percEarned) / totalWeight) * 100;
            
            return percGrade;
        }

        // Prints on the screen the basic course information
        public void ViewCourseInfo()
        {
            Draw.Border("Course information for " + name + ":", 4, Program.currColor);

            Console.SetCursorPosition(2, 6);
            Console.WriteLine("Name: " + name);

            Console.SetCursorPosition(2, 8);
            Console.WriteLine("Credits: " + credits);

            Console.SetCursorPosition(2, 10);
            int currHeight = 12;
            Console.WriteLine("Catagories and Weights:");
            foreach(Catagory catagory in catagories)
            {
                Console.SetCursorPosition(2, currHeight++);
                Console.WriteLine("   " + catagory.Name + " ~ " + catagory.Weight + "%");
            }

            // Check if there is a final
            if (finalWeight != 0)
            {
                Console.SetCursorPosition(2, currHeight++);
                Console.WriteLine("   FINAL ~ " + finalWeight + "%");
            }

            currHeight++;
            Console.SetCursorPosition(2, currHeight++);
            Console.WriteLine("Percentage Cuttoff Per Grade:");
            currHeight++;
            for (int i = 0; i < 4; ++i)
            {
                Console.SetCursorPosition(2, currHeight++);
                Console.WriteLine("   " + Convert.ToChar(i + 'A') + " ~ " + percCutoff[i] + "%");
            }

            Console.SetCursorPosition(2, Program.WindowHeight - 2);
            Program.PrintPressAnyKeyToContinue();
        }





        /*
         * Prints to the screen about grade information. Lets the user decide if 
         * they want to see all grades in every catagory
         */
        public void ViewGradeInfo()
        {
            ConsoleColor borderColor = Program.currColor;
            Draw.Border("Grade Information for " + name + ":", 8, borderColor);

            // Calc course grade and letter grade
            calcCourseGrade();
            letterGrade = FindLetterGrade(percGrade);

            // Writes the information
            Console.SetCursorPosition(2, 4);
            Console.WriteLine("Total Grade: {0:0.00}%", percGrade);
            Console.SetCursorPosition(2, 6);
            Console.WriteLine("Letter Grade: " + letterGrade);

            // Writes catagories main information
            Console.SetCursorPosition(2, 9);
            Console.WriteLine("Main Catagory Info:");

            // Prepares the data to be drawn into a table
            string[,] data = new string[catagories.Count() + 1, 7];

            int i;
            for (i = 0; i < catagories.Count(); ++i)
            {
                Catagory currCat = catagories[i];

                // Name
                data[i, 0] = currCat.Name;

                // Grade
                data[i, 1] = String.Format("{0:0.00}", currCat.GetPercGrade()) + "% ~ " + Convert.ToString(FindLetterGrade(currCat.GetPercGrade()));

                // Weight
                data[i, 2] = Convert.ToString(currCat.Weight) + "%";

                // % lost
                data[i, 3] = String.Format("{0:0.00}", currCat.Weight - currCat.GetPercGradeToCourse()) + "%";

                // # entries
                data[i, 4] = Convert.ToString(currCat.GetNumGrades());

                // # expected and % completed
                if (currCat.ExpAssignments != -1)
                {
                    data[i, 5] = Convert.ToString(currCat.ExpAssignments);
                    data[i, 6] = String.Format("{0:0.00}", currCat.GetPercComplete()) + "%";
                }      
                else
                {
                    data[i, 5] = "UNKNOWN";
                    data[i, 6] = "UNKNOWN";
                }

            }

            // Collects total data
            int totalWeight = 0;
            double totalLost = 0;
            int totalEntries = 0;
            int totalExpected = 0;
            foreach (Catagory catagory in catagories)
            {
                totalWeight += catagory.Weight;

                totalEntries += catagory.GetNumGrades();

                totalLost += catagory.Weight - catagory.GetPercGradeToCourse();

                if (catagory.ExpAssignments != -1)
                {
                    totalExpected += catagory.ExpAssignments;
                }
            }
            GetPercComplete();

            // Stores the remaning data
            data[i, 0] = "TOTAL";
            data[i, 1] = "";
            data[i, 2] = Convert.ToString(totalWeight) + "%";
            data[i, 3] = String.Format("{0:0.00}", totalLost) + "%";
            data[i, 4] = Convert.ToString(totalEntries);
            if (totalExpected != 0)
            {
                data[i, 5] = Convert.ToString(totalExpected);
                data[i, 6] = String.Format("{0:0.00}", percCompleted) + "%";
            }
            else
            {
                data[i, 5] = "UNKNOWN";
                data[i, 6] = "UNKNOWN";
            }

            // Writes the table
            Draw.TableWithData(9, 11, 15, new string[] {
                "NAME:", "GRADE:", "WEIGHT:", "% LOST:", "# ENTRIES:",
                "# EXPECTED:", "% COMPLETED:" }, Program.TableColor, data, true);
                           

            // Givse the user the choices to view through each catagory
            Console.SetCursorPosition(2, Program.WindowHeight - 1);
            Console.Write("Would you like to view each catagory (y/n)? ");
            char userInput = Program.getYesOrNo();

            // Display the courses
            if (userInput == 'y')
            {
                // Filps through all the catagories
                foreach (Catagory catagory in catagories)
                {
                    Console.Clear();

                    // Prints the boarder and title
                    Draw.Border("Grade Information for " + name, 7, borderColor);
                    Console.SetCursorPosition(2, 4);
                    Console.WriteLine("Catagory Grade: {0:0.00}% ~ " + FindLetterGrade(catagory.GetPercGrade()), catagory.GetPercGrade());
                    Console.SetCursorPosition(2, 6);
                    Console.WriteLine("Catagory: " + catagory.Name);

                    string[,] gradeData = new string[catagory.GetNumGrades(), 3];

                    for (i = 0; i < catagory.GetNumGrades(); ++i)
                    {
                        Grade currGrade = catagory.GetGrade(i);

                        gradeData[i, 0] = currGrade.Name;
                        gradeData[i, 1] = Convert.ToString(currGrade.PointsEarned) + "/" + Convert.ToString(currGrade.PointsTotal);
                        gradeData[i, 2] = String.Format("{0:0.00}", currGrade.GetPercGrade()) + "% ~ " + Convert.ToString(FindLetterGrade(currGrade.GetPercGrade()));
                    }

                    Draw.TableWithData(4, 9, 15, new string[] {
                        "NAME:", "POINTS EARNED/TOTAL:", "GRADE:" },
                        Program.TableColor, gradeData, ExtraSpace: false);

                    Console.SetCursorPosition(2, Program.WindowHeight - 1);
                    Program.PrintPressAnyKeyToContinue();
                }
            }

        }

        public void ViewGradeHist()
        {
            // Finds the min and max of our grade history data
            double min = gradeHistory[0], max = gradeHistory[0];
            foreach (double data in gradeHistory)
            {
                if (min > data)
                    min = data;

                if (max < data)
                    max = data;
            }

            // Prepares the lable data
            List<Tuple<double, string>> labelData = new List<Tuple<double, string>>();

            // Stores all the possible label
            for (int i = 0; i < 4; ++i)
            {
                if (percCutoff[i] < max && percCutoff[i] > min)
                {
                    Tuple<double, string> currCutoff = new Tuple<double, string>(Convert.ToDouble(percCutoff[i]), Convert.ToString(Convert.ToChar('A' + i)));
                    labelData.Add(currCutoff);
                }
            }

            Draw.Border("Grade History for " + name + ":", 4, Program.currColor);
            Draw.GraphWithData(9, Program.WindowHeight - 3, gradeHistory.ToArray(),
                18, max, min, labelData, Program.TableColor, Program.currColor);            

            Console.SetCursorPosition(2, Program.WindowHeight - 1);
            Program.PrintPressAnyKeyToContinue();
        }

        // Sets choices and logic for pick a catagory screen
        // Choices is just the name of the catagory
        public List<string> CreateChoices()
        {
            List<string> choices = new List<string>();

            foreach (Catagory catagory in catagories)
            {
                choices.Add(catagory.Name);
            }

            // Adds the exit choices
            choices.Add("Exit");

            return choices;
        }
        // Logic is just the index's of the courses (so 0, 1, 2, 3, etc)
        public List<int> CreateLogic()
        {
            List<int> logic = new List<int>();

            for (int i = 0; i < catagories.Count(); ++i)
            {
                logic.Add(i);
            }

            // Adds the exit index
            logic.Add(-1);

            return logic;
        }


    }
}
