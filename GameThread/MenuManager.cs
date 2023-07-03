using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tmp5
{
    public class MenuManager
    {

        string looseTitle = @"
                                        ██╗   ██╗ ██████╗ ██╗   ██╗    ██╗      ██████╗  ██████╗ ███████╗███████╗
                                        ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║     ██╔═══██╗██╔═══██╗██╔════╝██╔════╝
                                         ╚████╔╝ ██║   ██║██║   ██║    ██║     ██║   ██║██║   ██║███████╗█████╗  
                                          ╚██╔╝  ██║   ██║██║   ██║    ██║     ██║   ██║██║   ██║╚════██║██╔══╝  
                                           ██║   ╚██████╔╝╚██████╔╝    ███████╗╚██████╔╝╚██████╔╝███████║███████╗
                                           ╚═╝    ╚═════╝  ╚═════╝     ╚══════╝ ╚═════╝  ╚═════╝ ╚══════╝╚══════╝";
        string winTitle = @"
                                                ██╗   ██╗ ██████╗ ██╗   ██╗    ██╗    ██╗██╗███╗   ██╗
                                                ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║    ██║██║████╗  ██║
                                                 ╚████╔╝ ██║   ██║██║   ██║    ██║ █╗ ██║██║██╔██╗ ██║
                                                  ╚██╔╝  ██║   ██║██║   ██║    ██║███╗██║██║██║╚██╗██║
                                                   ██║   ╚██████╔╝╚██████╔╝    ╚███╔███╔╝██║██║ ╚████║
                                                   ╚═╝    ╚═════╝  ╚═════╝      ╚══╝╚══╝ ╚═╝╚═╝  ╚═══╝";

        MenuView view3;
        MenuView view2;
        MenuView view1;
        MenuView multiplayer;
        MenuView multiplayerCreate;
        MenuView multiplayerJoin;
        MenuView Statistics;
        public MenuManager()
        {
            view3 = new MenuLevelCustomView(3, new List<string> {"Name", "Width(5-20)", "Height(5-20)", "Apply", "Back" });
            view2 = new MenuLevelView(20, new List<string> { "Name", "Easy", "Middle", "Hard", "Custom","Back" }, null, null, null, null, view3);
            multiplayerJoin = new MultiplayerJoinMenuView(20, new List<string> { "Name", "Ip", "Exit", "Join" }, null, null, null);
            multiplayerCreate = new MultiplayerCreateMenuView(20, new List<string> { "Width(10-20)", "Height(10-20)", "Name", "Exit", "Create" }, null, null, null, null);
            multiplayer = new MultiplayerMenuView(20, new List<string> { "Create game", "Join game", "Exit" }, multiplayerCreate, multiplayerJoin,null);
            Statistics = new StatisticMenuView(10, new List<string> { "back", "previous", "next" });
            view1 = new StartMenuView(20, new List<string> { "Start","Multiplayer", "Statistic","Exit" }, view2,multiplayer,Statistics,null);
             
            
        }
        public void DisplayGameSideBar(int left_offset, int height_offset, DateTime time, string stepNumber, int FieldHeight)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(left_offset, height_offset);
            Console.Write("Music volume up: U");

            Console.SetCursorPosition(left_offset, height_offset + 1);
            Console.Write("Music volume down: D");

            Console.SetCursorPosition(left_offset, height_offset + 2);
            Console.Write("Quite game(you will lose): Q");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(left_offset, height_offset + 3);
            Console.Write("The number of steps made: " + stepNumber);

            int time_height = FieldHeight / 2;
            if (time_height <= height_offset + 3)
                time_height = height_offset + 4;
            Console.SetCursorPosition(left_offset, time_height);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Time spent: " + time.ToString("mm:ss"));

            Console.ForegroundColor = ConsoleColor.Gray;

            ResultConfig.NumberOfSteps = stepNumber;
            ResultConfig.Time = time.ToString("mm:ss");

        }


        public void DisplayMenu()
        {
            view1.DisplayOptions();
        }

        public void DisplayEndResult()
        {
            Console.Clear();
            if (ResultConfig.Status == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(winTitle);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(looseTitle);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ResultConfig.Time + " - you spend time");
            Console.WriteLine(ResultConfig.NumberOfSteps + " - you made steps");
            Console.WriteLine("Press any key to continue...");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey();
        }
    }
}
