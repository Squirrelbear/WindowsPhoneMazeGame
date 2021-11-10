using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Devices.Sensors;

namespace WindowsMazeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        // http://www.smartypantscoding.com/creating-a-windows-phone-7-xna-game-in-landscape-orientation
        //private RenderTarget2D renderTarget;

        private Maze maze;
        private HighScoreScreen highScoreScreen;
        private Menu menu;
        private bool levelFinished;
        private bool menuEnabled;
        private long levelTime;
        private int screenWidth, screenHeight;

        // http://msdn.microsoft.com/en-us/library/ff604984%28v=xnagamestudio.40%29.aspx
        private Accelerometer accelSensor;
        private Vector3 accelReading = new Vector3();
        private bool accelActive;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //this.screenWidth = graphics.GraphicsDevice.Viewport.Height;
            //this.screenHeight = graphics.GraphicsDevice.Viewport.Width;
            this.screenWidth = graphics.GraphicsDevice.Viewport.Width;
            this.screenHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            levelFinished = false;
            menuEnabled = false;
            levelTime = 0;
            maze = new Maze(screenWidth, screenHeight, this);
            menu = new Menu(screenWidth, screenHeight, this);

            accelSensor = new Accelerometer();

            // Add the accelerometer event handler to the accelerometer sensor.
            accelSensor.ReadingChanged +=
                new EventHandler<AccelerometerReadingEventArgs>(AccelerometerReadingChanged);

            startAccel();

            //renderTarget = new RenderTarget2D(GraphicsDevice, screenWidth, screenHeight, false, 
            //                                    SurfaceFormat.Color, DepthFormat.Depth16);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                handleBackPressed();

            updateTouchEvent();

            if (!menuEnabled && !levelFinished)
                levelTime += gameTime.ElapsedGameTime.Milliseconds;

			if(menuEnabled)
                menu.handleSensorEvent(accelReading.X, accelReading.Y, accelReading.Z);
			else if(!levelFinished)
                maze.handleSensorEvent(accelReading.X, accelReading.Y, accelReading.Z, gameTime.ElapsedGameTime.Milliseconds);
			else
                highScoreScreen.handleSensorEvent(accelReading.X, accelReading.Y, accelReading.Z, gameTime.ElapsedGameTime.Milliseconds);

            if(menuEnabled)
			    menu.update(gameTime.ElapsedGameTime.Milliseconds);
		    else if(!levelFinished)
                maze.update(gameTime.ElapsedGameTime.Milliseconds);
		    else
                highScoreScreen.update(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        public void updateTouchEvent()
        {
            // Process touch events
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if ((tl.State == TouchLocationState.Pressed))
                {
                    if (menuEnabled)
                        menu.handleTouchEvent((int)tl.Position.X, (int)tl.Position.Y);
                    else if (!levelFinished)
                        maze.handleTouchEvent((int)tl.Position.X, (int)tl.Position.Y);
                    else
                        highScoreScreen.handleTouchEvent((int)tl.Position.X, (int)tl.Position.Y);

                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(renderTarget);

            if(!menuEnabled)
                GraphicsDevice.Clear(Color.White);
            else
                GraphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin();            
            if (menuEnabled)
                menu.draw(spriteBatch);
            else if (!levelFinished)
                maze.draw(spriteBatch);
            else
                highScoreScreen.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);

            /*GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(renderTarget, new Vector2(240, 400), null, Color.White,
                                MathHelper.PiOver2, new Vector2(400,240), 1f, SpriteEffects.None, 0);
            spriteBatch.End();*/
        }

        public void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            accelReading.X = (float)e.X * -9.8f;
            accelReading.Y = (float)e.Y * -9.8f;
            accelReading.Z = (float)e.Z * 9.8f;
        }

        public void startAccel()
        {
            // Start the accelerometer
            try
            {
                accelSensor.Start();
                accelActive = true;
            }
            catch (AccelerometerFailedException e)
            {
                // the accelerometer couldn't be started.  No fun!
                Console.WriteLine(e.ToString());
                accelActive = false;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.ToString());
                // This exception is thrown in the emulator-which doesn't support an accelerometer.
                accelActive = false;
            }
        }

        public void stopAccel()
        {
            // Stop the accelerometer if it's active.
            if (accelActive)
            {
                try
                {
                    accelSensor.Stop();
                    accelActive = false;
                }
                catch (AccelerometerFailedException e)
                {
                    Console.WriteLine(e.ToString());
                    // the accelerometer couldn't be stopped now.
                }
            }
        }

        public void handleLevelFinished()
        {
            //long levelTime = (System.currentTimeMillis() - levelTime);// / 1000;
            highScoreScreen = new HighScoreScreen(screenWidth, screenHeight, levelTime, this);
            levelFinished = true;
        }

        public void handleBackPressed()
        {
            menuEnabled = !menuEnabled;
        }

        public void closeGame()
        {
            Exit();
        }

        public void newGame()
        {
            //maze.resetMaze();
            maze.loadRandomMap();
            levelTime = 0;
            hideMenu();
            levelFinished = false;
        }

        public void resetMap()
        {
            maze.resetMaze();
            levelTime = 0;
            hideMenu();
            levelFinished = false;
        }

        public void hideMenu()
        {
            menuEnabled = false;
        }

        public int getMapID()
        {
            return maze.getMapID();
        }
    }
}
