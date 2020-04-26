using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerThread
{
    class ControlManager
    {
        public BinaryWriter writer { private get; set; }
        public BinaryReader reader { private get; set; }

        int playerPosX;
        int playerPosY;

        int enemyPosX;
        int enemyPosY;

        int enXt;
        int enYt;

        bool isLose = false;
        public bool isWin = false;

        ConsoleKeyInfo k;


        public bool realLoose = false;
        public Action DepictUnpickedKey { private get; set; }
        public Action DepictUnpickedDoor { private get; set; }
        public Action<string> UpdateSideBar { private get; set; }
        public Action<int,int,bool> SendMessage { private get; set; }
        public Action changeVolumeUp { private get; set; }
        public Action changeVolumeDown { private get; set; }
        public Func<bool> moveUp { private get; set; }
        public Func<bool> moveDown { private get; set; }
        public Func<bool> moveLeft { private get; set; }
        public Func<bool> moveRight { private get; set; }
        public Func<bool> CheckAndPick_Key { private get; set; }
        public Func<bool> getStatus { private get; set; }
        public Func<int> GetEnemyX { private get; set; }
        public Func<int> GetEnemyY { private get; set; }


        int steps = 0;

        public int PLayerAndEnemyMove(int PlayerPosX, int PlayerPosY)
        {
            playerPosX = PlayerPosX;
            playerPosY = PlayerPosY;
            enXt = enemyPosX;
            enYt = enemyPosY;

            while (true)
            {
                isLose = getStatus();

                if (isLose == true)
                    break;

                enemyPosX = GetEnemyX();
                enemyPosY = GetEnemyY();

                Console.SetCursorPosition(playerPosX, playerPosY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write('X');
                Console.ForegroundColor = ConsoleColor.Gray;
                int plXt = playerPosX;
                int plYt = playerPosY;

                while (!Console.KeyAvailable)
                {
                    isLose = getStatus();

                    if (isLose == true)
                        break;

                    enemyPosX = GetEnemyX();
                    enemyPosY = GetEnemyY();
                    UpdateSideBar(steps.ToString());
                    if (enemyPosX != enXt || enemyPosY != enYt)
                    {
                        Console.SetCursorPosition(enXt, enYt);
                        Console.Write(' ');

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(enemyPosX, enemyPosY);
                        Console.Write('0');
                        Console.ForegroundColor = ConsoleColor.Gray;
                        enXt = enemyPosX;
                        enYt = enemyPosY;

                       

                    }

                    DepictUnpickedKey();
                    DepictUnpickedDoor();
                }

                isLose = getStatus();

                if (isLose == true)
                    break;


                Console.SetCursorPosition(enXt, enYt);
                Console.Write(' ');

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(enemyPosX, enemyPosY);
                Console.Write('0');
                Console.ForegroundColor = ConsoleColor.Gray;
                enXt = enemyPosX;
                enYt = enemyPosY;

                DepictUnpickedKey();
                DepictUnpickedDoor();

                k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.UpArrow)
                {
                    if (moveUp())
                    {
                        playerPosY--;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    if (moveDown())
                    {
                        playerPosY++;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.LeftArrow)
                {
                    if (moveLeft())
                    {
                        playerPosX--;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    if (moveRight())
                    {
                        playerPosX++;
                        steps++;
                    }
                }
                else if (k.Key == ConsoleKey.U)
                {
                    changeVolumeUp();
                }
                else if (k.Key == ConsoleKey.D)
                {
                    changeVolumeDown();
                }
                else if (k.Key == ConsoleKey.Q)
                {
                    realLoose = true;
                    break;
                }
                if (!CheckAndPick_Key())
                {
                    isWin = true;

                }

                
                Console.SetCursorPosition(plXt, plYt);
                Console.Write(' ');
               
                SendMessage(playerPosX, playerPosY,isWin);

                if (isWin)
                    break;

                UpdateSideBar(steps.ToString());

            }

            return steps;
        }
    }
}
