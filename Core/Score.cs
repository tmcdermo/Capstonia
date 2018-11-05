using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Capstonia.Core
{
    public class Score
    {
        GameManager game;

        public Score(GameManager game)
        {
            this.game = game;
        }
       
        public void Draw(SpriteBatch spriteBatch)
        {
            var positionIcon = new Vector2(450, 690); 
            spriteBatch.Draw(game.chest, positionIcon, null, Color.White, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);

            var positionValue = new Vector2(550, 710);
            spriteBatch.DrawString(game.mainFont, game.Player.Glory.ToString(), positionValue, Color.White, 0f, Vector2.Zero, 3.0f, SpriteEffects.None, 0f);
        }
    }
}
