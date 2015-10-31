using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MarbleWheels
{
    internal class KeyboardController : InputController
    {
        Vector2 marbleWheelsPosition;
        KeyboardState KeyState, oldState;
        private int weaponIndex;
        private bool shoot;

        public bool Quit
        {
            get
            {
                return KeyState.IsKeyDown(Keys.Escape);
            }
        }

        public bool shootMarbleAmmo
        {
            get
            {
               return shoot;
            }
        }

        public Vector2 marbleWheelsControl(KeyboardState KeyState)
        {
            marbleWheelsPosition = Vector2.Zero;

            //move the player right if the right button is pressed.
            if (KeyState.IsKeyDown(Keys.Right))
            {
                marbleWheelsPosition = new Vector2(1, 0);
            }

            //player goes left if the left button is pressed
            if (KeyState.IsKeyDown(Keys.Left))
            {
                marbleWheelsPosition = new Vector2(-1, 0);
            }

            if (KeyState.IsKeyDown(Keys.NumPad1))
            {
                weaponIndex = 1;
            }

            if (KeyState.IsKeyDown(Keys.NumPad2))
            {
                weaponIndex = 0;
            }

            // Is the SPACE key down?
            if (KeyState.IsKeyDown(Keys.Space))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    shoot = true;
                }

                else
                { 
                    shoot = false;
                }
            }

            oldState = KeyState;

            return marbleWheelsPosition;
        }

        public int currentWeaponSelection()
        {
            return weaponIndex;
        }

        public Vector2 marbleWheelsMovement()
        {
            return marbleWheelsPosition;
        }
    }
}