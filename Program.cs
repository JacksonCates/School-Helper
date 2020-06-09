using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Name: Jackson Cates
 * 
 * Date: 6/9/2020
 * 
 * Description:
 * 
 * The purpose of this program is to manage courses using files and to give the 
 * user stats about that course. I made this because I have a few profs that grade
 * courses by paper, making it difficult to know my grade throughout the semester.
 * 
 * For this file, it sets up the framework for the program. It creates and sets the 
 * different screens the user will go through and manages the file system. Whenever
 * the program runs a screen, via screenName.RunScreen(), it will return an index
 * to bring the user to a new screen.
 * 
 * What index it returns depends on what screen it needs to go to. If it simply needs
 * to go to another screen with choices that the user chooses, it
 * will simply return an index that correlates with the Screens enum. If it needs to go
 * to a screen for a specific course or category, it will return the index for that course
 * or category, then using the nextScreen variable it will go to that next screen
 * 
 * While for most menus that just lets the user chooses another screen as the
 * choices and logic are "set in stone", the pickCourse and pickCatagory screens require
 * updating logic and choices because the data can change. This is done by the 'Courses"
 * class that sets and return logic and choices, then we use the UpdateScreens function.
 * 
 */

namespace SchoolHelper
{
    // List of possible screens we can have
    enum Screens { MainScreen, ViewCourse, Editcourses, ExitProg, AddNewCourse, PickCourse, GradeManagement, ViewGradeInfo,
        AddNewGrade, PickCatagory, DeleteCourse, DeleteGrade, GpaManager, AddNewCompletedCourse, ViewCompletedCourses, ProjectGPA,
        WhatDoINeedOnFinal, AddOldCompletedCourse, InputExpectedAssignments, ViewGradeHist }

    class Program
    {
        // Boolean to end the program
        static bool endProg = false;

        // Screen logic
        static Screens currScreen; // Current screen for the program to go to
        static Screens returnScreen; // Screen that the program will return to if need be
        static Screens nextScreen; // This will be the screen after if we need to find a specific course/category
        static int courseIndex; // Index for the course
        static int catagoryIndex; // Index for a category in a course
        static int gradeIndex; // Index for a grade in a category in a course

        // Course data
        static Courses courses = new Courses();
        static Courses competedCourses = new Courses();

        // Screens
        static Menu MainScreen = new Menu("Welcome to Jackson's School Helper");
        static Menu EditCoursesScreen = new Menu("Edit Courses Screen");
        static Menu PickCourseScreen = new Menu("Pick a Course Screen");
        static Menu GradeManagementScreen = new Menu("Grade Management Screen for ");
        static Menu PickCatagoryScreen = new Menu("Pick a Category Screen");
        static Menu PickGradeScreen = new Menu("Pick a Grade Screen");
        static Menu GpaManagerScreen = new Menu("GPA Management Screen");

        // Drawing data
        const int StartingWindowWidth = 120;
        const int StartingWindowHeight = 30;
        public static int WindowWidth;
        public static int WindowHeight;
        public static ConsoleColor ForegroundColor;
        public static ConsoleColor BackgroundColor;
        public static ConsoleColor currColor;
        public static ConsoleColor TableColor;
        

        // File System data
        static string oldData; // Old data to replace later
        static string newData; // New data

