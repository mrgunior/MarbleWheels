﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MarbleWheels
{
    /*
    * Adapter pattern
    *   Generally the adapter pattern transforms one 
    *   interface into another, but it can simply wrap the behavior to isolate your 
    *   class from the underlying implementation
    */
    class MediumMarbleAmmo : MarbleAmmo
    {
        public MediumMarbleAmmo(ContentManager content) : base(content) 
        {

        }

        protected override void addMarbleShots()
        {
            Entity ammo = new Entity(marbleAmmoAppearance);
            ammo.position = marbleWheelsPosition;
            ammoSack.Add(ammo);
        }
    }
}