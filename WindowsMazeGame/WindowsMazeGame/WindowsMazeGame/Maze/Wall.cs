using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class Wall
    {
        protected Rectangle rect;
        protected int col, row;
        protected int wallType;
        protected Maze maze;
        protected Texture2D texture;

        public Wall(Rectangle rect, int col, int row, Maze maze)
        {
            this.col = col;
            this.row = row;
            this.maze = maze;
            this.rect = rect;
            wallType = 4;
            texture = maze.getTexture(5);
        }

        public Wall(int x, int y, int width, int height, int col, int row, Maze maze)
        {
            this.col = col;
            this.row = row;
            this.maze = maze;
            rect = new Rectangle(x, y, width, height);
            wallType = 4;
            texture = maze.getTexture(5);
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            //canvas.drawBitmap(texture, null, rect, null);
            spriteBatch.Draw(texture, rect, Color.White);

            /*
            Paint paint = new Paint();
            paint.setColor(Color.MAGENTA);
            canvas.drawRect(rect, paint);
            paint.setColor(Color.BLACK);
            paint.setStyle(Paint.Style.STROKE);
            canvas.drawRect(rect, paint);*/
        }

        // http://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
        public virtual bool collidingCircle(float x, float y, float radius)
        {
            float cDX = Math.Abs(x - rect.Left);
            float cDY = Math.Abs(y - rect.Top);

            if (cDX > (rect.Width / 2.0f + radius)) return false;
            if (cDY > (rect.Height / 2.0f + radius)) return false;

            if (cDX <= (rect.Width / 2.0f)) return true;
            if (cDY <= (rect.Height / 2.0f)) return true;

            float cD_sq = (float)(Math.Pow(cDX - rect.Width / 2.0f, 2.0f) + Math.Pow(cDY - rect.Height / 2.0f, 2.0f));

            return (cD_sq <= Math.Pow(radius, 2.0f));
        }

        public virtual bool collidingCircle(Ball b)
        {
            return collidingCircle(b.getX(), b.getY(), b.getRadius());
        }

        public virtual bool reactCollision(Ball b)
        {
            return true;
        }

        public int getWallType()
        {
            return wallType;
        }

        public int getCol()
        {
            return col;
        }

        public int getRow()
        {
            return row;
        }

        public bool isGridCoord(int row, int col)
        {
            return this.row == row && this.col == col;
        }
    }
}
