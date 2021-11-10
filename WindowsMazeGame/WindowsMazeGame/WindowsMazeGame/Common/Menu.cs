using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class Menu
    {
        private int screenWidth, screenHeight;
        //private long accelUpdate;
        private Game1 panel;
        private List<MenuButton> buttons;

        public Menu(int screenWidth, int screenHeight, Game1 panel)
        {
            Texture2D buttonTexture = panel.Content.Load<Texture2D>("button");
            SpriteFont font = panel.Content.Load<SpriteFont>("largeFont");

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.panel = panel;
            buttons = new List<MenuButton>();
            int btnWidth = 300;
            int btnHeight = 100;
            int btnX = this.screenWidth / 2 - btnWidth / 2;
            int btnNGY = this.screenHeight / 2 - btnHeight / 2;
            int btnEGY = this.screenHeight / 2 + btnHeight / 2 + 30;
            int btnRGY = this.screenHeight / 2 - 3 * btnHeight / 2 - 30;
            buttons.Add(new MenuButton(new Rectangle(btnX, btnRGY, btnWidth, btnHeight), "Resume", buttonTexture, font, 2));
            buttons.Add(new MenuButton(new Rectangle(btnX, btnNGY, btnWidth, btnHeight), "New Game", buttonTexture, font, 0));
            buttons.Add(new MenuButton(new Rectangle(btnX, btnEGY, btnWidth, btnHeight), "Exit Game", buttonTexture, font, 1));
        }

        public void update(long elapsedTime)
        {

        }

        public void draw(SpriteBatch spriteBatch)
	    {
		    foreach(MenuButton btn in buttons)
		    {
			    btn.draw(spriteBatch);
		    }
	    }

        public void handleTouchEvent(int x, int y)
	    {
		    //ball.setLocation(x, y);
		    int resultID = -1;
		    foreach(MenuButton btn in buttons)
		    {
			    if(btn.isPointInButton(x, y))
			    {
				    resultID = btn.getActionID();
				    break;
			    }
		    }
		
		    if(resultID == 0)
		    {
			    panel.newGame();
		    }
		    else if(resultID == 1)
		    {
			    panel.closeGame();
		    }
		    else if(resultID == 2)
		    {
			    panel.hideMenu();
		    }
	    }

        public void handleSensorEvent(float xAcc, float yAcc, float zAcc)
        {
            /*if(accelUpdate != -1)
            {
                long diff = System.currentTimeMillis() - accelUpdate;
                //ball.updateVelocity(xAcc * diff, yAcc * diff);
            }
		
            accelUpdate = System.currentTimeMillis();*/
        }
    }
}
