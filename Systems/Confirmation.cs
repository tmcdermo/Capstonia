using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Capstonia.Core;

namespace Capstonia.Systems
{
    public class Confirmation
    {
        private GameManager game;

        public Confirmation(GameManager game)
        {
            this.game = game;
        }

        public void Update()
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                game.Leaderboard.CloseFile();
                game.Exit();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                game.state = GameState.GamePlay;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int xOffset; // horizontal offset
            int yOffset; // vertical offset

            // draw avatar
            xOffset = 30;
            yOffset = 400;
            
            spriteBatch.DrawString(game.pressStart2PFont, "Are you sure you wish to quit?  (Y)es or (N)o", new Vector2(xOffset, yOffset), Color.White);

        }
    }
}
