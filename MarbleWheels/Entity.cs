using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Entity
{
    public Texture2D textureAppearance;
    public Vector2 position;
    
    public Entity(Texture2D texture)
	{
        textureAppearance = texture;
    }

    public void textureVisible()
    {
        textureAppearance = null;
    }
    
    public void draw(SpriteBatch spriteBatch)
    {  
        spriteBatch.Draw(textureAppearance, position, Color.White);
    }
}
