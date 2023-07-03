using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    public static class FieldConfig
    {
        static int width;
        public static int Width
        {
            get
            {
                if (width == 0)
                    width = 10;
                return width;
            }
            set
            {
                width = value;

                if (value < 5)
                    width = 5;
                if (value > 20)
                    width = 20;
            }
        }

        public static int height;
        public static int Height
        {
            get
            {
                if (height == 0)
                    height = 10;
                return height;
            }
            set
            {
                height = value;

                if (value < 5)
                    height = 5;
                if (value > 20)
                    height = 20;
            }
        }

        public static string Name { get; set; }
    }
}
