using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatisticsManager;
namespace tmp5
{
    class StatisticMenuView : MenuView
    {

        string title = @"
                            ███████╗████████╗ █████╗ ████████╗██╗███████╗████████╗██╗ ██████╗███████╗
                            ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██║██╔════╝╚══██╔══╝██║██╔════╝██╔════╝
                            ███████╗   ██║   ███████║   ██║   ██║███████╗   ██║   ██║██║     ███████╗
                            ╚════██║   ██║   ██╔══██║   ██║   ██║╚════██║   ██║   ██║██║     ╚════██║
                            ███████║   ██║   ██║  ██║   ██║   ██║███████║   ██║   ██║╚██████╗███████║
                            ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚═╝╚══════╝   ╚═╝   ╚═╝ ╚═════╝╚══════╝";

        int min_page = 1;
        int max_page = 10;
        int current_page = 0;
        int items_per_page = 10;
        List<string> content = new List<string>();
        public StatisticMenuView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
        {

            current_page = 1;
            controller = new StatisticMenuController(options);
            (controller as StatisticMenuController).ShowSelectedOpt = ShowHorizontalOptionSelected;
            (controller as StatisticMenuController).DeSelectedOpt = ShowHorizontalOptionDeSelected;
            (controller as StatisticMenuController).DrawSelectedOption = DrawUI;
        }

        public override void DisplayOptions(int startHighLight = 0, int count = -1)
        {
           ShowPageContent(startHighLight);
            controller.SelectProcess(startHighLight, controller.options.Count);

        }


        void ShowPageContent(int selected_option)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Gray;



            int displayedOptions = controller.options.Count;

            for (int i = 0; i < displayedOptions; i++)
            {
                if (i == selected_option)
                    Console.BackgroundColor = ConsoleColor.Blue;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition((Console.WindowWidth / 2) - 10 + 10 * i, height_offset);
                Console.Write(controller.options[i]);
            }




            Console.BackgroundColor = ConsoleColor.Black;

            content = StatisticHelper.ReadFromFile();
            if (content == null)
            {
                max_page = current_page;
                Console.SetCursorPosition(0, 15);
                Console.WriteLine("There are no played games");
            }
            else
            {
                max_page = content.Count / items_per_page;
                int temp = content.Count % items_per_page;
                if (temp != 0)
                    max_page++;

               

                Console.SetCursorPosition(0, 12);
                Console.Write("Page: " + current_page + "/" + max_page);
                Console.SetCursorPosition(0, 15);

                int up_bound = current_page * items_per_page;
                if (up_bound >= content.Count)
                {
                    up_bound = content.Count;
                }

                for (int i = (current_page - 1) * items_per_page; i < up_bound; i++)
                {
                    Console.WriteLine(content[i]);
                }
            }
            //Console.Write(current_page);
        }

        public override void DrawUI(int selectedOption)
        {
            

            if (selectedOption == 0)
            {
                view_pages.Pop().DisplayOptions();

            }
            else if (selectedOption == 1)
            {
                if (current_page == 1)
                {
                    current_page = max_page;
                }
                else
                    current_page--;

                ShowPageContent(selectedOption);

                controller.SelectProcess(selectedOption, 3);
            }
            else if (selectedOption == 2)
            {
                if (current_page == max_page)
                {
                    current_page = min_page;
                }
                else
                    current_page++;

                ShowPageContent(selectedOption);

                controller.SelectProcess(selectedOption, 3);
            }

        }

        void ShowHorizontalOptionDeSelected(int selectedOption)
        {
            Console.SetCursorPosition((Console.WindowWidth / 2) - 10 + 10 * selectedOption, height_offset);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(controller.options[selectedOption]);
        }

        void ShowHorizontalOptionSelected(int selectedOption)
        {
            Console.SetCursorPosition((Console.WindowWidth / 2) - 10 + 10 * selectedOption, height_offset);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(controller.options[selectedOption]);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}

