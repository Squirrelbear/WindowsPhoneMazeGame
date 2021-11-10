using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class ButtonWall : Wall
    {
        public bool isHit;
	    private int targetX, targetY;
	    private int notifyID;
	    private bool useTarget;
	
	    public ButtonWall(int x, int y, int width, int height, int col, int row, Maze maze, int targetX, int targetY) 
		   :  base(x, y, width, height, col, row, maze)
        {
		    wallType = 1;
		    isHit = false;
		    this.targetX = targetX;
		    this.targetY = targetY;
		    texture = maze.getTexture(1);
		    useTarget = true;
	    }
	
	    public ButtonWall(int x, int y, int width, int height, int col, int row, Maze maze, int notifyID) 
		  :  base(x, y, width, height, col, row, maze)
        {
            wallType = 1;
		    isHit = false;
		    this.notifyID = notifyID;
		    texture = maze.getTexture(1);
		    useTarget = false;
	    }
	
	    public override void draw(SpriteBatch spriteBatch)
	    {
		    base.draw(spriteBatch);
		    /*Paint paint = new Paint();
		    if(isHit)
			    paint.setColor(Color.BLACK);
		    else
			    paint.setColor(Color.CYAN);
		    canvas.drawRect(rect, paint);
		    paint.setColor(Color.BLACK);
		    paint.setStyle(Paint.Style.STROKE);
		    canvas.drawRect(rect, paint);*/
	    }
	
	    public void setHit(bool isHit)
	    {
		    if(isHit)
		    {
			    texture = maze.getTexture(0);
		    }
		    else 
		    {
			    texture = maze.getTexture(1);
		    }
		    this.isHit = isHit;
	    }
	
	    public override bool collidingCircle(Ball b)
	    {
		    if(!isHit && collidingCircle(b.getX(), b.getY(), b.getRadius()))
		    {
			    setHit(true);
			    if(!useTarget)
				    maze.notifyDoors(notifyID);
			    else
				    maze.switchDoor(targetX, targetY, false);
		    }
		    return false;
	    }
	
	    public override bool reactCollision(Ball b)
	    {
		    return true;
	    }
    }
}
