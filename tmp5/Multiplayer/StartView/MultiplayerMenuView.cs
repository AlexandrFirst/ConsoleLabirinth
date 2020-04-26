using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    class MultiplayerMenuView : MenuView
    {

        string title = @"
                            ███╗   ███╗██╗   ██╗██╗  ████████╗██╗██████╗ ██╗      █████╗ ██╗   ██╗███████╗██████╗ 
                            ████╗ ████║██║   ██║██║  ╚══██╔══╝██║██╔══██╗██║     ██╔══██╗╚██╗ ██╔╝██╔════╝██╔══██╗
                            ██╔████╔██║██║   ██║██║     ██║   ██║██████╔╝██║     ███████║ ╚████╔╝ █████╗  ██████╔╝
                            ██║╚██╔╝██║██║   ██║██║     ██║   ██║██╔═══╝ ██║     ██╔══██║  ╚██╔╝  ██╔══╝  ██╔══██╗
                            ██║ ╚═╝ ██║╚██████╔╝███████╗██║   ██║██║     ███████╗██║  ██║   ██║   ███████╗██║  ██║
                            ╚═╝     ╚═╝ ╚═════╝ ╚══════╝╚═╝   ╚═╝╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝";

        public MultiplayerMenuView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
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
            base.DisplayOptions(startHighLight);
        }

        public override void DrawUI(int selectedOption)
        {

            Console.Clear();
            if (selectedOption == 2)
                view_pages.Pop().DisplayOptions();
            else if (selectedOption == 0)
                MultiplayerConfig.isHost = true;
            else if (selectedOption == 1)
                MultiplayerConfig.isHost = false;

            base.DrawUI(selectedOption);


        }
    }
}
