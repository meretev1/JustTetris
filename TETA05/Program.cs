using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TETA05
{
    class Program
    {
        public static char[,] matrix;

        public static int x1;
        public static int x2;
        public static int x3;
        public static int x4;
        public static int y1;
        public static int y2;
        public static int y3;
        public static int y4;
        public static int height;
        public static int width;

        public static int counter;
        public static int level;
        public static int points;
        public static int speed;
        public static int constSpeed;
        public static string figureString1;
        public static string figureString2;
        public static Queue<int> figureQueue = new Queue<int>();
        public static int figureNumber;
        public static int rotationNumber;

        static void Main()
        {
            Console.WriteLine("Give the arrea:");

            while (height < 22 || height > 42)
            {
                Console.Write("Height (20 - 40): ");
                height = int.Parse(Console.ReadLine());
                height += 2;
            }

            while (width < 10 || width > 30)
            {
                Console.Write("Width (10 - 30): ");
                width = int.Parse(Console.ReadLine());
            }

            Console.WindowHeight = height + 8;
            Console.Clear();

            matrix = new char[height, width];
            speed = 650;
            constSpeed = 650;
            level = 1;

            ClearMatrix();

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;
                Console.WriteLine();
                Console.WriteLine("  Left arrow - Move left ; Right arrow - Move right");
                Console.WriteLine();
                Console.WriteLine("  Spacebar - Rotate ; Down arrow - Fast down ; ESC - Exit");
                Console.WriteLine();
                Console.WriteLine("  Press any key to continue ...");

                Thread.Sleep(speed);

                if (Console.KeyAvailable)
                {
                    Console.Clear();
                    break;
                }
            }

            Random rnd = new Random();
            int figureNumberRandom = rnd.Next(1, 4);

            for (int i = 0; i < 2; i++)
            {
                figureQueue.Enqueue(figureNumberRandom);
                figureNumberRandom = rnd.Next(1, 4);
            }

            counter = 0;
            int outNumber = 0;

            while (CheckUp())
            {
                if (outNumber > 0)
                {
                    break;
                }

                if (level == 0)
                {
                    Console.WriteLine("Success!");
                    break;
                }

                figureNumber = figureQueue.Dequeue();
                int figNumNext = figureQueue.Peek();

                PrintNext(figNumNext);
                figureNumberRandom = rnd.Next(1, 4);
                figureQueue.Enqueue(figureNumberRandom);

                if (figureNumber == 1)
                {
                    Fig1();
                }
                else if (figureNumber == 2)
                {
                    Fig2();
                }
                else if (figureNumber == 3)
                {
                    Fig3();
                }

                rotationNumber = 0;

                while (true)
                {
                    Thread.Sleep(speed);

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey().Key;

                        if (key == ConsoleKey.Escape)
                        {
                            outNumber++;
                            break;
                        }

                        if (key == ConsoleKey.LeftArrow)
                        {
                            MoveLeft();
                        }

                        if (key == ConsoleKey.RightArrow)
                        {
                            MoveRight();
                        }

                        if (key == ConsoleKey.DownArrow)
                        {
                            speed = 0;
                        }

                        if (key == ConsoleKey.Spacebar)
                        {
                            Rotate();
                        }
                    }

                    if (Check())
                    {
                        ClearFigure();
                        Down();
                    }
                    else
                    {
                        RemoveLines();
                        speed = constSpeed;
                        break;
                    }

                    counter++;

                }
            }

            Console.WriteLine("Game over!");

            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, height + 2);
                Console.WriteLine("Press any key to close ...");
                Thread.Sleep(speed);
            }
        }

        private static void PrintNext(int figNumNext)
        {
            switch (figNumNext)
            {
                case 1:
                    figureString1 = "      ";
                    figureString2 = "  ****";
                    break;
                case 2:
                    figureString1 = "    * ";
                    figureString2 = "  *** ";
                    break;
                case 3:
                    figureString1 = "   ** ";
                    figureString2 = "  **  ";
                    break;
            }
        }

        private static bool CheckUp()
        {
            if (matrix[2, width / 2 - 2] == ' ' && matrix[2, width / 2 - 1] == ' ')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void RemoveLines()
        {
            int fullNumber = 0;

            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (matrix[i, j] != '*')
                    {
                        fullNumber++;
                    }
                }

                if (fullNumber == 0)
                {
                    for (int k = i; k > 0; k--)
                    {
                        for (int l = 0; l < width; l++)
                        {
                            matrix[k, l] = matrix[k - 1, l];
                        }
                    }

                    points += 10;
                    Print();
                    i++;

                    if (points >= 100)
                    {
                        points = 0;
                        counter = 0; ;
                        ClearMatrix();

                        level++;

                        if (level == 5)
                        {
                            level = 0;
                        }

                        Print();

                        speed -= 50;
                        constSpeed -= 50;
                    }
                }

                fullNumber = 0;
            }
        }

        private static void ClearMatrix()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = ' ';
                }
            }
        }

        private static void ClearFigure()
        {
            matrix[x1, y1] = ' ';
            matrix[x2, y2] = ' ';
            matrix[x3, y3] = ' ';
            matrix[x4, y4] = ' ';
        }

        private static void Fig1()
        {
            x1 = 2;
            x2 = 2;
            x3 = 2;
            x4 = 2;
            y1 = width / 2 - 2;
            y2 = width / 2 - 1;
            y3 = width / 2;
            y4 = width / 2 + 1;

            matrix[x1, y1] = '*';
            matrix[x2, y2] = '*';
            matrix[x3, y3] = '*';
            matrix[x4, y4] = '*';
        }

        private static void Fig2()
        {
            x1 = 2;
            x2 = 2;
            x3 = 2;
            x4 = 1;
            y1 = width / 2 - 2;
            y2 = width / 2 - 1;
            y3 = width / 2;
            y4 = width / 2;

            matrix[x1, y1] = '*';
            matrix[x2, y2] = '*';
            matrix[x3, y3] = '*';
            matrix[x4, y4] = '*';
        }

        private static void Fig3()
        {
            x1 = 2;
            x2 = 2;
            x3 = 1;
            x4 = 1;
            y1 = width / 2 - 2;
            y2 = width / 2 - 1;
            y3 = width / 2 - 1;
            y4 = width / 2;

            matrix[x1, y1] = '*';
            matrix[x2, y2] = '*';
            matrix[x3, y3] = '*';
            matrix[x4, y4] = '*';
        }

        private static void MoveRight()
        {
            if (CheckRightSide() && CheckRightNeighbour())
            {
                ClearFigure();
                y1++;
                y2++;
                y3++;
                y4++;

                PrintFigure();
            }
        }

        private static bool CheckRightSide()
        {
            if (y1 + 1 < width && y2 + 1 < width
                && y3 + 1 < width && y4 + 1 < width)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckLeftSide()
        {
            if (y1 - 1 >= 0 && y2 - 1 >= 0
                && y3 - 1 >= 0 && y4 - 1 >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void MoveLeft()
        {
            if (CheckLeftSide() && CheckLeftNeighbour())
            {
                ClearFigure();
                y1--;
                y2--;
                y3--;
                y4--;

                PrintFigure();
            }
        }

        private static bool CheckLeftNeighbour()
        {
            if (figureNumber == 1 && rotationNumber % 2 != 0 && matrix[x1, y1 - 1] != '*' &&
                matrix[x2, y2 - 1] != '*' && matrix[x3, y3 - 1] != '*' && matrix[x4, y4 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 1 && rotationNumber % 2 == 0 && matrix[x1, y1 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && (rotationNumber == 0 || rotationNumber == 4)
                && matrix[x1, y1 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 1 &&
                matrix[x1, y1 - 1] != '*' && matrix[x2, y2 - 1] != '*' && matrix[x3, y3 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 2 &&
                matrix[x3, y3 - 1] != '*' && matrix[x4, y4 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 3 && matrix[x1, y1 - 1] != '*' &&
                matrix[x2, y2 - 1] != '*' && matrix[x4, y4 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 3 && rotationNumber % 2 == 0 && matrix[x1, y1 - 1] != '*' && matrix[x3, y3 - 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 3 && rotationNumber % 2 != 0 && matrix[x1, y1 - 1] != '*' &&
                matrix[x2, y2 - 1] != '*' && matrix[x4, y4 - 1] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckRightNeighbour()
        {
            if (figureNumber == 1 && rotationNumber % 2 != 0 && matrix[x1, y1 + 1] != '*' &&
                matrix[x2, y2 + 1] != '*' && matrix[x3, y3 + 1] != '*' && matrix[x4, y4 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 1 && rotationNumber % 2 == 0 && matrix[x4, y4 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && (rotationNumber == 0 || rotationNumber == 4)
                && matrix[x3, y3 + 1] != '*' && matrix[x4, y4 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 1 &&
                matrix[x1, y1 + 1] != '*' && matrix[x2, y2 + 1] != '*' && matrix[x4, y4 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 2 &&
                matrix[x1, y1 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 3 && matrix[x1, y1 + 1] != '*' &&
                matrix[x2, y2 + 1] != '*' && matrix[x3, y3 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 3 && rotationNumber % 2 == 0 && matrix[x2, y2 + 1] != '*' && matrix[x4, y4 + 1] != '*')
            {
                return true;
            }
            else if (figureNumber == 3 && rotationNumber % 2 != 0 && matrix[x1, y1 + 1] != '*' &&
                matrix[x3, y3 + 1] != '*' && matrix[x4, y4 + 1] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Down()
        {
            if (x1 < height - 1 && x2 < height - 1 && x3 < height - 1 && x4 < height - 1)
            {
                x1++;
                x2++;
                x3++;
                x4++;
            }

            PrintFigure();
        }

        private static void PrintFigure()
        {
            matrix[x1, y1] = '*';
            matrix[x2, y2] = '*';
            matrix[x3, y3] = '*';
            matrix[x4, y4] = '*';

            Print();
        }

        private static bool Check()
        {
            if (figureNumber == 1 && rotationNumber % 2 != 0 && BottomChecking() && CheckFig11())
            {
                return true;
            }
            else if (figureNumber == 1 && rotationNumber % 2 == 0 && BottomChecking() && CheckFig12())
            {
                return true;
            }
            else if (figureNumber == 2 && (rotationNumber == 0 || rotationNumber == 4)
                && BottomChecking() && CheckFig24())
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 1 && BottomChecking() && CheckFig21())
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 2 && BottomChecking() && CheckFig22())
            {
                return true;
            }
            else if (figureNumber == 2 && rotationNumber == 3 && BottomChecking() && CheckFig23())
            {
                return true;
            }
            else if (figureNumber == 3 && rotationNumber % 2 != 0 && BottomChecking() && CheckFig31())
            {
                return true;
            }
            else if (figureNumber == 3 && rotationNumber % 2 == 0 && BottomChecking() && CheckFig32())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig31()
        {
            if (matrix[x2 + 1, y2] != '*' && matrix[x4 + 1, y4] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig32()
        {
            if (matrix[x1 + 1, y1] != '*' && matrix[x2 + 1, y2] != '*' &&
                matrix[x4 + 1, y4] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig21()
        {
            if (matrix[x3 + 1, y3] != '*' && matrix[x4 + 1, y4] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig22()
        {
            if (matrix[x1 + 1, y1] != '*' && matrix[x2 + 1, y2] != '*' &&
                matrix[x4 + 1, y4] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig23()
        {
            if (matrix[x1 + 1, y1] != '*' && matrix[x4 + 1, y4] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig24()
        {
            if (matrix[x1 + 1, y1] != '*' && matrix[x2 + 1, y2] != '*' &&
                matrix[x3 + 1, y3] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig12()
        {
            if (matrix[x1 + 1, y1] != '*' && matrix[x2 + 1, y2] != '*' &&
                matrix[x3 + 1, y3] != '*' && matrix[x4 + 1, y4] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckFig11()
        {
            if (matrix[x1 + 1, y1] != '*')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool BottomChecking()
        {
            if (x1 + 1 < height && x2 + 1 < height && x3 + 1 < height && x4 + 1 < height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Rotate()
        {
            rotationNumber++;
            if (rotationNumber == 5)
            {
                rotationNumber = 1;
            }

            switch (figureNumber)
            {
                case 1:
                    if (rotationNumber % 2 != 0)
                    {
                        if (x2 + 1 < height)
                        {
                            Fig11();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    else
                    {
                        if (y2 - 1 >= 0 && y2 + 2 < width && matrix[x1, y1 - 1] != '*' && matrix[x2, y2 - 1] != '*'
                            && matrix[x2, y2 + 1] != '*' && matrix[x2, y2 + 2] != '*')
                        {
                            Fig12();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    break;
                case 2:
                    if (rotationNumber == 1)
                    {
                        if (x2 + 1 < height && matrix[x2 + 1, y2] != '*'
                            && matrix[x2 + 1, y2 + 1] != '*' && matrix[x3 + 1, y3] != '*')
                        {
                            Fig21();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    else if (rotationNumber == 2)
                    {
                        if (y2 - 1 >= 0 && y2 + 1 < width && matrix[x1, y1 + 1] != '*' && matrix[x2, y2 - 1] != '*'
                            && matrix[x2 + 1, y2 - 1] != '*' && matrix[x3, y3 - 1] != '*')
                        {
                            Fig22();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    else if (rotationNumber == 3)
                    {
                        if (y4 - 1 >= 0 && x2 + 1 < height && matrix[x1 + 1, y1] != '*' && matrix[x2 + 1, y2] != '*' 
                            && matrix[x4, y4 - 1] != '*')
                        {
                            Fig23();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    else if (rotationNumber == 4)
                    {
                        if (y2 - 1 >= 0 && y2 + 1 < width && matrix[x1, y1 - 1] != '*' && matrix[x2, y2 - 1] != '*'
                            && matrix[x2, y2 + 1] != '*' && matrix[x3, y3 + 1] != '*')
                        {
                            Fig24();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    break;
                case 3:
                    if (rotationNumber % 2 != 0)
                    {
                        if (x2 + 1 < height && matrix[x2 + 1, y2 + 1] != '*')
                        {
                            Fig31();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    else
                    {
                        if (y2 - 1 >= 0 && y2 + 1 < width && y4 + 1 < width && matrix[x2 - 1, y2 - 1] != '*'
                            && matrix[x2, y2 - 1] != '*' && matrix[x4, y4 + 1] != '*')
                        {
                            Fig32();
                        }
                        else
                        {
                            rotationNumber--;
                        }
                    }
                    break;
            }

        }

        private static void Fig12()
        {
            ClearFigure();

            x1 -= 1;
            x3 += 1;
            x4 += 2;
            y1 -= 1;
            y3 += 1;
            y4 += 2;

            PrintFigure();
        }

        private static void Fig11()
        {
            ClearFigure();

            x1 += 1;
            x3 -= 1;
            x4 -= 2;
            y1 += 1;
            y3 -= 1;
            y4 -= 2;

            PrintFigure();
        }

        private static void Fig21()
        {
            ClearFigure();

            x1 -= 1;
            x3 += 1;
            x4 += 2;
            y1 += 1;
            y3 -= 1;

            PrintFigure();
        }

        private static void Fig22()
        {
            ClearFigure();

            x1 += 1;
            x3 -= 1;
            y1 += 1;
            y3 -= 1;
            y4 -= 2;

            PrintFigure();
        }

        private static void Fig23()
        {
            ClearFigure();

            x1 += 1;
            x3 -= 1;
            x4 -= 2;
            y1 -= 1;
            y3 += 1;

            PrintFigure();
        }

        private static void Fig24()
        {
            ClearFigure();

            x1 -= 1;
            x3 += 1;
            y1 -= 1;
            y3 += 1;
            y4 += 2;

            PrintFigure();
        }

        private static void Fig31()
        {
            ClearFigure();

            x1 -= 1;
            x3 += 1;
            x4 += 2;
            y1 += 1;
            y3 += 1;

            PrintFigure();
        }

        private static void Fig32()
        {
            ClearFigure();

            x1 += 1;
            x3 -= 1;
            x4 -= 2;
            y1 -= 1;
            y3 -= 1;

            PrintFigure();
        }

        static void Print()
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

            for (int i = 0; i < height; i++)
            {
                var line = new StringBuilder();

                if (i < 2)
                {
                    line.Append(' ');
                }
                else
                {
                    line.Append('I');
                }

                for (int j = 0; j < width; j++)
                {
                    line.Append(matrix[i, j]);
                }

                if (i < 2)
                {
                    line.Append(' ');
                }
                else
                {
                    line.Append('I');
                }

                if (i == 2)
                {
                    if (counter < 10)
                    {
                        line.Append("  Score:    " + counter.ToString());
                    }
                    else if (counter >= 10 && counter < 100)
                    {
                        line.Append("  Score:   " + counter.ToString());
                    }
                    else if (counter >= 100 && counter < 1000)
                    {
                        line.Append("  Score:  " + counter.ToString());
                    }
                    else
                    {
                        line.Append("  Score: " + counter.ToString());
                    }
                }

                else if (i == 3)
                {
                    line.Append("  Level: " + level.ToString());
                }

                else if (i == 4)
                {
                    if (points < 10)
                    {
                        line.Append("  Points:   " + points.ToString());
                    }
                    else if (points >= 10 && points < 100)
                    {
                        line.Append("  Points:  " + points.ToString());
                    }
                    else
                    {
                        line.Append("  Points: " + points.ToString());
                    }
                }
                else if (i == 6)
                {
                    line.Append("  Next figure:");
                }
                else if (i == 8)
                {
                    line.Append(figureString1);
                }

                else if (i == 9)
                {
                    line.Append(figureString2);
                }

                Console.WriteLine(line.ToString());

            }

            StringBuilder lastLine = new StringBuilder();

            for (int i = 0; i < width + 2; i++)
            {
                lastLine.Append('I');
            }

            Console.WriteLine(lastLine);
        }
    }
}
