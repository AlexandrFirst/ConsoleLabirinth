using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    public abstract class MenuView
    {

        MenuView[] views;
        protected MenuController controller;
        protected int height_offset;

        public static Stack<MenuView> view_pages = new Stack<MenuView>();

        public MenuView(int height_offset,List<string>options, params MenuView[] views)
        {
            MusicManager.PlayMenuMusic();
           
            this.height_offset = height_offset;
            this.views = views;
            
        }

        public virtual void DisplayOptions(int startHighLight = 0,int count = -1)
        {
            int displayedOptions = controller.options.Count;
            if (count > -1)
                displayedOptions = count;

            DisplaySideBar(Console.WindowWidth / 2 + 20);
            for (int i = 0; i < displayedOptions; i++)
            {
                if (i == startHighLight)
                    Console.BackgroundColor = ConsoleColor.Blue;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + i);
                Console.Write(controller.options[i]);
            }
            controller.SelectProcess(startHighLight, displayedOptions);
        }

        public virtual void DrawUI(int selectedOption)
        {
            view_pages.Push(this);
            views[selectedOption].DisplayOptions();
        }

        protected void ShowOptionDeSelected(int selectedOption)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + selectedOption);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(controller.options[selectedOption]);
        }

        protected void ShowOptionSelected(int selectedOption)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, height_offset + selectedOption);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(controller.options[selectedOption]);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        

        public virtual void DisplaySideBar(int left_offset)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(left_offset, height_offset);
            Console.Write("Select menu option pressing:");

            Console.SetCursorPosition(left_offset, height_offset+1);
            Console.Write("  Arrow Up and Arrow Down");

            Console.SetCursorPosition(left_offset, height_offset+2);
            Console.Write("Change music volume");
            Console.SetCursorPosition(left_offset, height_offset + 3);
            Console.Write("    higher - press U");

            Console.SetCursorPosition(left_offset, height_offset + 4);
            Console.Write("    lower - press D");
            Console.ForegroundColor = ConsoleColor.Gray;

        }
    }
}
