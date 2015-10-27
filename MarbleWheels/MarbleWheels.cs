using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

//Name changed
namespace MarbleWheels
{
    public class MarbleWheels : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Song shootSoundFX;
        private SpriteFont headerFont;
        private int ammoCount = 100;

        private Vector2 marbleWheelsPosition, jumpHeight, jumpSpeed, 
                        ammoSackBarPosition, healthBarPosition, scoreBarPosition;
        private Vector2 spriteDirection = Vector2.Zero;
        private Texture2D marbleWheels, background, fallingObstacles, 
                          marbleAmmo, ammoSackBar, healthBar, scoreBar;
        private int gameLogicScriptPC = 0, rndNumberLine1, iLine1, rndNumberLine5, iLine5;
        private float speed, timeToWaitLine3, timeToWaitLine4, timeToWaitLine8, timeToWaitLine7;

        private Random randomGenerator = new Random();
        private List<Vector2> fallingObstaclesPositions = new List<Vector2>();
        private List<Vector2> marbleAmmoPositions = new List<Vector2>();

        private KeyboardState oldState, newState;

        /**
        *
        *
        */
        public MarbleWheels()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /**
        *
        *
        */
        protected override void Initialize()
        {
            speed = 300;
            base.Initialize();
        }

        /**
        *
        *
        */
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Positions of the objects
            marbleWheelsPosition = new Vector2(100, 350);
            ammoSackBarPosition = new Vector2(680, 10);
            healthBarPosition = new Vector2(560, 10);
            scoreBarPosition = new Vector2(40, 10);
 
