using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class BallStartWall : Wall
    {
        public BallStartWall(int x, int y, int width, int height, int col, int row, Maze maze) 
		   : base(x, y, width, height, col, row, maze)
        {    
            wallType = 5;
	    }
	
	    public override void draw(SpriteBatch spriteBatch)
	    {
		    // draw nothing
	    }
	
	    public override bool collidingCircle(Ball b)
	    {
		    return false;
	    }
    }
}
