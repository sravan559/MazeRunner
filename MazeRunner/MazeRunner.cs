using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner
{
    class MazeRunner
    {
        #region Maze Variables
        private static int startColumnIndexInMaze = -1; // To determine the location of Start Point column index of the maze
        private static int startRowIndexInMaze = -1; // To determine the location of Start Point Row index of the maze
        private static int finishColumnIndexInMaze = -1;// To determine the location of Finish Point column index of the maze
        private static int finishRowIndexInMaze = -1;   //To determine the location of Finish Point Row index of the maze

        private static int RowsCount = 0; // Counting the number of lines to determine the rows size of the maze
        private static int ColumnsCount = 0; // Counting the number of characters in a line to determine the columns size of the maze
        #endregion

        #region MazeProblem

        /// <summary>
        /// Used to read the text file for maze input and load the maze
        /// </summary>
        /// <returns></returns>
        static List<char[]> LoadMaze()
        {
            List<char[]> list = null;
            string line = string.Empty; ;
            Console.WriteLine("Reading Maze File Input: ");
            // Read the file and display it line by line.
            try
            {
                StreamReader file = new StreamReader("maze.txt");
                list = new List<char[]>();
                while ((line = file.ReadLine()) != null)
                {
                    startRowIndexInMaze = (startRowIndexInMaze == -1 && line.IndexOf("S") > -1) ? (RowsCount) : startRowIndexInMaze;
                    startColumnIndexInMaze = startColumnIndexInMaze == -1 ? line.IndexOf("S") : startColumnIndexInMaze;

                    finishRowIndexInMaze = (finishRowIndexInMaze == -1 && line.IndexOf("F") > -1) ? (RowsCount) : finishRowIndexInMaze;
                    finishColumnIndexInMaze = finishColumnIndexInMaze == -1 ? line.IndexOf("F") : finishColumnIndexInMaze;
                    ColumnsCount = (ColumnsCount == 0) ? line.Length : ColumnsCount;
                    RowsCount++;
                    list.Add(line.ToCharArray());
                    Console.WriteLine("\t" + line);
                }
                file.Close();
            }
            catch (IOException)
            {
                Console.WriteLine("Unable to read File. Please make sure file is available at the specified location and try again.");
            }
            catch (Exception)
            {
                Console.WriteLine("Unhandled exception occured. Please try again.");
            }
            return list;
        }

        bool[,] isNodeVisited = new bool[RowsCount, ColumnsCount]; // Keeps track of the nodes that are visited previously while recursively checking the nodes
        bool[,] mazePath = new bool[RowsCount, ColumnsCount]; // Array to trace the actual maze path from S to F

        static void MazeSolver(List<char[]> mazeArray)
        {
            MazeRunner objMaze = new MazeRunner();
            objMaze.RecursiveSolver(mazeArray, startRowIndexInMaze, startColumnIndexInMaze);
            Console.WriteLine("Correct Path for the given Maze is shown below ('*' indicates the path traversed) \n");
            for (int i = 0; i < objMaze.mazePath.GetLength(0); i++)
            {
                Console.Write("\t");
                for (int j = 0; j < objMaze.mazePath.GetLength(1); j++)
                {
                    bool isInPath = objMaze.mazePath[i, j];
                    if (isInPath)
                    {
                        if (i == startRowIndexInMaze && j == startColumnIndexInMaze)
                            Console.Write("S"); //To indicate the start position on the maze solution
                        else if (i == finishRowIndexInMaze && j == finishColumnIndexInMaze)
                            Console.Write("F"); // Indicate the Finish position in the solution
                        else
                            Console.Write("*"); // Path traversed to reach from S to F
                    }
                    else
                        Console.Write("-"); // Found wall
                }
                Console.WriteLine(); // Linebreak after the current row
            }
        }

        /// <summary>
        /// Used to traverse throught the maze source List to determine the path recursively
        /// </summary>
        /// <param name="maze">List of Char[] containing the maze source</param>
        /// <param name="x">Current X (Row) position of the Node</param>
        /// <param name="y">Current Y (Column) position of the Node</param>
        /// <returns>True if is node is in the path</returns>
        public bool RecursiveSolver(List<char[]> maze, int x, int y)
        {
            if (maze[x][y] == 'F')
            {
                // F indicates Finish
                mazePath[x, y] = true; //Maze path found 
                return true;
            }
            if (maze[x][y] == '1' || isNodeVisited[x, y])
                return false; // Found wall or a node that has been visiteed previously 
            isNodeVisited[x, y] = true;
            if (x != 0)
            {
                // Checking if the node is not on top edge to handle indexoutofrange exception
                if (RecursiveSolver(maze, x - 1, y))
                {
                    mazePath[x, y] = true; // Sets that path value to true;
                    return true;
                }
            }
            if (x != RowsCount - 1)
            {
                // Checksing if the node is not on bottom edge
                if (RecursiveSolver(maze, x + 1, y))
                {
                    mazePath[x, y] = true;
                    return true;
                }
            }
            if (y != 0)
            {
                // Checking if the node is not on left edge
                if (RecursiveSolver(maze, x, y - 1))
                {
                    mazePath[x, y] = true;
                    return true;
                }
            }
            if (y != ColumnsCount - 1)
            {
                // Checking if the node is not on right edge
                if (RecursiveSolver(maze, x, y + 1))
                {
                    mazePath[x, y] = true;
                    return true;
                }
            }
            return false; //No condition is matching
        }

        #endregion

        public static void Main()
        {
            Console.WriteLine("Maze path generated below is based on the text file loaded into the application folder.");
            List<char[]> mazeArray = LoadMaze();
            if (mazeArray != null)
                MazeSolver(mazeArray);
            Console.ReadLine();
        }
    }
}
