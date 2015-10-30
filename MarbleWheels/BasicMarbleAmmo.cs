using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


/*
*   Adapter pattern
*   Generally the adapter pattern transforms one 
*   interface into another, but it can simply wrap the behavior to isolate your 
*   class from the underlying implementation
*/
namespace MarbleWheels
{
    class BasicMarbleAmmo : MarbleAmmo
    {
        public BasicMarbleAmmo(ContentManager content) : base(content) 
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
