using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Description:
 * 
 * This is the class for the manager of multiple courses. It allows to set multiple courses,
 * get and set a single course, the screen to add a new course from user input, and to set
 * the logic and choices for our pickCourse screen using the course information that is
 * currently there.
 *  
 * This class will also contain screens needed to manage multiple courses. Such as adding and deleting
 * a course.
 */

namespace SchoolHelper
{
    class Courses
    {
        private List<Course> courses = new List<Course> { };
        private int gradePointsEarned;
        private int gradePointsTotal;
        private double gpa;

        // Constructor
        public Courses()
        {
            gradePointsEarned = 0;
            gradePointsTotal = 0;
            gpa = 0;
        }

        // Property function for a single course
        public Course GetCourse(int courseIndex)
        {
            return courses[courseIndex];
        }

        public void RemoveCourse(int courseIndex)
        {
            courses.RemoveAt(courseIndex);
        }

        public void CreateCourse(Course newCourse)
        {
            courses.Add(newCourse);
        }

        // Property functions for an array of courses
        public void CreateCourses(Course[] newCourses)
        {
            foreach (Course course in newCourses)
            {
                courses.Add(course);
            }
        }
        public int GetGradePointsEarned()
        {
            CalcGpa(0,0);
            return gradePointsEarned;
        }
        public int GetGradePointsTotal()
        {
            CalcGpa(0,0);
            return gradePointsTotal;
        }
        public int CoursesLength
        {
            get => courses.Count();
        }

        // Converts a letter grade to grade points
        private int findGradePointsFromGrade(char letterGrade)
        {
            int gradePoints = 0;

            switch (letterGrade)
            {
                case 'A':
                    gradePoints = 4;
                    break;
                case 'B':
                    gradePoints = 3;
                    break;
                case 'C':
                    gradePoints = 2;
                    break;
                case 'D':
                    gradePoints = 1;
                    break;
                case 'F':
                    gradePoints = 0;
                    break;
            }

            return gradePoints;
        }

        // Calculates overall gpa
        private void CalcGpa(int extraPointsEarned, int extraPointsTotal)
        {
            gradePointsEarned = 0;
            gradePointsTotal = 0;

            // Calculates total and earned points from completed course data
            foreach (Course course in courses)
            {
                gradePointsTotal += course.Credits;

                gradePointsEarned += course.Credits * findGradePointsFromGrade(course.LetterGrade);
            }

            // Does the same for additional courses
            gradePointsTotal += extraPointsTotal;
            gradePointsEarned += extraPointsEarned;

            // Calculates gpa
            gpa = Convert.ToDouble(gradePointsEarned) / gradePointsTotal;
        }

        // Project GPA screen
        public void ProjectGpa(int extraPointsEarned, int extraPointsTotal)
        {
            Draw.Border("GPA Projection Screen:", 4, Program.currColor);
            int pointsEarned = 0, pointsTotal = 0;

            // Asks the user to input a grade they'll think they'll get for each course
            Console.SetCursorPosition(3, 6);
            Console.WriteLine("Write grade for each course:");
            int currHeight = 8;
            int currWidth = 5;
            foreach (Course course in courses)
            {
                Console.SetCursorPosition(currWidth, currHeight);
                Console.Write(course.Name + ": ");

                // Gets the grade
                char letterGrade = Char.ToUpper(Convert.ToChar(Console.ReadLine()));

                // Checks choice
                while (letterGrade != 'A' && letterGrade != 'B' && letterGrade != 'C' && letterGrade != 'D' && letterGrade != 'F')
                {
                    ++currHeight;
                    Console.SetCursorPosition(currWidth - 2, ++currHeight);
                    Console.WriteLine("Incorrect choice, try again");
                    Console.SetCursorPosition(currWidth, ++currHeight);
                    Console.Write(course.Name + ": ");
                    letterGrade = Char.ToUpper(Convert.ToChar(Console.ReadLine()));

                    // Checks if we have reached too low
                    if (currHeight >= Program.WindowHeight - 6)
                    {
                        currHeight = 4;
                        currWidth = Program.WindowWidth / 2;
                    }
                }

                // Adds it to the total
                pointsTotal += course.Credits;
                pointsEarned += findGradePointsFromGrade(letterGrade) * course.Credits;
                currHeight++;

                // Checks if we have reached too low
                if (currHeight >= Program.WindowHeight - 6)
                {
                    currHeight = 9;
                    currWidth = Program.WindowWidth / 2;
                }
            }

            // Prints the projected gpa
            gpa = Convert.ToDouble(pointsEarned + extraPointsEarned) / (extraPointsTotal + pointsTotal);
            Console.SetCursorPosition(currWidth - 2, ++currHeight);
            Console.WriteLine("Your projected GPA: {0:0.00}", gpa);
            Console.SetCursorPosition(3, Program.WindowHeight - 2);
            Program.PrintPressAnyKeyToContinue();
        }

