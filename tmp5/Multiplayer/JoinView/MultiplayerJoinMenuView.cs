using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tmp5
{
    class MultiplayerJoinMenuView : MenuView
    {
        string title = @"
                                    ██╗ ██████╗ ██╗███╗   ██╗     ██████╗  █████╗ ███╗   ███╗███████╗
                                    ██║██╔═══██╗██║████╗  ██║    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝
                                    ██║██║   ██║██║██╔██╗ ██║    ██║  ███╗███████║██╔████╔██║█████╗  
                               ██   ██║██║   ██║██║██║╚██╗██║    ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  
                               ╚█████╔╝╚██████╔╝██║██║ ╚████║    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗
                                ╚════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝";

        int _count;

        public MultiplayerJoinMenuView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
        {
            controller = new MenuLevelController(options);
            (controller as MenuLevelController).ShowSelectedOpt = ShowOptionSelected;
            (controller as MenuLevelController).DeSelectedOpt = ShowOptionDeSelected;
            (controller as MenuLevelController).DrawSelectedOption = DrawUI;
        }

        public override void DisplayOptions(int startHighLight = 0, int count = -1)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Gray;

            _count = 3;
            if (MultiplayerConfig.ValidateData())
                _count = 4;
            base.DisplayOptions(startHighLight, _count);
        }

        public override void DrawUI(int selectedOption)
        {

            DisplaySideBar(Console.WindowWidth / 2 + 20);

            if (selectedOption == 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    if (i == 0)
                        continue;
                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                    Console.Write(controller.options[i]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Name: ");

                string name = Console.ReadLine();

                MultiplayerConfig.Name = name;
                controller.options[0] = "Name: " + MultiplayerConfig.Name;

                Console.BackgroundColor = ConsoleColor.Black;

                DisplayOptions(0);
            }
            else if (selectedOption == 1)
            {
                for (int i = 0; i < _count; i++)
                {
                    if (i == 1)
                        continue;
                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                    Console.Write(controller.options[i]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + 1);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("IP: ");

                string ip = Console.ReadLine();


                MultiplayerConfig.IP = ip;
                controller.options[1] = "IP: " + MultiplayerConfig.IP;


                Console.BackgroundColor = ConsoleColor.Black;


                DisplayOptions(1);
            }
            else if (selectedOption == 2)
            {
                view_pages.Pop().DisplayOptions();
            }

            //чтобы начать игру необходимо просто выйти из функции(теперь надо переделать GameManager)
        }



    }
}