        static void Main(string[] args)
        {
            // Sets up the file system
            FileSystem.Setup();

            // Reads data from the files and stores it
            courses = FileSystem.ReadCurrentCourseFiles();
            competedCourses = FileSystem.ReadCompletedCourseFiles();

            // Sets up the screens
            SetUp();

            // Loops until the end of the program
            while (endProg == false)
            {
                Console.Clear();

                // Screens with set choices
                switch (currScreen)
                {
                    // Branching screens
                    // Branching screens that lead to other screens
                    case Screens.MainScreen:
                        currColor = ConsoleColor.DarkMagenta;
                        currScreen = (Screens)MainScreen.RunScreen();
                        returnScreen = Screens.MainScreen;
                        break;

                    case Screens.Editcourses:
                        currColor = ConsoleColor.DarkRed;
                        currScreen = (Screens)EditCoursesScreen.RunScreen();
                        returnScreen = Screens.Editcourses;
                        break;

                    case Screens.GradeManagement:
                        currColor = ConsoleColor.DarkGreen;
                        GradeManagementScreen.Title = "Grade Management Screen for " + courses.GetCourse(courseIndex).Name;
                        currScreen = (Screens)GradeManagementScreen.RunScreen();
                        returnScreen = Screens.GradeManagement;
                        break;

                    case Screens.GpaManager:
                        currColor = ConsoleColor.DarkBlue;
                        currScreen = (Screens)GpaManagerScreen.RunScreen();
                        returnScreen = Screens.GpaManager;
                        break;

                }

                // Checks if we need to pick a course, category, or grade
                // current courses
                if (currScreen == Screens.ViewCourse || currScreen == Screens.DeleteCourse || currScreen == Screens.GradeManagement
                    || currScreen == Screens.AddOldCompletedCourse || currScreen == Screens.InputExpectedAssignments)
                {
                    nextScreen = currScreen;

                    // Runs the screen, updates the screen before hand     
                    currColor = ConsoleColor.DarkYellow;
                    UpdateCourseScreen();
                    Console.Clear();
                    courseIndex = (int)PickCourseScreen.RunScreen();

                    // Checks to see if the user picked a course or wanted to exit (return to prev screen)
                    if (courseIndex >= 0)
                    {
                        currScreen = nextScreen;
                    }
                    else
                    {
                        currScreen = returnScreen;
                    }
                }
                // category
                if (currScreen == Screens.AddNewGrade || currScreen == Screens.DeleteGrade || currScreen == Screens.InputExpectedAssignments)
                {
                    nextScreen = currScreen;

                    // Runs the screen, updates the screen before hand
                    currColor = ConsoleColor.DarkYellow;
                    UpdateCatagoryScreen();
                    Console.Clear();
                    catagoryIndex = (int)PickCatagoryScreen.RunScreen();

                    // Checks to see if the user picked a course or wanted to exit (return to prev screen)
                    if (catagoryIndex >= 0)
                    {
                        currScreen = nextScreen;
                    }
                    else
                    {
                        currScreen = returnScreen;
                    }
                }
                // grade
                if (currScreen == Screens.DeleteGrade)
                {
                    nextScreen = currScreen;

                    // Runs the screen, updates the screen before hand
                    currColor = ConsoleColor.DarkYellow;
                    UpdateGradeScreen();
                    Console.Clear();
                    gradeIndex = (int)PickGradeScreen.RunScreen();

                    // Checks to see if the user picked a course or wanted to exit (return to prev screen)
                    if (gradeIndex >= 0)
                    {
                        currScreen = nextScreen;
                    }
                    else
                    {
                        currScreen = returnScreen;
                    }
                }


                Console.Clear();
                // Secondary screens (Without branches) (WILL HAVE A RETURNSCREEN)
                switch (currScreen) {
                    case Screens.ExitProg:
                        // Ends prog
                        endProg = true;
                        break;

                    case Screens.AddNewCourse:
                        // Creates new course
                        Course newCourse = courses.AddNewCourse(false);

                        // Updates the file system
                        FileSystem.CreateCourseFiles(newCourse);

                        // Tells the user the course has been created
                        Console.WriteLine(Environment.NewLine + "Course created!");
                        Program.PrintPressAnyKeyToContinue();

                        currScreen = returnScreen;
                        break;

                    case Screens.ViewCourse:
                        // Runs the viewCourseInfo just for that course
                        courses.GetCourse(courseIndex).ViewCourseInfo();

                        currScreen = returnScreen;
                        break;

                    case Screens.ViewGradeInfo:
                        // Runs the viewGradeInfo just for that course
                        courses.GetCourse(courseIndex).ViewGradeInfo();

                        currScreen = returnScreen;
                        break;

                    case Screens.AddNewGrade:
                        // Creates a new grade
                        Grade newGrade = courses.GetCourse(courseIndex).GetCatagory(catagoryIndex).AddNewGrade();

                        // Updates the file system
                        FileSystem.UpdateGradeFiles(courses.GetCourse(courseIndex), catagoryIndex, newGrade);
                        FileSystem.UpdateGradeHist(courses.GetCourse(courseIndex).GetPercGrade(), courses.GetCourse(courseIndex));
                        courses.GetCourse(courseIndex).AddGradeHist(courses.GetCourse(courseIndex).GetPercGrade());

                        // Tells the user the grade has been created
                        Console.SetCursorPosition(2, 12);
                        Console.WriteLine("Grade created!");
                        Console.SetCursorPosition(2, Program.WindowHeight - 1);
                        Program.PrintPressAnyKeyToContinue();

                        currScreen = returnScreen;
                        break;

                    case Screens.DeleteCourse:

                        // Asks the user if we want to delete this course
                        bool needDelete = courses.DeleteCourse(courseIndex);

                        // If so, delete course
                        if (needDelete)
                        {
                            FileSystem.DeleteCourseFiles(courses.GetCourse(courseIndex));

                            // Removes the course from the courses vector
                            courses.RemoveCourse(courseIndex);
                        }

                        currScreen = returnScreen;

                        break;

                    case Screens.DeleteGrade:

                        // Gets the course that we need to delete
                        needDelete = courses.GetCourse(courseIndex).GetCatagory(catagoryIndex).DeleteGrade(gradeIndex);

                        // Checks if the user wants to delete the grade
                        if (needDelete)
                        {
                            FileSystem.DeleteGradeInfo(courses.GetCourse(courseIndex), catagoryIndex, gradeIndex);
                            FileSystem.UpdateGradeHist(courses.GetCourse(courseIndex).GetPercGrade(), courses.GetCourse(courseIndex));
                            courses.GetCourse(courseIndex).AddGradeHist(courses.GetCourse(courseIndex).GetPercGrade());

                            // Tells the user the course has been deleted
                            Console.SetCursorPosition(2, 5);
                            Console.WriteLine("Grade Deleted!");
                            Console.SetCursorPosition(2, Program.WindowHeight - 1);
                            Program.PrintPressAnyKeyToContinue();
                        }

                        currScreen = returnScreen;

                        break;

                    case Screens.ViewCompletedCourses:
                        competedCourses.ViewCompetedCourses();
                        currScreen = returnScreen;
                        break;

                    case Screens.AddNewCompletedCourse:
                        newCourse = competedCourses.AddNewCourse(true);
                        FileSystem.UpdateCompletedCourseFiles(newCourse);
                        currScreen = returnScreen;
                        break;

                    case Screens.ProjectGPA:
                        courses.ProjectGpa(competedCourses.GetGradePointsEarned(), competedCourses.GetGradePointsTotal());
                        currScreen = returnScreen;
                        break;

                    case Screens.WhatDoINeedOnFinal:
                        courses.ViewFinalNeeds();
                        currScreen = returnScreen;
                        break;

                    case Screens.AddOldCompletedCourse:
                        newCourse = courses.AddOldCompletedCourse(courseIndex);
                        competedCourses.CreateCourse(newCourse);

                        // Deletes the old course in files, updates the files for completed course
                        FileSystem.DeleteCourseFiles(newCourse);
                        FileSystem.UpdateCompletedCourseFiles(newCourse);

                        currScreen = returnScreen;
                        break;

                    case Screens.InputExpectedAssignments:
                        // Save old data
                        oldData = Convert.ToString(courses.GetCourse(courseIndex).GetCatagory(catagoryIndex).ExpAssignments);
                        newData = Convert.ToString(courses.GetCourse(courseIndex).GetCatagory(catagoryIndex).InputExpAssignments());
                        FileSystem.UpdateExpAssignmentInfo(newData, courses.GetCourse(courseIndex), catagoryIndex);
                        currScreen = returnScreen;
                        break;

                    case Screens.ViewGradeHist:
                        courses.GetCourse(courseIndex).ViewGradeHist();
                        currScreen = returnScreen;
                        break;
                }

            }
        }

