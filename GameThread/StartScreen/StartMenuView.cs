using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    class StartMenuView:MenuView
    {

        string title = @"
                                        ██╗      █████╗ ██████╗ ██╗██████╗ ██╗███╗   ██╗████████╗██╗  ██╗                    
                                        ██║     ██╔══██╗██╔══██╗██║██╔══██╗██║████╗  ██║╚══██╔══╝██║  ██║                    
                                        ██║     ███████║██████╔╝██║██████╔╝██║██╔██╗ ██║   ██║   ███████║                    
                                        ██║     ██╔══██║██╔══██╗██║██╔══██╗██║██║╚██╗██║   ██║   ██╔══██║                    
                                        ███████╗██║  ██║██████╔╝██║██║  ██║██║██║ ╚████║   ██║   ██║  ██║                    
                                        ╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝                    
            ███████╗███╗   ██╗     ██╗ ██████╗ ██╗   ██╗    ████████╗██╗  ██╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗
            ██╔════╝████╗  ██║     ██║██╔═══██╗╚██╗ ██╔╝    ╚══██╔══╝██║  ██║██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝
            █████╗  ██╔██╗ ██║     ██║██║   ██║ ╚████╔╝        ██║   ███████║█████╗      ██║  ███╗███████║██╔████╔██║█████╗  
            ██╔══╝  ██║╚██╗██║██   ██║██║   ██║  ╚██╔╝         ██║   ██╔══██║██╔══╝      ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  
            ███████╗██║ ╚████║╚█████╔╝╚██████╔╝   ██║          ██║   ██║  ██║███████╗    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗
            ╚══════╝╚═╝  ╚═══╝ ╚════╝  ╚═════╝    ╚═╝          ╚═╝   ╚═╝  ╚═╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝";

        public StartMenuView(int height_offset, List<string> options, params MenuView[] views) : base(height_offset, options, views)
        {
         

            controller = new StartMenuController(options);
            (controller as StartMenuController).ShowSelectedOpt = ShowOptionSelected;
            (controller as StartMenuController).DeSelectedOpt = ShowOptionDeSelected;
            (controller as StartMenuController).DrawSelectedOption = DrawUI;    
        }

        public override void DisplayOptions(int startHighLight = 0,int count = -1)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Gray;
            base.DisplayOptions(startHighLight);
        }

        public override void DrawUI(int selectedOption)
        {
            
            if (selectedOption == 0)
                GameTypeConfig.isMuliplayer = false;
            else if (selectedOption == 1)
                GameTypeConfig.isMuliplayer = true;
            else if (selectedOption == 3)
                Environment.Exit(0);

            base.DrawUI(selectedOption);
        }
    }
}
