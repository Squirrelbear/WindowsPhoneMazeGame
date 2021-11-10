using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsMazeGame
{
    public class NotifyLink
    {
        public int x, y, id;

        public NotifyLink(int x, int y, int id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }

        public bool isLocation(int x, int y)
        {
            return this.x == x && this.y == y;
        }
    }
}
