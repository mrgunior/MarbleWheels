using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace MarbleWheels
{
    public class MarbleWheels : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont headerFont;
        private Song shootSoundFX;

        private Vector2 marbleWheelsPosition, 
                        ammoSackBarPosition, 
                        healthBarPosition, 
                        scoreBarPosition, 
                        spriteDirection,  
                        topEnemySpawner;

        private Texture2D marbleWheels, 
                          background, 
                          enemyObstacles, 
                          bma, 
                          mma,
                          ammoSackBar, 
                          healthBar, 
                          scoreBar;

        private int gameLogicScriptPC = 0, rndNumberLine1, iLine1, rndNumberLine5, iLine5, ammoCount = 500;
        private float speed, timeToWaitLine3, timeToWaitLine4, timeToWaitLine8, timeToWaitLine7, deltaTime;

        private Random randomGenerator = new Random();
        private List<Entity> enemyObjects = new List<Entity>();
        private List<Entity> ammo = new List<Entity>();
        private List<Weapon<Entity>> weaponList;

        private KeyboardState oldState;
        private int score = 0;
        private int marbleDamage = 100;
        private bool takeOut;
        private bool mediumAmmoSelected;

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
 
            //Texture location
            marbleWheels = Content.Load<Texture2D>("sprites/character/marbleWheels.png");
            background = Content.Load<Texture2D>("sprites/background/background.jpg");
            enemyObstacles = Content.Load<Texture2D>("sprites/Obstacles/hammer.png");
            bma = Content.Load<Texture2D>("sprites/ammo/basicMarbleAmmo.png");
            mma = Content.Load<Texture2D>("sprites/ammo/mediumMarbleAmmo.png");
            ammoSackBar = Content.Load<Texture2D>("sprites/ammo/ammoSackBar.png");
            healthBar = Content.Load<Texture2D>("sprites/health/healthBar.png");
            scoreBar = Content.Load<Texture2D>("sprites/scoreBar/scoreBar.png");
            shootSoundFX = Content.Load<Song>(@"sound/shootSoundFX");
            headerFont = Content.Load<SpriteFont>("headerFont");

            //Create weaponList component
            weaponList = new List<Weapon<Entity>> { new BasicMarbleAmmo(Content), new MediumMarbleAmmo(Content) };
        }

        /**
        *
        *
        */
        protected override void Update(GameTime gameTime)
        {

            //Movement in frames. This is not hardware depended but frame depended. 
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //reset the value. This makes sure the player waits on input instead continueing on the pressed button.
            spriteDirection = Vector2.Zero;

            //handle the keyboard inputs
            HandleKeyboardInput(Keyboard.GetState());

            //Spawn them
            spawnEnemyObject();

            base.Update(gameTime);
        }

        /*
        * Factory Pattern
        * An important facet of system design is the manner in which objects are created. Although far more time is 
        * often spent considering the object model and instance interaction, if this simple design aspect is ignored it will adversely impact the entire system. 
        * Thus, it is not only important what an object does or what it models, but also in what manner it was created.
        */
        private void spawnEnemyObject()
        {
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
                        enemyObjectCreation();
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
                        enemyObjectCreation();
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
        }

        /*
        *
        *
        */
        private void enemyObjectCreation()
        {
            Vector2 randomSpawningLocation = Vector2.Zero;

            randomSpawningLocation = new Vector2(randomGenerator.Next(50,600),randomGenerator.Next(50, 50));

            //Creating an Object of the class Entity. Afterwards adding the spawning position to the object.
            Entity enemyObject = new Entity(enemyObstacles);
            enemyObject.position = randomSpawningLocation;
            enemyObjects.Add(enemyObject);
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

            if(KeyState.IsKeyDown(Keys.NumPad1))
            {
                mediumAmmoSelected = true;
            }

            if (KeyState.IsKeyDown(Keys.NumPad2))
            {
                mediumAmmoSelected = false;
            }
            // Is the SPACE key down?
            if (KeyState.IsKeyDown(Keys.Space))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Space))
                {

                    if(mediumAmmoSelected == true)
                    {
                        //The first thing that happens = we need to update the object with the current information i.e
                        weaponList[1].Update(deltaTime, marbleWheelsPosition, mma);
                        weaponList[1].shootMarble();
                        ammo = weaponList[1].newAmmo();
                        ammoCount -= 2;
                    }

                    else
                    {   
                        weaponList[0].Update(deltaTime, marbleWheelsPosition, bma);
                        weaponList[0].shootMarble();
                        ammo = weaponList[0].newAmmo();
                        ammoCount--;
                    }
                    
                }
            }
            
            oldState = KeyState;
            spriteDirection *= speed;
            marbleWheelsPosition += (spriteDirection * deltaTime);
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
                spriteBatch.DrawString(headerFont, "" + marbleDamage, new Vector2(620, 35), Color.Black);
                spriteBatch.DrawString(headerFont, "" + score, new Vector2(120, 35), Color.Black);

                //draw the enenemyObject
                foreach(Entity eo in enemyObjects)
                {
                    eo.draw(spriteBatch);
                }

                //draw the ammo 
                foreach (Entity a in ammo)
                {
                    a.draw(spriteBatch);
                }

            //after drawing update their position and if they will still exist or not
            updateEntity();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /*
        *
        *
        */
        private void updateEntity()
        {
            //add the current direction to the position (1,0)
            //this makes the sprite move 
            foreach (Entity a in ammo)
            {
                a.position -= Vector2.UnitY * 250.0f * deltaTime;
            }

            //add the current direction to the position (1,0)
            foreach (Entity e in enemyObjects)
            {
                //this makes the sprite move
                e.position += Vector2.UnitY * 250.0f * deltaTime;

                //preparing the deletion of the objects in the list
                foreach (Entity a in ammo)
                {
                    //if the ammo distance is smaller then the enemy 
                    if (Vector2.Distance(a.position, e.position) < 40.0f)
                    {
                        takeOut = true;
                        score += 1;
                    }
                }
                //if the enemy is close to the marble
                if (Vector2.Distance(e.position, marbleWheelsPosition) < 40.0f)
                {
                    marbleDamage -= 1;
                }
            }

            //Deleting the objects after they have passed Y 50.f 
            for (int i = 0; i < ammo.Count; i++)
            {
                if (ammo[i].position.Y < 50.0f || takeOut == true)
                {
                    
                    ammo.RemoveAt(i);
                }
            }

            //Deleting the objects after they have passed Y 350.f 
            for (int i = 0; i < enemyObjects.Count; i++)
            {
                if (enemyObjects[i].position.Y > 350.0f || takeOut == true)
                {
                    takeOut = false;
                    enemyObjects.RemoveAt(i);
                }
            }
        }
    }
}
