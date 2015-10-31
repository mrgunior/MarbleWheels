using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleWheels
{
    interface InputController
    {
        bool Quit
        {
            get;
        }

        Vector2 marbleWheelsControl(KeyboardState KeyState);

        Vector2 marbleWheelsMovement();

        int currentWeaponSelection();

        bool shootMarbleAmmo
        {
            get;
        }
    }
}
