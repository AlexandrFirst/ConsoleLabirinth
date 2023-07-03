using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    public class MenuLevelView : MenuView
    {

        string title = @"
                                                     ██████╗██╗  ██╗ ██████╗  ██████╗ ███████╗███████╗        
                                                    ██╔════╝██║  ██║██╔═══██╗██╔═══██╗██╔════╝██╔════╝        
                                                    ██║     ███████║██║   ██║██║   ██║███████╗█████╗          
                                                    ██║     ██╔══██║██║   ██║██║   ██║╚════██║██╔══╝          
                                                    ╚██████╗██║  ██║╚██████╔╝╚██████╔╝███████║███████╗        
                                                     ╚═════╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝╚══════╝        
                                        ██████╗ ██╗███████╗███████╗██╗ ██████╗██╗   ██╗██╗  ████████╗██╗   ██╗
                                        ██╔══██╗██║██╔════╝██╔════╝██║██╔════╝██║   ██║██║  ╚══██╔══╝╚██╗ ██╔╝
                                        ██║  ██║██║█████╗  █████╗  ██║██║     ██║   ██║██║     ██║    ╚████╔╝ 
                                        ██║  ██║██║██╔══╝  ██╔══╝  ██║██║     ██║   ██║██║     ██║     ╚██╔╝  
                                        ██████╔╝██║██║     ██║     ██║╚██████╗╚██████╔╝███████╗██║      ██║   
                                        ╚═════╝ ╚═╝╚═╝     ╚═╝     ╚═╝ ╚═════╝ ╚═════╝ ╚══════╝╚═╝      ╚═╝   ";

        public MenuLevelView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
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
            
            //Console.Clear();
            switch (selectedOption)
            {
                case 0:
                    for (int i = 0; i < controller.options.Count; i++)
                    {
                        if (i == 0)
                            continue;
                        Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                        Console.Write(controller.options[i]);
                    }

                    Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + 0);
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("Name: ");

                    string name = Console.ReadLine();

                    if (!string.IsNullOrEmpty(name))
                    {
                        FieldConfig.Name = name;
                        controller.options[0] = "Name: " + FieldConfig.Name;
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    DisplayOptions(0);
                    break;
                case 1:
                    FieldConfig.Width = 10;
                    FieldConfig.Height = 10;
                    break;
                case 2:
                    FieldConfig.Width = 15;
                    FieldConfig.Height = 20;
                    break;
                case 3:
                    FieldConfig.Width = 20;
                    FieldConfig.Height = 20;
                    break;
                case 4:
                    base.DrawUI(selectedOption);
                    break;
                case 5:
                    view_pages.Pop().DisplayOptions();
                    break;
                default:
                    break;
            }
        }
    }
}
