using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    public abstract class MenuController
    {
        public List<string> options { get; set; }

        public Action<int> ShowSelectedOpt { get; set; }
        public Action<int> DeSelectedOpt { get; set; }
        public Action<int> DrawSelectedOption { get; set; }

        protected int displayedCount = 0;

        public MenuController(List<string> options)
        {
            this.options = options;
        }

        public virtual void SelectProcess(int startPos = 0, int _displayedCount = -1)
        {

            displayedCount = options.Capacity;
            if (_displayedCount > -1)
                displayedCount = _displayedCount;
            

            ConsoleKeyInfo k;

            int selectedItem = startPos;

            while (true)
            {
                k = Console.ReadKey(true);
                int prev_item = selectedItem;
                if (k.Key == ConsoleKey.DownArrow)
                {
                    selectedItem++;
                    if (selectedItem >= displayedCount)
                    {
                        selectedItem = 0;
                    }
                }
                else if (k.Key == ConsoleKey.UpArrow)
                {
                    selectedItem--;
                    if (selectedItem < 0)
                    {
                        selectedItem = displayedCount - 1;
                    }

                }
                else if (k.Key == ConsoleKey.Enter)
                {
                    DrawSelectedOption.Invoke(selectedItem);
                    break;
                }
                else if (k.Key == ConsoleKey.U)
                {
                    MusicManager.ChangeVolume(true);
                }
                else if (k.Key == ConsoleKey.D)
                {
                    MusicManager.ChangeVolume(false);
                }

                DeSelectedOpt.Invoke(prev_item);
                ShowSelectedOpt.Invoke(selectedItem);
            }

        }
    }
}
