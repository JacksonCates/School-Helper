using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/* Description:
 * 
 * The file system deals with reading and writing data to the files for future use.
 * 
 * Each course will get its own folder in the "FileSystem" folder. This folder will
 * have the course name in the folder name. For each course folder, it will have a
 * variety of files to store various information.
 * 
 * The first file is the general course information file. This file can be found by
 * its name by the course name followed by "COURSEINFO." This file contains the course
 * name, the number of credits, and the grade cut offs (1st is for an 'A', 2nd is for an
 * 'B', etc).
 * 
 * The second file is the catagory information file. This file can be found by its
 * name by the course name followed by "CATAGORYINFO." This file contains the name of
 * each catagory, followed by its corresponding weight.
 * 
 * The rest of the files are grade information. Each catagory will have a file containing
 * the grades for that catagory. The file canbe found by its name by the course name, 
 * followed by the catagory name, then "GRADEINFO." This stores all the grade's name, 
 * points earned, and points possible.
 * 
 * The file system also updates the files whenever a new grade or course it added.
 * 
 */

namespace SchoolHelper
{
    // This class manages the files in the system
    class FileSystem
    {
        
        private static string FileSystemDir = @"FileSystem\";
        private static string FileExt = ".xlsx";
        private static string[] dirs;
        private static List<string[]> files = new List<string[]>(); // Sets of files per folder
        private static List<Course> coursesToUpdate = new List<Course>();
        private const string GeneralSheetName = "General";
        private const string EndOfData = "END";

        // Finds all of the files in the folder and prints the paths for testing
        public static void Setup()
        {
            // Opens the file and read the paths
            dirs = Directory.GetDirectories(FileSystemDir);

            // Finds the paths for each file
            foreach (string dir in dirs)
            {
                files.Add(Directory.GetFiles(dir));
            }


            // Prints to console for testing
            foreach (string dir in dirs)
            {
                Console.WriteLine(dir);

            }

            // Prints the files for testing
            foreach (string[] fileSet in files)
            {
                foreach (string file in fileSet)
                {
                    Console.WriteLine(file);
                }
            }
        }

        // Read the files and returns our courses data
        public static Courses ReadCurrentCourseFiles()
        {
            Courses courseData = new Courses();

            // Current Course data
            // Reads through each fileSet in each folder
            foreach (string[] fileSet in files)
            {
                // Instantiate new course
                Course newCourse = new Course();   
                
                // Finds the course information folder 
                for (int i = 0; i < fileSet.Length; ++i)
                {
                    // Fills all the information about the course
                    if (fileSet[i].Contains("COURSEINFO"))
                    {
                        // Opens the excel
                        Excel excel = new Excel(Path.GetFullPath(fileSet[i]));

                        // Open the general information sheet
                        excel.OpenSheet("General");

                        // Gets the data
                        newCourse.Name = excel.ReadCell(1, 2);
                        newCourse.Credits = Convert.ToInt16(excel.ReadCell(2, 2));
                        newCourse.FinalWeight = Convert.ToInt16(excel.ReadCell(3, 2));

                        // Read cutoffs
                        List<int> cutOffs = new List<int>();
                        for (int j = 0; j < 4; ++j)
                        {
                            cutOffs.Add(Convert.ToInt16(excel.ReadCell(2 + j, 5)));
                        }
                        newCourse.PercCutoff = cutOffs;

                        // Read and add catagories
                        List<Catagory> currCats = new List<Catagory>();
                        for (int j = 2; excel.ReadCell(j, 7).Equals(EndOfData) == false; j++)
                        {
                            Catagory currCat = new Catagory();
                            currCat.Name = excel.ReadCell(j, 7);
                            currCat.Weight = Convert.ToInt16(excel.ReadCell(j, 8));

                            // Adds it to the vector
                            currCats.Add(currCat);
                        }

                        // Adds the cats to the course
                        newCourse.Catagories = currCats;

                        // Adds grade history information
                        excel.OpenSheet("GradeHistory");
                        for (int j = 2; excel.ReadCell(j, 1).Equals(EndOfData) == false; j++)
                        {
                            newCourse.AddGradeHist(Convert.ToDouble(excel.ReadCell(j, 2)));
                        }

                        // Search for each sheets in the catagory vector
                        foreach (Catagory catagory in newCourse.Catagories)
                        {
                            // Opens the sheet
                            excel.OpenSheet(catagory.Name);

                            // Assignments expected
                            catagory.ExpAssignments = Convert.ToInt16(excel.ReadCell(3, 2));

                            // Grade information
                            for (int k = 3; excel.ReadCell(k, 4).Equals(EndOfData) == false; k++)
                            {
                                // Makes a new grade
                                Grade currGrade = new Grade();

                                // Adds the data
                                currGrade.Name = excel.ReadCell(k, 4);
                                currGrade.PointsEarned = Convert.ToInt16(excel.ReadCell(k, 5));
                                currGrade.PointsTotal = Convert.ToInt16(excel.ReadCell(k, 6));

                                // Adds the grade to the catagory
                                catagory.Grades.Add(currGrade);
                            }
                        }
                        // Close the excel
                        excel.Close();
                    }
                }
                // Adds the new course
                courseData.CreateCourse(newCourse);
            }

                return courseData;
        }

