using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsMazeGame
{
    public class Ball
    {
        
	    private float x, y;
        private int radius;
	    private bool colliding;
	    private float velX;
	    private float velY;
	    private Maze maze;
	    private float maxVel, minVel;
        private Texture2D circle;
	
	    public Ball(float x, float y, float radius, Texture2D circle, Maze maze)
	    {
		    this.maze = maze;
            this.circle = circle;
		
		    this.x = x;
		    this.y = y;
		    this.radius = (int)radius;
		    colliding = false;
		    velX = velY = 0;
		    maxVel = 100;
		    minVel = -100;
	    }
	
	    public void update(long gameTime)
	    {
		    float oldX = x;
		    float oldY = y;

            //Log.d("BALL", "Vel: x=" + velX + " y=" + velY + " gameTime=" + gameTime);
		    setLocation(x + velX * gameTime/1000.0f, y + velY * gameTime/1000.0f);
		    if(maze.testCollision(this))
		    {
			    float newX = x;
			    this.x = oldX;
			    if(maze.testCollision(this))
			    {
				    this.x = newX;
				    this.y = oldY;
				    if(maze.testCollision(this))
				    {
					    setLocation(oldX, oldY);
				    }
			    }
		    }
	    }
	
	    public void draw(SpriteBatch spriteBatch)
	    {
		    //Paint circlePaint = new Paint();
		    //if(colliding)
		    //	circlePaint.setColor(Color.YELLOW);
		    //else
		    //circlePaint.setColor(Color.BLUE);
		    //circlePaint.setStyle(Style.FILL);
		    //circlePaint.setStrokeWidth((float) 5.0);
		    //canvas.drawCircle(x, y, radius, circlePaint);
            spriteBatch.Draw(circle, new Rectangle((int)x, (int)y, radius * 2, radius * 2), Color.Blue);
            
	    }
	
	    public void setLocation(float x, float y)
	    {
		    this.x = x;
		    this.y = y;
	    }
	
	    public void setColliding(bool colliding)
	    {
		    this.colliding = colliding;
		    if(colliding)
		    {
			    velX = velY = 0;
		    }
	    }
	
	    public float getX()
	    {
            return x - (int)(0.5 * radius);//- (int)(1.4 * radius);// - radius;
	    }
	
	    public float getY()
	    {
            return y - (int)(0.5 * radius);// -(int)(1.40 * radius);
	    }
	
	    public float getRadius()
	    {
		    return radius;
	    }
	
	    public void updateVelocity(float modX, float modY)
	    {
		    this.velX += modY;
		    this.velY += modX;
		
		    if(velX > maxVel)
			    velX = maxVel;
		    if(velX < minVel)
			    velX = minVel;
		    if(velY > maxVel)
			    velY = maxVel;
		    if(velY < minVel)
			    velY = minVel;
			
	    }
    }
}
