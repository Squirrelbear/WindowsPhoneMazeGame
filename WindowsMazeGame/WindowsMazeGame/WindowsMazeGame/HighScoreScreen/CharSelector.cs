using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class CharSelector
    {
        public static String[] CHARS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
									"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
									"U", "V", "W", "X", "Y", "Z" };

        private int textPosX, textPosY;
        private int minSelX, maxSelX, topY, bottomY, curSelX;
        private int minSelY, maxSelY, leftX, rightX, curSelY;
        private Rectangle arrowBottomRect, arrowTopRect;

        private int curChar;
        private bool hasFocus;

        private Texture2D arrowTop, arrowBottom, circle;
        private SpriteFont font;

        public CharSelector(int x, int y, Texture2D arrowTop, Texture2D arrowBottom, Texture2D circle, SpriteFont font)
        {
            this.arrowTop = arrowTop;
            this.arrowBottom = arrowBottom;
            this.circle = circle;
            this.font = font;
            curChar = 0;
            hasFocus = false;

            // bars
            minSelX = x - 60; // -20
            maxSelX = x + 100; //200 + 20;
            topY = y - 125 - 60;//15 - 40;
            bottomY = y + 10; //+ 40;//+ 200 + 20;

            // balls
            leftX = x - 30 - 20;
            rightX = x + 170;// 200 + 20;
            minSelY = y - 125 - 10; //y - 10;
            maxSelY = y - 10; //y + 200 - 10 - 20;

            textPosX = x;
            textPosY = y;

            curSelX = (minSelX + maxSelX) / 2;
            curSelY = (minSelY + maxSelY) / 2;

            //arrowTopRect = new RectF(curSelX, topY, curSelX + 80, topY + 40);
            arrowTopRect = new Rectangle(curSelX, topY, 80, 40);
            //arrowBottomRect = new RectF(curSelX, bottomY, curSelX + 80, bottomY + 40);
            arrowBottomRect = new Rectangle(curSelX, bottomY, 80, 40);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (hasFocus)
            {
                /*Paint paint = new Paint();
                paint.setColor(Color.GRAY);
                canvas.drawBitmap(arrowTop, null, arrowTopRect, null);
                canvas.drawBitmap(arrowBottom, null, arrowBottomRect, null);*/
                spriteBatch.Draw(arrowTop, arrowTopRect, Color.White);
                spriteBatch.Draw(arrowBottom, arrowBottomRect, Color.White);    
                //canvas.drawRect(new RectF(curSelX, topY, curSelX + 80, topY + 40), paint);
                //canvas.drawRect(new RectF(curSelX, bottomY, curSelX + 80, bottomY + 40), paint);
                //canvas.drawCircle(leftX, curSelY, 10, paint);
                //canvas.drawCircle(rightX, curSelY, 10, paint);
                spriteBatch.Draw(circle, new Rectangle(leftX, curSelY, 10, 10), Color.LightGray);
                spriteBatch.Draw(circle, new Rectangle(rightX, curSelY, 10, 10), Color.LightGray);
            }

            /*Paint paintText = new Paint();
            paintText.setColor(Color.BLACK);
            paintText.setTextSize(200);
            canvas.drawText(CHARS[curChar], textPosX, textPosY, paintText);*/
            //spriteBatch.DrawString(font, CHARS[curChar], new Vector2(textPosX, textPosY), Color.Black);
            spriteBatch.DrawString(font, CHARS[curChar], new Vector2(textPosX+10, textPosY-155), Color.Black, 0, new Vector2(0,0), 5f, SpriteEffects.None, 0);
        }

        public void previous()
        {
            curChar--;
            if (curChar < 0)
                curChar = CHARS.Length - 1;
        }

        public void next()
        {
            curChar++;
            if (curChar >= CHARS.Length)
                curChar = 0;
        }

        public void setSelCurX(float percent)
        {
            if (percent > 1)
                percent = 1;
            else if (percent < 0)
                percent = 0;

            curSelX = (int)(minSelX + (maxSelX - minSelX) * percent);
            arrowTopRect = new Rectangle(curSelX, topY, 80,  40);
            arrowBottomRect = new Rectangle(curSelX, bottomY, 80, 40);
        }

        public void setSelCurY(float percent)
        {
            if (percent > 1)
                percent = 1;
            else if (percent < 0)
                percent = 0;

            curSelY = (int)(minSelY + (maxSelY - minSelY) * percent);
        }

        public void setFocus(bool hasFocus)
        {
            if (!this.hasFocus)
            {
                curSelX = (minSelX + maxSelX) / 2;
                curSelY = (minSelY + maxSelY) / 2;
            }
            this.hasFocus = hasFocus;
        }

        public String getChar()
        {
            return CHARS[curChar];
        }
    }
}
