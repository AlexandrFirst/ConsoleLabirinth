using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    class PlayerManager
    {
        int palyer_current_x; //set off position of the eplayer due to the border
        int palyer_current_y;

        int real_size_y;
        int real_size_x;

        int start_x;//the actual position of the playe in the fireld
        int start_y;

        List<char> keys;
        List<List<Point>> maze;

        public int count_doors_open { get; private set; }
        int Initial_door_count;

        ControlManager controls;


        public PlayerManager(SinglePlayerGameManager g_manager)
        {
            keys = new List<char>();
            this.maze = g_manager.maze;

            real_size_x = maze[0].Count;
            real_size_y = maze.Count;

            this.start_x = g_manager.start_x;
            this.start_y = g_manager.start_y;

            palyer_current_x = this.start_x + 1;
            palyer_current_y = this.start_y + 1;
            Initial_door_count = g_manager.Initial_door_count;
            count_doors_open = 0;
            controls = new ControlManager(palyer_current_x, palyer_current_y, moveUp, moveDown, moveLeft, moveRight,CheckAndPick_Key);

            controls.DisplaySideBar = g_manager.UpdateSidePlayBar;//єто нужно везде

            if (controls.PlayerMove())
            {
                ResultConfig.Status = 1;
            }
            else
            {
                ResultConfig.Status = 0;
            }
            g_manager.timerThread.Abort();
            g_manager.menuManager.DisplayEndResult();
            
        }

        bool moveUp()
        {
            if (start_y - 1 >= 0 && !(maze[start_y - 1][start_x].symbol == '#' || maze[start_y - 1][start_x].symbol == '|'))
            {
                if (maze[start_y - 1][start_x].door_key != ' ' && char.IsUpper(maze[start_y - 1][start_x].door_key))
                {
                    if (keys.Contains(char.ToLower(maze[start_y - 1][start_x].door_key)))
                    {
                        palyer_current_y--;
                        start_y--;
                        count_doors_open++;

                        Point temp = maze[start_y][start_x];
                        temp.door_key = ' ';
                        maze[start_y][start_x] = temp;
                        return true;
                    }
                }
                else if (maze[start_y - 1][start_x].door_key == ' ' || char.IsLower(maze[start_y - 1][start_x].door_key))
                {
                    palyer_current_y--;
                    start_y--;
                    return true;
                }
                
            }
            return false;
        }
        bool moveDown()
        {
            if (start_y + 1 < real_size_y && !(maze[start_y + 1][start_x].symbol == '#' || maze[start_y + 1][start_x].symbol == '|'))
            {
                if (maze[start_y + 1][start_x].door_key != ' ' && char.IsUpper(maze[start_y + 1][start_x].door_key))
                {
                    if (keys.Contains(char.ToLower(maze[start_y + 1][start_x].door_key)))
                    {
                        count_doors_open++;
                        palyer_current_y++;
                        start_y++;

                        Point temp = maze[start_y][start_x];
                        temp.door_key = ' ';
                        maze[start_y][start_x] = temp;
                        return true;
                    }
                }
                else if (maze[start_y + 1][start_x].door_key == ' ' || char.IsLower(maze[start_y + 1][start_x].door_key))
                {
                    palyer_current_y++;
                    start_y++;
                    return true;
                }

                
            }
            return false;
        }
        bool moveLeft()
        {
            if (start_x - 1 >= 0 && !(maze[start_y][start_x - 1].symbol == '#' || maze[start_y][start_x - 1].symbol == '|'))
            {
                if (maze[start_y][start_x - 1].door_key != ' ' && char.IsUpper(maze[start_y][start_x - 1].door_key))
                {
                    if (keys.Contains(char.ToLower(maze[start_y][start_x - 1].door_key)))
                    {
                        count_doors_open++;
                        palyer_current_x--;
                        start_x--;

                        Point temp = maze[start_y][start_x];
                        temp.door_key = ' ';
                        maze[start_y][start_x] = temp;

                        return true;
                    }
                }
                else if (maze[start_y][start_x - 1].door_key == ' ' || char.IsLower(maze[start_y][start_x - 1].door_key))
                {
                    palyer_current_x--;
                    start_x--;
                    return true;
                }
                

            }
            return false;
        }
        bool moveRight()
        {
            if (start_x + 1 < real_size_x && !(maze[start_y][start_x + 1].symbol == '#' || maze[start_y][start_x + 1].symbol == '|'))
            {
                if (maze[start_y][start_x + 1].door_key != ' ' && char.IsUpper(maze[start_y][start_x + 1].door_key))
                {
                    if (keys.Contains(char.ToLower(maze[start_y][start_x + 1].door_key)))
                    {
                        count_doors_open++;
                        palyer_current_x++;
                        start_x++;

                        Point temp = maze[start_y][start_x];
                        temp.door_key = ' ';
                        maze[start_y][start_x] = temp;
                        return true;
                    }
                }
                else if (maze[start_y][start_x + 1].door_key == ' ' || char.IsLower(maze[start_y][start_x + 1].door_key))
                {
                    palyer_current_x++;
                    start_x++;
                    return true;
                }

            }
            return false;
        }
        bool CheckAndPick_Key()
        {
            if (maze[start_y][start_x].door_key != ' ')
            {
                if (char.IsLower(maze[start_y][start_x].door_key))
                {
                    keys.Add(maze[start_y][start_x].door_key);
                }
            }

            if (count_doors_open < Initial_door_count)
            {
                return true;
            }
            else
                return false;
        }

    }
}
