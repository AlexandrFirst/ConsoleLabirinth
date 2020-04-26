using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    [Serializable]
    public class Point
    {
        public Point(char symbol, int x, int y)
        {
            this.symbol = symbol;
            up = down = left = right = false;
            this.x = x;
            this.y = y;
            door_key = ' ';
        }

        public int x;
        public int y;
        public char door_key;
        public char symbol;
        public bool up, down, left, right;
    }

    [Serializable]
    public class Labr
    {
        public List<List<Point>> maze { get; set; }
        public int start_x { get; set; }
        public int start_y { get; set; }
    }
}
