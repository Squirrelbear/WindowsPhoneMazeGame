using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class MenuButton
    {
        protected Rectangle rect;
        protected String text;
        protected int actionID;
        protected Texture2D buttonTexture;
        protected SpriteFont font;

        public MenuButton(Rectangle rect, String text, Texture2D buttonTexture, SpriteFont font, int actionID)
        {
            this.buttonTexture = buttonTexture;
            this.font = font;

            this.rect = rect;
            this.text = text;
            this.actionID = actionID;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, rect, Color.White);

            spriteBatch.DrawString(font, text, new Vector2(rect.X + 20, rect.Y + 20), Color.Black, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(font, text, new Vector2(rect.X + 30, rect.Y + 60), Color.Black);
            
            /*Paint paint = new Paint();
            paint.setColor(Color.GRAY);
            canvas.drawRect(rect, paint);
            paint.setColor(Color.BLACK);
            paint.setStyle(Paint.Style.STROKE);
            canvas.drawRect(rect, paint);

            Paint paintText = new Paint();
            paintText.setColor(Color.BLACK);
            paintText.setTextSize(50);
            canvas.drawText(text, rect.left + 30, rect.top + 60, paintText);*/
        }

        public bool isPointInButton(int x, int y)
        {
            return isPointInButton(new Point(x, y));
        }

        public bool isPointInButton(Point p)
        {
            return rect.Contains(p);
        }

        public int getActionID()
        {
            return actionID;
        }
    }
}
