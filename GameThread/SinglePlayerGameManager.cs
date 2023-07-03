using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tmp5
{
    public struct Point
    {
        public Point(char symbol, int x, int y)
        {
            this.symbol = symbol;
            up = down = left = right = false;
            this.x = x;
            this.y = y;
            door_key = ' ';
        }



        public int x;
        public int y;
        public char door_key;
        public char symbol;
        public bool up, down, left, right;
    }
    
    public class SinglePlayerGameManager
    {
        public MenuManager menuManager { get; private set; }


        Stack<char> doors_keys;
        int real_size_x;
        int real_size_y;

        public int Initial_door_count { get; private set; }


        public List<List<Point>> maze { get; private set; }
        public int start_x { get; private set; }
        public int start_y { get; private set; }

        public DateTime timer { get; private set; }
        public Thread timerThread;
        public int steps = 0;
        public SinglePlayerGameManager(MenuManager menuManager)
        {
            this.menuManager = menuManager;           

            maze = new List<List<Point>>();

            timer = new DateTime(0, 0);

            int size_x = FieldConfig.Width;
            int size_y = FieldConfig.Height;

            real_size_x = size_x * 2 - 1;
            real_size_y = size_y * 2 - 1;

            MusicManager.PlayGameMusic();

            FillArea();

            CreateMaze();
            DisplayMaze();

            StartTimer();

            PlayerManager player = new PlayerManager(this);
        }

        void UpdateTimer()
        {
            Thread.Sleep(100);
            while (true)
            {
                timer = timer.AddSeconds(1);
                Thread.Sleep(1000);
            }
        }

        void StartTimer()
        {
            timerThread = new Thread(UpdateTimer);
            timerThread.Start();
        }

        public void UpdateSidePlayBar(string steps)
        {
            this.steps = int.Parse(steps);
            menuManager.DisplayGameSideBar(real_size_x + 10, 4,timer,steps,real_size_y);
        }

        void FillArea()
        {
            for (int i = 0; i < real_size_y; i++)
            {
                List<Point> row = new List<Point>();
                for (int j = 0; j < real_size_x; j++)
                {
                    row.Add(new Point('#', j, i));
                }
                maze.Add(row);
            }

            doors_keys = new Stack<char>();

            //manage the number of keys(custom set)
            doors_keys.Push('a');
            doors_keys.Push('A');
            doors_keys.Push('b');
            doors_keys.Push('B');
            doors_keys.Push('c');
            doors_keys.Push('C');

            Initial_door_count = doors_keys.Count / 2;
        }


        void CreateMaze()
        {
            Random random = new Random();

            int current_x = random.Next(real_size_x);
            int current_y = random.Next(real_size_y);

            start_x = current_x;
            start_y = current_y;

            Stack<Point> visited = new Stack<Point>();
            Point current = maze[current_y][current_x];
            current.symbol = ' ';
            visited.Push(current);
            maze[current_y][current_x] = current;
            while (visited.Count > 0)
            {
                List<int> direction = new List<int>() { 0, 1, 2, 3 }.OrderBy(x => Guid.NewGuid()).ToList();
                bool b = false;

                current_x = visited.Peek().x;
                current_y = visited.Peek().y;

                foreach (int item in direction)
                {
                    if (item == 0)//down
                    {
                        if (current_y + 2 < real_size_y && maze[current_y + 2][current_x].symbol != ' ')
                        {
                            b = false;
                            Point temp = visited.Pop();
                            temp.down = true;
                            visited.Push(temp);
                            current = maze[current_y + 2][current_x];
                            current.symbol = ' ';
                            current.up = true;
                            maze[current_y + 2][current_x] = current;

                            temp = maze[current_y + 1][current_x];
                            temp.symbol = ' ';
                            maze[current_y + 1][current_x] = temp;

                            visited.Push(current);
                            break;
                        }
                        else
                        {
                            b = true;
                            continue;
                        }
                    }
                    else if (item == 1)//up
                    {
                        if (current_y - 2 >= 0 && maze[current_y - 2][current_x].symbol != ' ')
                        {
                            b = false;
                            Point temp = visited.Pop();
                            temp.up = true;
                            visited.Push(temp);
                            current = maze[current_y - 2][current_x];
                            current.symbol = ' ';
                            current.down = true;
                            maze[current_y - 2][current_x] = current;

                            temp = maze[current_y - 1][current_x];
                            temp.symbol = ' ';
                            maze[current_y - 1][current_x] = temp;


                            visited.Push(current);
                            break;
                        }
                        else
                        {
                            b = true;
                            continue;
                        }
                    }
                    else if (item == 2)//left
                    {
                        if (current_x - 2 >= 0 && maze[current_y][current_x - 2].symbol != ' ')
                        {
                            b = false;
                            Point temp = visited.Pop();
                            temp.left = true;
                            visited.Push(temp);
                            current = maze[current_y][current_x - 2];
                            current.symbol = ' ';
                            current.right = true;
                            maze[current_y][current_x - 2] = current;

                            temp = maze[current_y][current_x - 1];
                            temp.symbol = ' ';
                            maze[current_y][current_x - 1] = temp;

                            visited.Push(current);
                            break;
                        }
                        else
                        {
                            b = true;
                            continue;
                        }
                    }
                    else if (item == 3)//right
                    {
                        if (current_x + 2 < real_size_x && maze[current_y][current_x + 2].symbol != ' ')
                        {
                            b = false;
                            Point temp = visited.Pop();
                            temp.right = true;
                            visited.Push(temp);
                            current = maze[current_y][current_x + 2];
                            current.symbol = ' ';
                            current.left = true;
                            maze[current_y][current_x + 2] = current;

                            temp = maze[current_y][current_x + 1];
                            temp.symbol = ' ';
                            maze[current_y][current_x + 1] = temp;

                            visited.Push(current);
                            break;
                        }
                        else
                        {
                            b = true;
                            continue;
                        }
                    }
                }

                if (b)
                {
                    if ((current.left && !current.right && !current.up && !current.down) ||
                        //(!current.left && current.right && !current.up && !current.down) ||
                        (!current.left && !current.right && current.up && !current.down) ||
                        //(!current.left && !current.right && !current.up && current.down) ||
                        (current.left && !current.right && current.up && !current.down))
                    {
                        if (maze[current_y][current_x].door_key == ' ' && (current.y != current_y || current.x != current_x))
                        {
                            if (doors_keys.Count > 0)
                            {
                                Point temp = maze[current_y][current_x];
                                temp.door_key = doors_keys.Pop();
                                maze[current_y][current_x] = temp;
                                //Console.WriteLine(current_y + " " + current_x);

                            }
                        }
                    }
                    current = visited.Pop();
                }
            }
        }


        void DisplayMaze()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            while (doors_keys.Count > 0)
            {
                maze = new List<List<Point>>();
                FillArea();
                CreateMaze();
            }

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