        // View "What do I need on my final?" screen
        public void ViewFinalNeeds()
        {
            // Draws the border and table
            ConsoleColor borderColor = Program.currColor;
            Draw.Border("What Do I Need On My Final? Screen:", 4, borderColor);

            // Finds the number of courses with finals
            int numFinals = 0;
            foreach (Course course in courses)
            {
                if (course.FinalWeight != 0)
                {
                    numFinals++;
                }
            }


            // Shows data if there are finals
            if (numFinals != 0)
            {
                // Collects the data
                string[,] data = new string[numFinals, 8];

                int courseIndex = 0;
                for (int i = 0; i < courses.Count(); ++i)
                {
                    if (courses[i].FinalWeight != 0)
                    {
                        // Name
                        data[courseIndex, 0] = courses[i].Name;

                        //% Grade

                        data[courseIndex, 1] = String.Format("{0:0.00}", courses[i].GetPercGrade()) + "% ~ " + Convert.ToString(courses[i].FindLetterGrade(courses[i].GetPercGrade()));

                        // Final Weight
                        data[courseIndex, 2] = Convert.ToString(courses[i].FinalWeight) + "%";

                        // Stores each letter grade
                        for (int j = 0; j < 4; ++j)
                        {
                            Course course = courses[i];
                            data[courseIndex, j + 3] = String.Format("{0:0.00}", (course.PercCutoff[j] - course.GetPercGrade() * (1 - course.FinalWeight / 100.0)) / (course.FinalWeight / 100.0)) + "%";
                        }

                        // % Completed
                        data[courseIndex, 7] = String.Format("{0:0.00}", courses[i].GetPercComplete()) + "%";

                        courseIndex++;
                    }
                }

                // Draws the tab;e
                Draw.TableWithData(6, 6, 18, new string[] {
                "NAME:", "% GRADE", "FINAL WEIGHT", "A:", "B:", "C:", "D:", "% COMPLETED:"},
                     Program.TableColor, data, colorFormat: FormatFinalNeededColor);


            }

            // Shows there are no finals in file
            else
            {
                Console.SetCursorPosition(2, 6);
                Console.Write("There are no courses with finals!");
            }

            Console.SetCursorPosition(2, Program.WindowHeight - 1);
            Program.PrintPressAnyKeyToContinue();
        }