        public static Courses ReadCompletedCourseFiles()
        {
            Courses courseData = new Courses();

            string dirPath = @"FileSystem\COMPLETEDCOURSESINFO.txt";

            // Reads all of the lines
            string[] lines = File.ReadAllLines(dirPath);

            // Stores the information
            for (int i = 0; i < lines.Length; i += 3)
            {
                // first line is name
                string name = lines[i];

                // Second line is credits
                int credits = Convert.ToInt16(lines[i + 1]);

                // Third line is grade recieved
                char letterGrade = Convert.ToChar(lines[i + 2]);

                // Adds the new course in our vector
                courseData.CreateCourse(new Course(name, credits, letterGrade));
            }

            return courseData;
        }

        // Creates a new set of folders and files for a new course
        public static void CreateCourseFiles(Course newCourse)
        {
            string dirPath = FileSystemDir + newCourse.Name;

            // Creates a new folder 
            Directory.CreateDirectory(dirPath);
                        
            // Creates the excel file and writes into it
            string infoFilePath = dirPath + "\\" + newCourse.Name + "COURSEINFO" + FileExt;
            Excel excel = new Excel(infoFilePath);

            // Uses the first sheet that is the general information sheet
            excel.OpenSheet(1);
            excel.RenameCurrentSheet(GeneralSheetName);

            // Writes the data
            excel.AlignCol(1, "left");
            excel.SetColWidth(1, 18);
            excel.WriteCol(1, 1, new string[]
            {
                "Course Name:",
                "Credits:",
                "Final Weight:"
            });
            excel.AlignCol(2, "center");
            excel.WriteCol(1, 2, new string[]
            {
                newCourse.Name,
                newCourse.Credits.ToString(),
                newCourse.FinalWeight.ToString()
            });

            // Cutoff
            excel.AlignCol(4, "left");
            excel.AlignCol(5, "center");
            excel.WriteCell(1, 4, "Grade Scale:");
            excel.WriteCol(2, 4, new string[] { "A:", "B:", "C:", "D:" });
            excel.WriteCol(2, 5, newCourse.GetPercCutoffAsString());

            // Catagory information
            excel.AlignCol(7, "left");
            excel.SetColWidth(7, 24);
            excel.AlignCol(8, "center");
            excel.WriteCell(1, 7, "Catagory Weights:");

            // Creates the gradeHistory sheet
            excel.AlignCol(2, "center");
            excel.AlignCol(1, "left");
            excel.SetColWidth(1, 18);
            excel.AddSheet("GradeHistory");
            excel.WriteCell(1, 1, "Date:");
            excel.WriteCell(1, 2, "Grade:");
            excel.WriteCell(2, 1, EndOfData);

            // Creates a new sheet for each catagory, edits the sheet and program information page
            for (int i = 0; i < newCourse.GetNumCatagories(); ++i)
            {
                Catagory currCat = newCourse.Catagories[i];

                // Writes on the general sheet
                excel.OpenSheet(GeneralSheetName);
                excel.WriteCell(2 + i, 7, currCat.Name);
                excel.WriteCell(2 + i, 8, currCat.Weight.ToString());

                // Adds the sheet
                excel.AddSheet(currCat.Name);

                // Adds the catagory information on the sheet
                excel.AlignCol(1, "left");
                excel.SetColWidth(1, 24);
                excel.WriteCol(1, 1, new string[]
                {
                    "Catagory Name:",
                    "Weight:",
                    "Assignments Expected:"
                });
                excel.AlignCol(2, "center");
                excel.WriteCol(1, 2, new string[]
                {
                    currCat.Name,
                    currCat.Weight.ToString(),
                    currCat.ExpAssignments.ToString()
                });

                // Starts the grade table
                excel.AlignCol(4, "left");
                excel.AlignCol(5, "center");
                excel.AlignCol(6, "center");
                excel.SetColWidth(4, 18);
                excel.SetColWidth(5, 18);
                excel.SetColWidth(6, 18);
                excel.WriteCell(1, 4, "Grade Information:");
                excel.WriteCell(2, 4, "Name:");
                excel.WriteCell(2, 5, "Points Earned:");
                excel.WriteCell(2, 6, "Points Total:");
                excel.WriteCell(3, 4, EndOfData);
            }

            // Adds the end of data on general sheet
            excel.OpenSheet(GeneralSheetName);
            excel.AppendAtCol(2, 7, EndOfData);

            // Closes the file
            excel.Close();            
        }