            marbleWheels = Content.Load<Texture2D>("sprites/character/marbleWheels.png");
            background = Content.Load<Texture2D>("sprites/background/background.jpg");
            fallingObstacles = Content.Load<Texture2D>("sprites/Obstacles/hammer.png");
            marbleAmmo = Content.Load<Texture2D>("sprites/ammo/marbleBulletBasic.png");
            ammoSackBar = Content.Load<Texture2D>("sprites/ammo/ammoSackBar.png");
            healthBar = Content.Load<Texture2D>("sprites/health/healthBar.png");
            scoreBar = Content.Load<Texture2D>("sprites/scoreBar/scoreBar.png");
            headerFont = Content.Load<SpriteFont>("headerFont");
            shootSoundFX = Content.Load<Song>(@"sound/shootSoundFX");
        }

        /**
        *
        *
        */
        protected override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            //Movement in frames. This is not hardware depended but frame depended. 
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //reset the value. This makes sure the player waits on input instead continueing on the pressed button.
            spriteDirection = Vector2.Zero;

            var newMarbleAmmoPositions =
            (
                from marbleAmmoPosition in marbleAmmoPositions
                let colliders =
                from fallingObstaclePosition in fallingObstaclesPositions
                where Vector2.Distance(marbleAmmoPosition, fallingObstaclePosition) < 20.0f

                select fallingObstaclePosition
                where marbleAmmoPosition.X > 50.0f &&
                      marbleAmmoPosition.X < 600.0f &&
                      marbleAmmoPosition.Y > 50.0f &&
                      marbleAmmoPosition.Y < 600.0f &&
                      colliders.Count() == 0

                select marbleAmmoPosition - Vector2.UnitY * 500.0f * deltaTime).ToList();

            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    MediaPlayer.Play(shootSoundFX);

                    ammoCount--;
                    newMarbleAmmoPositions.Add(marbleWheelsPosition);
                }
            }
            else if (oldState.IsKeyDown(Keys.Space))
            {
                // Key was down last update, but not down now, so
                // it has just been released.
            }

            // Update saved state.
            oldState = newState;

            var newfallingObstaclesPositions =
              (
                from fallingObstaclesPosition in fallingObstaclesPositions
                let colliders =
                from marbleAmmoPosition in marbleAmmoPositions
                where Vector2.Distance(marbleAmmoPosition, fallingObstaclesPosition) < 40.0f

                select marbleAmmoPosition
                where fallingObstaclesPosition.X > 50.0f &&
                      fallingObstaclesPosition.X < 600.0f &&
                      fallingObstaclesPosition.Y > 50.0f &&
                      fallingObstaclesPosition.Y < 300.0f &&
                      colliders.Count() == 0

                select fallingObstaclesPosition + Vector2.UnitY * 200.0f * deltaTime).ToList();

            HandleKeyboardInput(Keyboard.GetState());

            spriteDirection *= speed;

            marbleWheelsPosition += (spriteDirection * deltaTime);

            switch (gameLogicScriptPC)
            {
                case 0:
                    if (true)
                    {
                        gameLogicScriptPC = 1;
                        iLine1 = 1;
                        rndNumberLine1 = randomGenerator.Next(20, 60);
                    }
                    break;
                case 1:
                    if (iLine1 <= rndNumberLine1)
                    {
                        gameLogicScriptPC = 2;
                    }

                    else
                    {
                        gameLogicScriptPC = 4;
                        timeToWaitLine4 = (float)(randomGenerator.NextDouble() * 2.0 + 5.0);
                    }
                    break;
                case 2:
                    newfallingObstaclesPositions.Add(new Vector2((float)(randomGenerator.NextDouble() * 500.0 + 51.0), 51.0f));
                    gameLogicScriptPC = 3;
                    timeToWaitLine3 = (float)(randomGenerator.NextDouble() * 0.2 + 0.1);
                    break;
                case 3:
                    timeToWaitLine3 -= deltaTime;

                    if (timeToWaitLine3 > 0.0f)
                    {
                        gameLogicScriptPC = 3;
                    }

                    else
                    {
                        gameLogicScriptPC = 1;
                        iLine1++;
                    }
                    break;
                case 4:
                    timeToWaitLine4 -= deltaTime;

                    if (timeToWaitLine4 > 0.0f)
                    {
                        gameLogicScriptPC = 4;
                    }

                    else
                    {
                        gameLogicScriptPC = 5;
                        iLine5 = 1;
                        rndNumberLine5 = randomGenerator.Next(10, 20);
                    }
                    break;
                case 5:
                    if (iLine5 <= rndNumberLine5)
                    {
                        gameLogicScriptPC = 6;
                    }

                    else
                    {
                        gameLogicScriptPC = 8;
                        timeToWaitLine8 = (float)(randomGenerator.NextDouble() * 2.0 + 5.0);
                    }
                    break;
                case 6:
                    newfallingObstaclesPositions.Add(new Vector2((float)(randomGenerator.NextDouble() * 500.0 + 51.0), 51.0f));
                    gameLogicScriptPC = 7;
                    timeToWaitLine7 = (float)(randomGenerator.NextDouble() * 1.5 + 0.5);
                    break;
                case 7:
                    timeToWaitLine7 -= deltaTime;

                    if (timeToWaitLine7 > 0)
                    {
                        gameLogicScriptPC = 7;
                    }

                    else
                    {
                        gameLogicScriptPC = 5;
                        iLine5++;
                    }
                    break;
                case 8:
                    timeToWaitLine8 -= deltaTime;

                    if (timeToWaitLine8 > 0.0f)
                    {
                        gameLogicScriptPC = 8;
                    }
                    else
                    {
                        gameLogicScriptPC = 0;
                    }
                    break;

                default:
                    break;
            }

            marbleAmmoPositions = newMarbleAmmoPositions;
            fallingObstaclesPositions = newfallingObstaclesPositions;

            base.Update(gameTime);
        }

        /**
        *
        *
        */
        protected void HandleKeyboardInput(KeyboardState KeyState)
        {
            //exit the game if the ESC button is pressed
            if (KeyState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //move the player right if the right button is pressed.
            if (KeyState.IsKeyDown(Keys.Right))
            {
                spriteDirection += new Vector2(1, 0);
            }

            //player goes left if the left button is pressed
            if (KeyState.IsKeyDown(Keys.Left))
            {
                spriteDirection += new Vector2(-1, 0);
            }
        }

        /**
        *
        *
        */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(marbleWheels, marbleWheelsPosition, Color.White);
            spriteBatch.Draw(ammoSackBar, ammoSackBarPosition, Color.White);
            spriteBatch.Draw(healthBar, healthBarPosition, Color.White);
            spriteBatch.Draw(scoreBar, scoreBarPosition, Color.White);
            spriteBatch.DrawString(headerFont, "" + ammoCount, new Vector2(740, 35), Color.Black);
            spriteBatch.DrawString(headerFont, "%", new Vector2(640, 35), Color.Black);

            foreach (var marbleAmmoPosition in marbleAmmoPositions)
            {
                spriteBatch.Draw(marbleAmmo, marbleAmmoPosition, Color.White);
            }

            foreach (var fallingObstaclesPosition in fallingObstaclesPositions)
            {
                spriteBatch.Draw(fallingObstacles, fallingObstaclesPosition, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