        // View courses Shows all courses with name, credits and letter grade
        public void ViewCompetedCourses()
        {
            Draw.Border("Completed Course Information:", 6, Program.currColor);

            // Calculates and display GPA
            CalcGpa(0, 0);
            Console.SetCursorPosition(2, 4);
            Console.WriteLine("Current GPA: {0:0.00}", gpa);
            Console.SetCursorPosition(Program.WindowWidth - 30, 4);
            Console.WriteLine("Format: NAME(CREDITS) ~ GRADE");

            // Prints the course data
            const int widthPerCourse = 20;
            const int startingHeight = 8;
            const int startingWidth = 2;
            const int coursesPerLine = 6;
            int currWidth = 0;
            int currHeight = startingHeight;
            foreach (Course course in courses)
            {
                if (currWidth < coursesPerLine)
                {
                    // Prints the info
                    Console.SetCursorPosition(widthPerCourse * currWidth + startingWidth, currHeight);
                    Console.Write(course.Name + "(" + course.Credits + ") ~ " + course.LetterGrade);
                    currWidth++;
                }
                else
                {
                    // Increases the currentHeight, resets other info
                    currHeight++;
                    currWidth = 1;

                    // Prints the info
                    Console.SetCursorPosition(startingWidth, currHeight);
                    Console.Write(course.Name + "(" + course.Credits + ") ~ " + course.LetterGrade);
                }
            }

            Console.SetCursorPosition(2, Program.WindowHeight - 2);
            Program.PrintPressAnyKeyToContinue();
        }

        // Screen to add a new course
        public Course AddNewCourse(bool competedCourse)
        {
            // Screen to add new course
            Course newCourse = new Course();

            // Ask and scan the information for our new course object
            Console.WriteLine("Add a New Course Screen" + Environment.NewLine);

            Console.Write("Enter the name: ");
            newCourse.Name = Console.ReadLine().ToUpper();

            Console.Write(Environment.NewLine + "Enter credits: ");
            newCourse.Credits = Convert.ToInt16(Console.ReadLine());

            if (competedCourse == false)
            {
                Console.WriteLine(Environment.NewLine + "Enter category information (NAME WEIGHT): ");

                // Data for category info
                int weightTotal = 0;
                string line;
                int spaceIndex;
                int currWeight; // Place holder for weight
                Category currCat; // Current category
                List<Category> currCatagories = new List<Category>();

                // Gets the category information from the user
                while (weightTotal != 100)
                {
                    line = Console.ReadLine();
                    line = line.ToUpper();

                    // Breaks the line into the two information we need
                    currCat = new Category();
                    spaceIndex = line.IndexOf(' ');
                    currCat.Name = line.Substring(0, spaceIndex);
                    currWeight = Convert.ToInt16(line.Substring(spaceIndex, line.Length - spaceIndex));
                    currCat.Weight = currWeight;
                    weightTotal += currWeight;

                    // Checks if its a final
                    if (currCat.Name != "FINAL")
                    {
                        // Stores it into the list
                        currCatagories.Add(currCat);
                    }
                    else
                    {
                        // Add the final weight
                        newCourse.FinalWeight = currCat.Weight;
                    }

                    // Checks if user puts it over 100
                    if (weightTotal > 100)
                    {
                        Console.WriteLine("ERROR: Weights are greater than 100. Please try again");
                        weightTotal = 0;
                        currCatagories.Clear();
                        newCourse.FinalWeight = 0;
                    }
                }

                // Adds cat info to course
                newCourse.Catagories = currCatagories;

                // Inputs cutoff
                List<int> percCutoff = new List<int>();
                Console.WriteLine(Environment.NewLine + "Enter Grade Cutoff Information:");

                // Adds the cutoff for each
                for (char i = 'A'; i <= 'D'; ++i)
                {
                    Console.Write("   " + i + ": ");
                    percCutoff.Add(Convert.ToInt16(Console.ReadLine()));

                    // Checks user input
                    if (i != 'A' && percCutoff[(i - 'A') - 1] <= percCutoff[(i - 'A')])
                    {
                        // Prints The error msg
                        Console.WriteLine(Environment.NewLine + "Error - Incorrect input - Try again:");

                        // Rests the information
                        i = 'A';
                        i--;
                        percCutoff.Clear();
                    }
                }

                // Adds cutoff information
                newCourse.PercCutoff = percCutoff;
            }
            else
            {
                Console.Write(Environment.NewLine + "Enter grade received: ");
                char letterGrade = Char.ToUpper(Convert.ToChar(Console.ReadLine()));

                // Checks choice
                while (letterGrade != 'A' && letterGrade != 'B' && letterGrade != 'C' && letterGrade != 'D' && letterGrade != 'F')
                {
                    Console.WriteLine("Incorrect choice, try again");
                    Console.Write("Enter grade revived: ");
                    letterGrade = Char.ToUpper(Convert.ToChar(Console.Read()));
                }

                // Updates the course
                newCourse.LetterGrade = letterGrade;
            }

            // Adds course to our vector
            CreateCourse(newCourse);           

            return newCourse;
        }

