using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsMazeGame
{
    public class ButtonTarget
    {
        public int sourceX;
        public int sourceY;
        public int targetX;
        public int targetY;

        public ButtonTarget(int sourceX, int sourceY, int targetX, int targetY)
        {
            this.sourceX = sourceX;
            this.sourceY = sourceY;
            this.targetX = targetX;
            this.targetY = targetY;
        }

        public bool isSource(int sourceX, int sourceY)
        {
            return this.sourceX == sourceX && this.sourceY == sourceY;
        }
    }
}
