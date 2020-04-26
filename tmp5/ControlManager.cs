using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    class ControlManager
    {
        int palyer_current_x;
        int palyer_current_y;

        Func<bool> moveUp;
        Func<bool> moveDown;
        Func<bool> moveLeft;
        Func<bool> moveRight;
        Func<bool> CheckAndPick_Key;
        public Action<string> DisplaySideBar;

        int steps;

     
        public ControlManager(int player_x, int player_y,params Func<bool>[]actions)
        {
            palyer_current_x = player_x;
            palyer_current_y = player_y;

            moveUp = actions[0];
            moveDown = actions[1];
            moveLeft = actions[2];
            moveRight = actions[3];
            CheckAndPick_Key = actions[4];

            steps = 0;
           
        }

        ConsoleKeyInfo k;

        public bool PlayerMove()
        {

            while (true)
            {
                

                DisplaySideBar(steps.ToString());
                Console.SetCursorPosition(palyer_current_x, palyer_current_y);
                int temp_x = palyer_current_x;
                int temp_y = palyer_current_y;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write('x');
                Console.ForegroundColor = ConsoleColor.Gray;

                while (!Console.KeyAvailable)
                    DisplaySideBar(steps.ToString());

                k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.UpArrow)
                {
                    if (moveUp())
                    {
                        palyer_current_y--;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    if (moveDown())
                    {
                        palyer_current_y++;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.LeftArrow)
                {
                    if (moveLeft())
                    {
                        palyer_current_x--;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    if (moveRight())
                    {
                        palyer_current_x++;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.U)
                {
                    MusicManager.ChangeVolume(true);
                }
                else if (k.Key == ConsoleKey.D)
                {
                    MusicManager.ChangeVolume(false);
                }
                else if (k.Key == ConsoleKey.Q)
                    return false;

                if (!CheckAndPick_Key())
                    return true;

                Console.SetCursorPosition(temp_x, temp_y);
                Console.Write(' ');

            }
        }
       

    }
}
