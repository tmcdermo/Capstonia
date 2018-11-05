using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Capstonia.Core
{
    public class MapLevel
    {
        GameManager game;

        public MapLevel(GameManager game)
        {
            this.game = game;
        }
       
        public void Draw(SpriteBatch spriteBatch)
        {
            var positionIcon = new Vector2(475, 775); 
            spriteBatch.Draw(game.exit, positionIcon, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            var positionValue = new Vector2(550, 774);
            spriteBatch.DrawString(game.mainFont, game.mapLevel.ToString(), positionValue, Color.White, 0f, Vector2.Zero, 3.0f, SpriteEffects.None, 0f);
        }
    }
}
