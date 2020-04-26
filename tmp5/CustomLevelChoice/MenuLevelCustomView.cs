using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    public class MenuLevelCustomView : MenuView
    {
       

        public MenuLevelCustomView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
        {
            controller = new MenuLevelCustomController(options);
            (controller as MenuLevelCustomController).ShowSelectedOpt = ShowOptionSelected;
            (controller as MenuLevelCustomController).DeSelectedOpt = ShowOptionDeSelected;
            (controller as MenuLevelCustomController).DrawSelectedOption = DrawUI;
        }

        public override void DisplayOptions(int startHighLight = 0, int count = -1)
        {
            Console.Clear();

            DisplaySideBar(Console.WindowWidth / 2 + 20);
            for (int i = 0; i < controller.options.Count; i++)
            {
                if (i == startHighLight)
                    Console.BackgroundColor = ConsoleColor.Blue;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                if (i == 0)
                {
                    if (!string.IsNullOrEmpty(FieldConfig.Name))
                    {
                        controller.options[i] = "Name: " + FieldConfig.Name;
                    }
                }
                Console.Write(controller.options[i]);
            }
            controller.SelectProcess(startHighLight, controller.options.Count);
        }

        public override void DrawUI(int selectedOption)
        {
            Console.Clear();
            DisplaySideBar(Console.WindowWidth/2+20);

            DisplaySideBar(Console.WindowWidth / 2 + 20);
            if (selectedOption == 0)
            {
                for (int i = 0; i < controller.options.Count; i++)
                {
                    if (i == 0)
                        continue;
                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                    Console.Write(controller.options[i]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Name: ");

                string Name = Console.ReadLine();

                if (!string.IsNullOrEmpty(Name))
                {
                    FieldConfig.Name = Name;
                    controller.options[0] = "Name: " + FieldConfig.Name;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                DisplayOptions(0);
            }
            else if (selectedOption == 1)
            {
                for (int i = 0; i < controller.options.Count; i++)
                {
                    if (i == 1)
                        continue;
                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                    Console.Write(controller.options[i]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset+1);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Width(5-20): ");

                string s_width = Console.ReadLine();

                if (!string.IsNullOrEmpty(s_width))
                {
                    FieldConfig.Width = int.Parse(s_width);
                    controller.options[1] = "Width(5-20): " + FieldConfig.Width;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                DisplayOptions(1);
            }
            else if (selectedOption == 2)
            {
                for (int i = 0; i < controller.options.Count; i++)
                {
                    if (i == 2)
                        continue;
                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                    Console.Write(controller.options[i]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + 2);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Height(5-20): ");

                string s_height = Console.ReadLine();

                if (!string.IsNullOrEmpty(s_height))
                {
                    FieldConfig.Height = int.Parse(s_height);
                    controller.options[1] = "Height(5-20): " + FieldConfig.Height;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                DisplayOptions(2);
            }
            else if (selectedOption ==3)
            {
                return;
            }
            if (selectedOption == 4)
            {
                view_pages.Pop().DisplayOptions();
            }
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
