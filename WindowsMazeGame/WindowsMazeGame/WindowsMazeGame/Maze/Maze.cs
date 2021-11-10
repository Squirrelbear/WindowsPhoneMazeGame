using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsMazeGame
{
    public class Maze
    {
        private List<Wall> walls;
        private List<DoorWall> doors;
        private int screenWidth, screenHeight;
        private Ball ball;

        //private long accelUpdate;
        private Game1 panel;

        private int curMapID;
        private int[][] mazeGrid;
        //private ButtonTarget[] buttonMap;
        private NotifyLink[] buttonLinks;

        private List<Texture2D> bitmaps;
        private Random gen;

        public Maze(int screenWidth, int screenHeight, Game1 panel)
        {
            this.panel = panel;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            walls = new List<Wall>();
            doors = new List<DoorWall>();

            loadTextures();

            gen = new Random();

            loadRandomMap();

            //curMapID = 0;

            /*
            // This defines the map
            // 0 = nothing
            // 1 = button 
            // 2 = door
            // 3 = level end
            // 4 = normal wall
            // 5 = ball start		
                               // 0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19
            int[][] mazeGrid = { {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4}, // 0
                                 {4, 1, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 1
                                 {4, 0, 0, 0, 0, 4, 0, 0, 0, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 4}, // 2
                                 {4, 0, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4}, // 3
                                 {4, 0, 0, 0, 0, 4, 4, 4, 0, 4, 4, 0, 4, 4, 0, 0, 0, 0, 0, 4}, // 4
                                 {4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 5
                                 {4, 0, 0, 0, 0, 0, 0, 0, 0, 5, 4, 4, 4, 4, 0, 0, 0, 0, 0, 4}, // 6
                                 {4, 0, 0, 0, 0, 4, 4, 2, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 7
                                 {4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 8
                                 {4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 9
                                 {4, 3, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 10
                                 {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4} }; // 11
            this.mazeGrid = mazeGrid;
            ButtonTarget[] buttonMap = new ButtonTarget[1];
            buttonMap[0] = new ButtonTarget(1, 1, 7, 7);
            this.buttonMap = buttonMap;
		
            setMaze(mazeGrid, buttonMap);
            */
            /*
            int cols = 10;
            int rows = 5;
            float wallWidth = screenWidth / cols;
            float wallHeight = screenHeight / rows;
		
            walls = new ArrayList<Wall>();
            for(int row = 0; row < rows; row++)
            {
                for(int col = 0; col < cols; col++)
                {
                    if(col == 0 || row == 0 || col == cols-1 || row == rows-1)
                        walls.add(new Wall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this));
                }
            }
            walls.add(new DoorWall(1 * wallWidth, 1 * wallHeight, wallWidth, wallHeight, 1, 1, this));
            walls.add(new ButtonWall(7 * wallWidth, 3 * wallHeight, wallWidth, wallHeight, 8, 4, this, 1, 1));
            walls.add(new LevelEndWall(3 * wallWidth, 1 * wallHeight, wallWidth, wallHeight, 3, 1, this));
		
            float ballRadius = (wallWidth > wallHeight) ? wallHeight / 2 : wallWidth / 2;
            ballRadius *= 0.8f;
            ball = new Ball(screenWidth / 2, screenHeight / 2, ballRadius, this);
            accelUpdate = -1;
            */
        }

        public void update(long elapsedTime)
        {
            ball.update(elapsedTime);
        }

        public bool testCollision(Ball b)
	    {
		    ball.setColliding(false);
		    foreach(Wall w in walls)
		    {
			    if(w.collidingCircle(ball))
			    {
				    ball.setColliding(true);
				    return w.reactCollision(b);
			    }
		    }
		    return false;
	    }

        public void draw(SpriteBatch spriteBatch)
	    {
		    foreach(Wall w in walls)
		    {
				    w.draw(spriteBatch);
		    }
		
		    ball.draw(spriteBatch);
	    }

        public void handleTouchEvent(float x, float y)
        {
            //ball.setLocation(x, y);
        }

        public void handleSensorEvent(float xAcc, float yAcc, float zAcc, long time)
        {
            //if (accelUpdate != -1)
            //{
                //long diff = System.currentTimeMillis() - accelUpdate;
                ball.updateVelocity(xAcc * time, yAcc * time);
            //}

            //accelUpdate = System.currentTimeMillis();
        }

        public void switchDoor(int targetX, int targetY, bool isWallUp)
	    {
		    foreach(Wall w in walls)
		    {
			    if(w.isGridCoord(targetX, targetY))
			    {
				    if(w.getWallType() == 2)
				    {
					    ((DoorWall)w).setWallUp(isWallUp);
				    }
				    else
				    {
					    //Log.d("MAZE", "ERROR: " + targetX + "," + targetY + " : " + w.getWallType());
				    }
				    break;
			    }
		    }
	    }

        public void notifyDoors(int notifyID)
        {
            //Log.d("MAZE", "Notifying: " + notifyID);

            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i].getListenID() == notifyID)
                {
                    doors[i].setWallUp(false);
                    //Log.d("MAZE", "Notified " + doors.get(i).getCol() + " " + doors.get(i).getRow());
                }
            }
        }

        public void triggerEndLevel()
        {
            panel.handleLevelFinished();
        }

        public void setMaze(int[][] mazeGrid, ButtonTarget[] buttonMap)
        {
            walls.Clear();

            int wallWidth = screenWidth / mazeGrid[0].Length;
            int wallHeight = screenHeight / mazeGrid.Length;

            int ballCol = 0;
            int ballRow = 0;

            for (int row = 0; row < mazeGrid.Length; row++)
            {
                for (int col = 0; col < mazeGrid[row].Length; col++)
                {
                    if (mazeGrid[row][col] == 0) continue;

                    if (mazeGrid[row][col] == 5)
                    {
                        ballCol = col;
                        ballRow = row;
                    }

                    int targetX = 0, targetY = 0;
                    for (int k = 0; k < buttonMap.Length; k++)
                    {
                        if (buttonMap[k].isSource(col, row))
                        {
                            targetX = buttonMap[k].targetX;
                            targetY = buttonMap[k].targetY;
                        }
                    }

                    walls.Add(createWall(mazeGrid[row][col], col, row, wallWidth, wallHeight, targetX, targetY));
                }
            }

            float ballRadius = (wallWidth > wallHeight) ? wallHeight / 2 : wallWidth / 2;
            ballRadius *= 0.7f;
            ball = new Ball(ballCol * wallWidth + ballRadius, ballRow * wallHeight + ballRadius, ballRadius, getTexture(6), this);
            //accelUpdate = -1;
        }

        public void setMaze(int[][] mazeGrid, NotifyLink[] buttonLinks)
        {
            walls.Clear();
            doors.Clear();

            int wallWidth = screenWidth / mazeGrid[0].Length;
            int wallHeight = screenHeight / mazeGrid.Length;

            int ballCol = 0;
            int ballRow = 0;

            for (int row = 0; row < mazeGrid.Length; row++)
            {
                for (int col = 0; col < mazeGrid[row].Length; col++)
                {
                    int type = mazeGrid[row][col];
                    if (type == 0) continue;

                    if (type == 5)
                    {
                        ballCol = col;
                        ballRow = row;
                    }
                    else if (type == 1 || type == 2)
                    {
                        int notifyID = -1;
                        for (int k = 0; k < buttonLinks.Length; k++)
                        {
                            if (buttonLinks[k].isLocation(col, row))
                                notifyID = buttonLinks[k].id;
                        }
                        if (notifyID == -1)
                        {
                            //Log.d("MAZE", "HUGE ERROR LINK NOT FOUND!!!");
                        }

                        Wall w = createWall(type, col, row, wallWidth, wallHeight, notifyID);
                        walls.Add(w);
                        if (type == 2)
                            doors.Add((DoorWall)w);
                    }
                    else
                    {
                        walls.Add(createWall(type, col, row, wallWidth, wallHeight, 0, 0));
                    }
                }
            }

            float ballRadius = (wallWidth > wallHeight) ? wallHeight / 2 : wallWidth / 2;
            ballRadius *= 0.7f;
            ball = new Ball(ballCol * wallWidth + ballRadius, ballRow * wallHeight + ballRadius, ballRadius, getTexture(6), this);
            //accelUpdate = -1;
        }

        public void resetMaze()
        {
            setMaze(mazeGrid, buttonLinks);
        }

        public Wall createWall(int type, int col, int row, int wallWidth, int wallHeight, int targetX, int targetY)
        {
            switch (type)
            {
                case 4:
                    return new Wall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
                case 1:
                    return new ButtonWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this, targetX, targetY);
                case 2:
                    return new DoorWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
                case 3:
                    return new LevelEndWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
                case 5:
                    return new BallStartWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
            }

            return null;
        }

        public Wall createWall(int type, int col, int row, int wallWidth, int wallHeight, int notifyID)
        {
            switch (type)
            {
                case 4:
                    return new Wall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
                case 1:
                    return new ButtonWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this, notifyID);
                case 2:
                    return new DoorWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this, notifyID);
                case 3:
                    return new LevelEndWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
                case 5:
                    return new BallStartWall(col * wallWidth, row * wallHeight, wallWidth, wallHeight, col, row, this);
            }

            return null;
        }

        public Texture2D getTexture(int textureID)
        {
            if (textureID >= bitmaps.Count)
                return bitmaps[0];
            return bitmaps[textureID];
        }

        public void setMap(int map)
        {
            this.curMapID = map;
        }

        public int getMapID()
        {
            return curMapID;
        }

        public void loadRandomMap()
        {
            int nextMap = gen.Next(2) + 1;
            loadMap(nextMap);
        }

        public void loadMap(int id)
        {
            switch (id)
            {
                case 1:
                    loadMap1();
                    break;
                case 2:
                    loadMap2();
                    break;
                default:
                    loadMap1();
                    id = 1;
                    break;
            }
            curMapID = id;

            setMaze(mazeGrid, buttonLinks);
        }

        private void loadTextures()
	    {
		    string[] fileNames = { "buttonhit", "buttonunhit", "doorlocked",
							       "dooropen", "levelend", "wall", "circle" };
		    bitmaps = new List<Texture2D>();
		    //AssetManager assetManager = context.getAssets();
		
		    foreach(string fileName in fileNames)
		    {
                bitmaps.Add(panel.Content.Load<Texture2D>(fileName));
		    }
		
	    }

        private void loadMap1()
        {
            // This defines the map
            // 0 = nothing
            // 1 = button 
            // 2 = door
            // 3 = level end
            // 4 = normal wall
            // 5 = ball start		
                                                     // 0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19
            int[][] mazeGrid = new int[][] { new int[] {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4}, // 0
							                 new int[] {4, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 1
							                 new int[] {4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4}, // 2
							                 new int[] {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 4}, // 3
							                 new int[] {4, 0, 0, 2, 0, 0, 4, 1, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4}, // 4
							                 new int[] {4, 0, 0, 4, 0, 0, 4, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4}, // 5
							                 new int[] {4, 0, 0, 4, 0, 0, 4, 2, 4, 0, 0, 4, 0, 0, 4, 0, 0, 0, 1, 4}, // 6
							                 new int[] {4, 0, 1, 4, 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 4, 0, 4, 4, 4, 4}, // 7
							                 new int[] {4, 2, 4, 4, 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 4, 2, 4, 4, 4, 4}, // 8
							                 new int[] {4, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4}, // 9
							                 new int[] {4, 3, 0, 4, 1, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 5, 4}, // 10
							                 new int[] {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4} }; // 11
            this.mazeGrid = mazeGrid;
            /*ButtonTarget[] buttonMap = new ButtonTarget[5];
            buttonMap[0] = new ButtonTarget(1, 1, 1, 8);
            buttonMap[1] = new ButtonTarget(2, 7, 3, 1);
            buttonMap[2] = new ButtonTarget(7, 4, 3, 4);
            buttonMap[3] = new ButtonTarget(18, 6, 7, 6);
            buttonMap[4] = new ButtonTarget(4, 10, 15, 8);
            this.buttonMap = buttonMap;*/
            NotifyLink[] buttonLinks = new NotifyLink[10];
            buttonLinks[0] = new NotifyLink(1, 1, 1);
            buttonLinks[1] = new NotifyLink(1, 8, 1);
            buttonLinks[2] = new NotifyLink(2, 7, 2);
            buttonLinks[3] = new NotifyLink(3, 1, 2);
            buttonLinks[4] = new NotifyLink(7, 4, 3);
            buttonLinks[5] = new NotifyLink(3, 4, 3);
            buttonLinks[6] = new NotifyLink(18, 6, 4);
            buttonLinks[7] = new NotifyLink(7, 6, 4);
            buttonLinks[8] = new NotifyLink(4, 10, 5);
            buttonLinks[9] = new NotifyLink(15, 8, 5);
            this.buttonLinks = buttonLinks;
        }

        private void loadMap2()
        {
            // This defines the map
            // 0 = nothing
            // 1 = button 
            // 2 = door
            // 3 = level end
            // 4 = normal wall
            // 5 = ball start		
                                                     // 0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19
            int[][] mazeGrid = new int[][] { new int[] {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4}, // 0
							                 new int[] {4, 0, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 4}, // 1
							                 new int[] {4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 4}, // 2
							                 new int[] {4, 0, 4, 4, 0, 0, 4, 4, 4, 0, 4, 4, 4, 0, 0, 0, 4, 4, 0, 4}, // 3
							                 new int[] {4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4}, // 4
							                 new int[] {4, 4, 4, 4, 4, 4, 4, 0, 0, 5, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4}, // 5
							                 new int[] {4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4}, // 6
							                 new int[] {4, 0, 0, 0, 0, 0, 4, 4, 4, 0, 4, 4, 4, 0, 0, 0, 0, 0, 0, 4}, // 7
							                 new int[] {4, 0, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 0, 4}, // 8
							                 new int[] {4, 0, 0, 4, 0, 0, 0, 4, 4, 4, 4, 4, 4, 0, 0, 0, 4, 0, 0, 4}, // 9
							                 new int[] {4, 0, 1, 4, 0, 0, 0, 4, 3, 2, 2, 2, 2, 0, 0, 0, 4, 1, 0, 4}, // 10
							                 new int[] {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4} }; // 11
            this.mazeGrid = mazeGrid;
            NotifyLink[] buttonLinks = new NotifyLink[8];
            buttonLinks[0] = new NotifyLink(2, 1, 1);
            buttonLinks[1] = new NotifyLink(12, 10, 1);
            buttonLinks[2] = new NotifyLink(2, 10, 2);
            buttonLinks[3] = new NotifyLink(11, 10, 2);
            buttonLinks[4] = new NotifyLink(17, 1, 3);
            buttonLinks[5] = new NotifyLink(10, 10, 3);
            buttonLinks[6] = new NotifyLink(17, 10, 4);
            buttonLinks[7] = new NotifyLink(9, 10, 4);
            this.buttonLinks = buttonLinks;
        }
    }
}