        // Adds a new grade information to files
        public static void UpdateGradeFiles(Course course, int catagoryIndex, Grade grade)
        {
            // Opens the excel document
            Excel excel = new Excel(Path.GetFullPath(FindCourseFilePath(course)));
            excel.OpenSheet(course.Catagories[catagoryIndex].Name);

            // Finds the end of data cell and replaces it
            int rowIndex = excel.ReplaceAtCol(3, 4, EndOfData, grade.Name);
            excel.WriteCell(rowIndex, 5, grade.PointsEarned.ToString());
            excel.WriteCell(rowIndex, 6, grade.PointsTotal.ToString());
            excel.AppendAtCol(++rowIndex, 4, EndOfData);

            excel.Close();
        }

        public static void UpdateCompletedCourseFiles(Course newCourse)
        {
            string FilePath = @"FileSystem\COMPLETEDCOURSESINFO.txt";

            File.AppendAllLines(FilePath, new List<string>
            {
                // First the name, then credits, then grade
                newCourse.Name,
                Convert.ToString(newCourse.Credits),
                Convert.ToString(newCourse.LetterGrade)

            });
        }

        // Deletes a course information
        public static void DeleteCourseFiles(Course delCourse)
        {
            // Gets the course filepath
            string dirPath = @"FileSystem\" + delCourse.Name;

            // Deletes the course and all files inside of it
            Directory.Delete(dirPath, true);
        }


        // Deletes a grade information
        public static void DeleteGradeInfo(Course course, int catagoryIndex, int gradeIndex)
        {
            string FilePath = FindCourseFilePath(course);
            Excel excel = new Excel(Path.GetFullPath(FilePath));
            excel.OpenSheet(course.Catagories[catagoryIndex].Name);

            // Removes it
            excel.RemoveRowAndShift(3, 4, 6, course.Catagories[catagoryIndex].Grades[gradeIndex].Name);

            // Removes the grade from the vector
            course.Catagories[catagoryIndex].GetGrade(gradeIndex);
            excel.Close();
        }


        public static void UpdateExpAssignmentInfo(string newData, Course course, int catagoryIndex)
        {
            // Creats the string path
            string FilePath = FindCourseFilePath(course);
            Excel excel = new Excel(Path.GetFullPath(FilePath));
            excel.OpenSheet(course.Catagories[catagoryIndex].Name);

            // Replaces it
            excel.WriteCell(3, 2, newData);

            excel.Close();
        }

        public static void UpdateGradeHist(double newData, Course course)
        {
            // Creats the string path
            string FilePath = FindCourseFilePath(course);
            Excel excel = new Excel(Path.GetFullPath(FilePath));
            excel.OpenSheet("GradeHistory");

            // Update the grade history
            DateTime dateTime = DateTime.UtcNow.Date;
            int rowIndex = excel.ReplaceAtCol(2, 1, EndOfData, dateTime.ToString("MM/dd/yyyy"));
            excel.WriteCell(rowIndex, 2, String.Format("{0:0.00}\n", newData));
            excel.AppendAtCol(2, 1, EndOfData);

            excel.Close();
        }
        
        //Finds a specific grade file
        private static string FindGradeFile(Course course, int catagoryIndex)
        {
            string GradeFilePath = @"FileSystem\" + course.Name + "\\" + course.Name + course.GetCatagory(catagoryIndex).Name + "GRADEINFO.txt";

            return GradeFilePath;
        }

        private static string FindCourseFilePath(Course course)
        {
            string infoFilePath = FileSystemDir + course.Name + "\\" + course.Name + "COURSEINFO" + FileExt;

            return infoFilePath;
        }
    }
}
