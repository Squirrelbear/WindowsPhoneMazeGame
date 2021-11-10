using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class DoorWall : Wall
    {
        public bool isWallUp;
	    public int listenID;
	
	    public DoorWall(int x, int y, int width, int height, int col, int row, Maze maze)
		  :  base(x, y, width, height, col, row, maze)
        {
		    isWallUp = true;
		    wallType = 2;
		    texture = maze.getTexture(2);
	    }
	
	    public DoorWall(int x, int y, int width, int height, int col, int row, Maze maze, int listenID) 
		    : base(x, y, width, height, col, row, maze)
		{
            isWallUp = true;
		    wallType = 2;
		    texture = maze.getTexture(2);
		    this.listenID = listenID;
	    }
	
	    public override void draw(SpriteBatch spriteBatch)
	    {
		    base.draw(spriteBatch);
		    /*Paint paint = new Paint();
		    if(isWallUp)
			    paint.setColor(Color.GRAY);
		    else
			    paint.setColor(Color.GREEN);
		    canvas.drawRect(rect, paint);
		    paint.setColor(Color.BLACK);
		    paint.setStyle(Paint.Style.STROKE);
		    canvas.drawRect(rect, paint);*/
	    }
	
	    public void setWallUp(bool isWallUp)
	    {
		    if(isWallUp)
		    {
			    texture = maze.getTexture(2);
		    }
		    else
		    {
			    texture = maze.getTexture(3);
		    }
		
		    this.isWallUp = isWallUp;
	    }
	
	    public override bool collidingCircle(Ball b)
	    {
		    if(!isWallUp) return false;
		
		    return collidingCircle(b.getX(), b.getY(), b.getRadius());
	    }
	
	    public override bool reactCollision(Ball b)
	    {
		    return true;
	    }
	
	    public int getListenID()
	    {
		    return listenID;
	    }
    }
}
