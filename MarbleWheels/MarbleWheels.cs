using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using MarbleWheels.scripts;

namespace MarbleWheels
{
    /*
    The enum keyword is used to declare an enumeration, a distinct type that consists of a set of named constants called the enumerator list. 
    Usually it is best to define an enum directly within a namespace so that all classes in the namespace can access it with equal convenience. 
    However, an enum can also be nested within a class or struct.
    By default, the first enumerator has the value 0, and the value of each successive enumerator is increased by 1
    */
    //0, 1, 2, 3
    enum InstructionResult { Done, DoneAndCreateEnemyObject, Running, RunningAndCreateEnemyObject }

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
                        spriteDirection;

        private Texture2D marbleWheels, 
                          background, 
                          enemyObstacles, 
                          bma, 
                          mma,
                          ammoSackBar, 
                          healthBar, 
                          scoreBar;

        private int ammoAmount = 500, index;
        private float speed, deltaTime;

        private Random randomGenerator = new Random();

        static Random random = new Random();
        //child classes(repeat, for, wait, createEnemyObject) derived from parent class(Instruction)
        Instruction enemyObjectSpawnLogic =
          new Repeat(
                //start, end, input => expression
                new For(0, 10, i =>   
                        //Func < float > getTimeToWait
                        new Wait(() => i * 0.1f) +
                        new CreateEnemyObject()
                    ) +
                new Wait(() => random.Next(1, 5)) +
                //start, end, input => expression
                new For(0, 10, i =>
                        //Func < float > getTimeToWait
                        //Returns a random floating - point number that is greater than or equal to 0.0, and less than 1.0.
                        new Wait(() => (float)random.NextDouble() * 1.0f + 0.2f) +
                        new CreateEnemyObject()
                    ) +
                //Func < float > getTimeToWait
                new Wait(() => random.Next(2, 3))
              );

        //generic List collection
        //generic collection List<Entity>, which is referred to as List of Entity
        private List<Entity> enemyObjects = new List<Entity>();
        private List<Entity> ammo = new List<Entity>();
        private List<Texture2D> textureList;

        //Each weapon is unique in texture and power. Nested list
        private List<Weapon<Entity>> weaponList;

        private KeyboardController keyboardInput = new KeyboardController();
        private int score = 0, marbleDamage = 100;
        private bool takeOut;

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
            textureList = new List<Texture2D> { bma, mma};
        }

        /**
        *
        *
        */
        protected override void Update(GameTime gameTime)
        {
            //Movement in frames. This is not hardware dependent but frame dependent. 
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Constantly send current input
            keyboardInput.marbleWheelsControl(Keyboard.GetState());

            spriteDirection = Vector2.Zero; //reset the value. This makes sure the player waits on input instead continueing on the pressed button.
            spriteDirection = keyboardInput.marbleWheelsMovement();  //update to last current position
            marbleWheelsPosition += spriteDirection * speed * deltaTime;  //update it to currrent position
     
            if(keyboardInput.shootMarbleAmmo)
            {
                //update which ammo has been selected
                index = keyboardInput.currentWeaponSelection();

                //The first thing that happens = we need to update the object with the current information
                weaponList[index].Update(deltaTime, marbleWheelsPosition, textureList[index]);
                weaponList[index].shootMarble();
                ammo = weaponList[index].newAmmo();
                ammoAmount -= weaponList[index].ammoAmountToTakeOff();
            }

            //constructor, Execute in order
            switch (enemyObjectSpawnLogic.Execute(deltaTime))
            {
                case InstructionResult.DoneAndCreateEnemyObject:
                    enemyObjectCreation();
                    break;

                case InstructionResult.RunningAndCreateEnemyObject:
                    enemyObjectCreation();
                    break;
            }

            base.Update(gameTime);
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
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
                spriteBatch.Draw(marbleWheels, marbleWheelsPosition, Color.White);
                spriteBatch.Draw(ammoSackBar, ammoSackBarPosition, Color.White);
                spriteBatch.Draw(healthBar, healthBarPosition, Color.White);
                spriteBatch.Draw(scoreBar, scoreBarPosition, Color.White);
                spriteBatch.DrawString(headerFont, "" + ammoAmount, new Vector2(740, 35), Color.Black);
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