        // Sets up screen that have "set in stone" choices
        private static void SetUp()
        {
            // Gets the window width and height
            WindowHeight = StartingWindowHeight - 1;
            WindowWidth = StartingWindowWidth - 1;
            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
            TableColor = ConsoleColor.DarkGray;
            //Console.SetBufferSize(WindowWidth + 1, WindowHeight + 1);
            Console.SetWindowPosition(0, 0);

            // Sets first screen to go to
            currScreen = Screens.MainScreen;

            // Sets up the screens
            MainScreen.SetChoices(new List<string> { "Grade Management Screen", "Edit Courses Screen", "GPA Management Screen", "What Do I Need On My Final?", "Exit Program" });
            MainScreen.SetLogic(new List<Screens> { Screens.GradeManagement, Screens.Editcourses, Screens.GpaManager, Screens.WhatDoINeedOnFinal, Screens.ExitProg });

            EditCoursesScreen.SetChoices(new List<string> { "View Course Information", "Add a New Course", "Delete a Course", "Input Expected Assignments", "Return to Main Screen" });
            EditCoursesScreen.SetLogic(new List<Screens> { Screens.ViewCourse, Screens.AddNewCourse, Screens.DeleteCourse, Screens.InputExpectedAssignments, Screens.MainScreen });

            GradeManagementScreen.SetChoices(new List<string> { "View Grade Info", "View Grade History", "Add a new Grade", "Delete a Grade", "Return to Main Screen" });
            GradeManagementScreen.SetLogic(new List<Screens> { Screens.ViewGradeInfo, Screens.ViewGradeHist, Screens.AddNewGrade, Screens.DeleteGrade, Screens.MainScreen });

            GpaManagerScreen.SetChoices(new List<string> { "View Completed Courses", "Project Future GPA", "Add Completed Course", "Add Completed Course from Current Courses", "Return to Main Screen" });
            GpaManagerScreen.SetLogic(new List<Screens> { Screens.ViewCompletedCourses, Screens.ProjectGPA, Screens.AddNewCompletedCourse, Screens.AddOldCompletedCourse, Screens.MainScreen });

        }

