using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MarbleWheels
{
    interface Weapon<Ammunition>
    {
        List<Ammunition> newAmmo();
        void shootMarble();
        int ammoAmountToTakeOff();
        void Update(float deltaTime, Vector2 marbleWheelsPosition, Texture2D marbleAmmoAppereance);
    }
}
