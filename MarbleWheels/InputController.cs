using Microsoft.Xna.Framework;
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
        Vector2 marbleWheelsMovement();

        bool shootMarbleAmmo
        {
            get;
        }
    }
}
