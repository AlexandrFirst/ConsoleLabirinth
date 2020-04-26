using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze;
namespace MultiplayerThread
{
    class ClientLabirinth
    {
        public void DisplayMaze(List<List<Point>> maze, int real_size_x, int real_size_y)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            Console.WriteLine(new string('-', real_size_x));

            foreach (List<Point> row in maze)
            {
                Console.Write('|');
                foreach (Point elem in row)
                {
                    if (elem.door_key != ' ')
                    {
                        switch (elem.door_key)
                        {
                            case 'a':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 'A':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 'b':
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 'B':
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 'c':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 'C':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }

                        Console.Write(elem.door_key);

                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                        Console.Write(elem.symbol);
                }
                Console.Write('|');
                Console.WriteLine();
            }

            Console.WriteLine(new string('-', real_size_x));
        }
    }
}
