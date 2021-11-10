using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class HighScoreScreen
    {
        private int screenWidth, screenHeight;
        private float time;
        private CharSelGroup nameInput;
        private MenuButton save, skip, newGame, quitGame;
        private Game1 panel;
        private DatabaseHandler db;
        private List<HighScore> scores;
        private bool showScoreInput;
        private SpriteFont font;
        private SpriteFont hugeFont;

        public HighScoreScreen(int screenWidth, int screenHeight, long time, Game1 panel)
        {
            db = new DatabaseHandler();
            //db.clearDatabase();

            this.panel = panel;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.time = time / 1000.0f;

            // get the list of scores and check if the score is a high score
            scores = db.getScores(panel.getMapID());
            //Log.d("HIGHSCORES", "Scores: " + scores.size());

            bool isHighScore = false;
            if (scores.Count >= 5)
            {
                for (int i = 0; i < scores.Count; i++)
                {
                    //Log.d("HIGHSCORES", time + " " + scores.get(i).getScore() + " " + (time < scores.get(i).getScore()));
                    if (this.time < scores[i].getScore())
                    {
                        isHighScore = true;
                        break;
                    }
                }
            }
            else
            {
                isHighScore = true;
            }
            showScoreInput = isHighScore;

            //Collections.sort(scores, new ScoreComparator());
            scores.Sort(delegate(HighScore s1, HighScore s2) { return s1.getScore().CompareTo(s2.getScore()); });

            int btnWidth = 300;
            int btnHeight = 100;
            int btnYPos = this.screenHeight - 110;
            int btnLeftX = this.screenWidth / 2 - btnWidth - 30;
            int btnRightX = this.screenWidth / 2 + 30;

            Texture2D buttonTexture = panel.Content.Load<Texture2D>("button");
            font = panel.Content.Load<SpriteFont>("largeFont");
            hugeFont = panel.Content.Load<SpriteFont>("hugeFont");

            if (isHighScore)
            {
                nameInput = new CharSelGroup(100, 300, panel);

                save = new MenuButton(new Rectangle(btnLeftX, btnYPos, btnWidth, btnHeight), "Save", buttonTexture, font, 1);
                skip = new MenuButton(new Rectangle(btnRightX, btnYPos, btnWidth, btnHeight), "Skip", buttonTexture, font, 0);
            }

            newGame = new MenuButton(new Rectangle(btnLeftX, btnYPos, btnWidth, btnHeight), "New Game", buttonTexture, font,  2);
            quitGame = new MenuButton(new Rectangle(btnRightX, btnYPos, btnWidth, btnHeight), "Exit Game", buttonTexture, font, 3);
        }

        public void update(long elapsedTime)
        {
            if (showScoreInput)
                nameInput.update(elapsedTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {

            if (showScoreInput)
            {
                /*Paint paint = new Paint();
                paint.setColor(Color.rgb(178, 34, 34));//Color.RED); 
                paint.setTextSize(80);
                canvas.drawText("Your Score: " + time, 10, 70, paint);*/
                //spriteBatch.DrawString(hugeFont, "Your Score: " + time, new Vector2(10, 70), new Color(178, 34, 34));
                spriteBatch.DrawString(hugeFont, "Your Score: " + time, new Vector2(10, 30), new Color(178, 34, 34), 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);

                nameInput.draw(spriteBatch);
                save.draw(spriteBatch);
                skip.draw(spriteBatch);
            }
            else
            {
                /*Paint paint = new Paint();
                paint.setColor(Color.BLACK);//Color.RED); 
                paint.setTextSize(50);
                canvas.drawText("RANK  NAME  TIME", 10, 50, paint);*/
                //spriteBatch.DrawString(font, "RANK  NAME  TIME", new Vector2(10, 50), Color.Black);
                spriteBatch.DrawString(font, "RANK  NAME  TIME", new Vector2(10, 20), Color.Black, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);

                float y = 30;
                //int rank = 1;
                //for(int i = scores.size() - 1; i >= 0; i--)
                for (int i = 0; i < scores.Count; i++)
                {
                    y += 50;
                    HighScore s = scores[i];
                    //canvas.drawText((i + 1) + ".         " + s.getName() + "     " + s.getScore(), 10, y, paint);
                    spriteBatch.DrawString(font, (i + 1) + ".       " + s.getName() + "    " + s.getScore(),
                                            new Vector2(10, y), Color.Black, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
                    //rank++;
                }
                //paint.setColor(Color.RED);//Color.RED); 
                //paint.setTextSize(80);
                y = screenHeight / 2 - 40;
                float x = screenWidth / 2 + 100;
                //canvas.drawText(time + "", x, y, paint);
                spriteBatch.DrawString(hugeFont, time + "", new Vector2(x+20, y), Color.Red, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
                //paint.setTextSize(50);
                //canvas.drawText("Your score:", x, y - 80, paint);
                spriteBatch.DrawString(font, "Your score:", new Vector2(x+20, y - 80), Color.Red, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 0);

                newGame.draw(spriteBatch);
                quitGame.draw(spriteBatch);
            }
        }

        public void handleTouchEvent(int x, int y)
        {
            int result = -1;
            if (showScoreInput)
            {
                if (save.isPointInButton(x, y))
                {
                    result = 1;
                }
                else if (skip.isPointInButton(x, y))
                {
                    result = 0;
                }
            }
            else
            {
                if (newGame.isPointInButton(x, y))
                {
                    panel.newGame();
                }
                else if (quitGame.isPointInButton(x, y))
                {
                    panel.closeGame();
                }
            }

            if (result == 0 || result == 1)
            {
                if (result == 1)
                {
                    // remove the lowest score if needed
                    if (scores.Count >= 5)
                    {
                        int removeID = scores[scores.Count - 1].getID();
                        db.deleteScore(removeID);
                        //Log.d("HIGHSCORE", "Deleting score: " + removeID + " time: " + scores.get(scores.size() - 1).getScore());
                    }
                    HighScore score = new HighScore(panel.getMapID(), time, nameInput.getString());
                    db.addScore(score);
                    //Log.d("HIGHSCORE", "Score Inserted");

                    // update the known list of scores
                    scores = db.getScores(panel.getMapID());
                    //Collections.sort(scores, new ScoreComparator());
                    scores.Sort(delegate(HighScore s1, HighScore s2) { return s1.getScore().CompareTo(s2.getScore()); });

                    //Log.d("HIGHSCORES", "New Score Count: " + scores.size());
                }

                // switch score view
                showScoreInput = false;
            }
        }

        public void handleSensorEvent(float xAcc, float yAcc, float zAcc, long time)
        {
            if (showScoreInput)
                nameInput.handleSensorEvent(xAcc, yAcc, zAcc, time);
        }
    }
}
