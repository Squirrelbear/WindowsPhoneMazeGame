using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class LevelEndWall : Wall
    {
        public LevelEndWall(int x, int y, int width, int height, int col, int row, Maze maze) 
		    : base(x, y, width, height, col, row, maze)
        {
		    wallType = 3;
		    texture = maze.getTexture(4);
	    }
	
	    public override void draw(SpriteBatch spriteBatch)
	    {
		    base.draw(spriteBatch);
		    /*
		    Paint paint = new Paint();
		    paint.setColor(Color.RED);
		    canvas.drawRect(rect, paint);
		    paint.setColor(Color.BLACK);
		    paint.setStyle(Paint.Style.STROKE);
		    canvas.drawRect(rect, paint);*/
	    }

	    public override bool collidingCircle(Ball b)
	    {
		    if(collidingCircle(b.getX(), b.getY(), b.getRadius()))
		    {
			    maze.triggerEndLevel();
		    }
		    return false;
	    }
    }
}