        // Screen to add a new completed course
        public Course AddOldCompletedCourse(int courseIndex)
        {
            Draw.Border("Add Completed Current Course Screen", 4, Program.currColor);

            // Displays general course information
            Console.SetCursorPosition(2, 6);
            Console.WriteLine("Course information:");
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("Name: " + courses[courseIndex].Name);
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("Credits " + courses[courseIndex].Credits);

            // Gets the grade received in the course
            Console.SetCursorPosition(2, 12);
            Console.Write("Enter grade received: ");
            char letterGrade = Char.ToUpper(Convert.ToChar(Console.ReadLine()));

            // Checks choice
            int currPos = 14;
            while (letterGrade != 'A' && letterGrade != 'B' && letterGrade != 'C' && letterGrade != 'D' && letterGrade != 'F')
            {
                Console.SetCursorPosition(2, currPos++);
                Console.WriteLine("Incorrect choice, try again");
                Console.SetCursorPosition(2, currPos++);
                Console.Write("Enter grade revived: ");
                letterGrade = Char.ToUpper(Convert.ToChar(Console.Read()));
                currPos += 2;
            }

            courses[courseIndex].LetterGrade = letterGrade;

            // Deletes the course
            Course completedCourse = courses[courseIndex];
            courses.RemoveAt(courseIndex);

            return completedCourse;
        }

        public void EditCourseInfo(int courseIndex)
        {
            Console.WriteLine("FIX ME, EDIT COURSE INFO, IN COURSES CLASS");
            Program.PrintPressAnyKeyToContinue();
        }

        // Returns if we need to delete or nod
        public bool DeleteCourse(int courseIndex)
        {
            // Checks if the user wants to delete the course
            Console.WriteLine("DELETED COURSES CAN NOT BE BROUGHT BACK:");
            Console.Write("Are you sure (y/n)? ");
            char userInput = Program.getYesOrNo();

            // Deletes the course
            if (userInput == 'y')
            {
                // Tells the user the course has been deleted
                Console.WriteLine(Environment.NewLine + "Course Deleted!");
                Program.PrintPressAnyKeyToContinue();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Sets choices for pick a course screen
        public List<string> CreateChoices()
        {
            List<string> choices = new List<string>();

            // Add choices of the course name
            foreach(Course course in courses)
            {
                choices.Add(course.Name);
            }

            // Adds the exit choices
            choices.Add("Exit");

            return choices;
        }
        // For this function, we will actually be return course index instead of screens
        public List<int> CreateLogic()
        {
            List<int> logic = new List<int>();

            // Adds logic of the index in the course (so 0, 1, 2, 3, 4, etc)
            for (int i = 0; i < courses.Count(); ++i)
            {
                logic.Add(i);
            }

            // Adds the exit index
            logic.Add(-1);

            return logic;
        }


        // Used to format colors
        private void FormatFinalNeededColor(string data, int currCol, string name)
        {
            // Only prints in the grading cols
            if (currCol >= 3 && currCol <= 6 )
            {
                double currData = Convert.ToDouble(data.Substring(0, data.Length - 1));

                // Finds the perc cutoff
                int cutOff = 0;
                foreach(Course course in courses)
                {
                    if (name == course.Name)
                    {
                        cutOff = course.PercCutoff[currCol - 3];
                    }
                }
                
                // Prints the color
                if (currData > 100)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (currData < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (cutOff - 1 < currData )
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
            }
        }
    }
}
