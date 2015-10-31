using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MarbleWheels
{
    //Generic interface
    //It is often useful to define interfaces either for generic 
    //collection classes, or for the generic classes that represent items in the collection.
    abstract class MarbleAmmo : Weapon<Entity>
    {
        protected List<Entity> ammoSack = new List<Entity>();
        protected int ammoCount;
        protected ContentManager Content;
        public MarbleAmmo(ContentManager content)
        {
            Content = content;

        }

        public int ammoAmountToTakeOff()
        {
            return ammoCount;
        }

        public List<Entity> newAmmo()
        {
            return ammoSack;
        }

        public void shootMarble()
        {
             addMarbleShots();
        }


        protected abstract void addMarbleShots();

        protected Vector2 marbleWheelsPosition;
        protected Texture2D marbleAmmoAppearance;
        protected float deltaTime;
        public void Update(float deltaTime, Vector2 marbleWheelsPosition, Texture2D marbleAmmoAppearance)
        {
            this.deltaTime = deltaTime;
            this.marbleWheelsPosition = marbleWheelsPosition;
            this.marbleAmmoAppearance = marbleAmmoAppearance;
        }
    }
}
