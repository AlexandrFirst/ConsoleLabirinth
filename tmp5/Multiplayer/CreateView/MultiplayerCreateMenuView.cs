using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    class MultiplayerCreateMenuView : MenuView
    {
        string title = @"
                ██████╗██████╗ ███████╗ █████╗ ████████╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗
               ██╔════╝██╔══██╗██╔════╝██╔══██╗╚══██╔══╝██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝
               ██║     ██████╔╝█████╗  ███████║   ██║   █████╗      ██║  ███╗███████║██╔████╔██║█████╗  
               ██║     ██╔══██╗██╔══╝  ██╔══██║   ██║   ██╔══╝      ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  
               ╚██████╗██║  ██║███████╗██║  ██║   ██║   ███████╗    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗
                ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝";

        public int _count;

        public MultiplayerCreateMenuView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
        {
            controller = new MenuLevelController(options);
            (controller as MenuLevelController).ShowSelectedOpt = ShowOptionSelected;
            (controller as MenuLevelController).DeSelectedOpt = ShowOptionDeSelected;
            (controller as MenuLevelController).DrawSelectedOption = DrawUI;
        }

        public override void DisplayOptions(int startHighLight = 0,int count = -1)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Gray;

            _count = 4;
            if(MultiplayerConfig.ValidateData())
                _count = 5;
            base.DisplayOptions(startHighLight,_count);
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
                Console.Write("Width(10-20): ");

                string s_width = Console.ReadLine();

                if (!string.IsNullOrEmpty(s_width))
                {
                    MultiplayerConfig.Width = int.Parse(s_width);
                    controller.options[0] = "Width(10-20): " + MultiplayerConfig.Width;
                }
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
                Console.Write("Height(10-20): ");

                string s_height = Console.ReadLine();

                if (!string.IsNullOrEmpty(s_height))
                {
                    MultiplayerConfig.Height = int.Parse(s_height);
                    controller.options[1] = "Height(10-20): " + MultiplayerConfig.Height;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                DisplayOptions(1);
            }
            else if (selectedOption == 2)
            {
                for (int i = 0; i < _count; i++)
                {
                    if (i == 2)
                        continue;
                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                    Console.Write(controller.options[i]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + 2);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Name: ");

                string name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name))
                {
                    MultiplayerConfig.Name = name;
                    controller.options[2] = "Name: " + MultiplayerConfig.Name;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                DisplayOptions(2);
            }
            else if (selectedOption == 3)
            {
                view_pages.Pop().DisplayOptions();
                
            }

            //чтобы начать игру необходимо просто выйти из функции(теперь надо переделать GameManager)
        }
        public override void DisplaySideBar(int left_offset)
        {
            base.DisplaySideBar(left_offset);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(left_offset, height_offset + 5);
            Console.Write("Press enter once to set the value");
            Console.SetCursorPosition(left_offset, height_offset + 6);
            Console.Write("Press enter once to fix the value");
            Console.ForegroundColor = ConsoleColor.Gray;
        }


    }
}