        // Updates screens for when we need to pick a course or category or grade
        private static void UpdateCourseScreen()
        {
            PickCourseScreen.SetChoices(courses.CreateChoices());
            PickCourseScreen.SetLogic(courses.CreateLogic());
        }
        private static void UpdateCatagoryScreen()
        {
            // BUG IS HERE, NEED TO FIND A WAY TO UPDATE ALL OF CATAGORIES, NOT JUST A SPECIFIC COURSE
            PickCatagoryScreen.SetChoices(courses.GetCourse(courseIndex).CreateChoices());
            PickCatagoryScreen.SetLogic(courses.GetCourse(courseIndex).CreateLogic());
        }
        private static void UpdateGradeScreen()
        {
            PickGradeScreen.SetChoices(courses.GetCourse(courseIndex).GetCatagory(catagoryIndex).CreateChoices());
            PickGradeScreen.SetLogic(courses.GetCourse(courseIndex).GetCatagory(catagoryIndex).CreateLogic());
        }


        // A display so the user knows to press any key to continue
        public static void PrintPressAnyKeyToContinue()
        {
            // Hides the cursor
            Console.CursorVisible = false;

            Console.Write("Press any key to continue: ");
            Console.ReadKey();

            // Shows the cursor
            Console.CursorVisible = true;
        }

        // Temp fix it
        private static void PrintFixMe()
        {
            Console.WriteLine("FIXME: " + currScreen);
            PrintPressAnyKeyToContinue();
        }

        public static char getYesOrNo()
        {
            // Gets the current cursor information
            int cursorWidthPos = Console.CursorLeft;
            int cursorHeightPos = Console.CursorTop;

            string userInput = Console.ReadLine().ToLower();

            // Checks for the user input
            while (userInput.Length != 1 || userInput[0] != 'y' && userInput[0] != 'n')
            {
                // Resets the input and moves it
                Console.SetCursorPosition(cursorWidthPos, cursorHeightPos);

                // Asks and scans for input
                Console.Write("Incorrect choice, try again: ");
                userInput = Console.ReadLine().ToLower();
            }

            return userInput[0];
        }
    }
}
