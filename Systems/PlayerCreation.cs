using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Capstonia.Core;

namespace Capstonia.Systems
{
    public class PlayerCreation
    {
        private GameManager game;
        private KeyboardStringReader ksr;

        public PlayerCreation(GameManager game)
        {
            this.game = game;

            ksr = new KeyboardStringReader();

        }

        public void Update()
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.state = GameState.MainMenu;

            ksr.UpdateInput();

            if (ksr.IsFinished)
            {
                game.Player.Name = ksr.TextString;
                game.state = GameState.GamePlay;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int xOffset = 100;
            int yOffset = 100;

            spriteBatch.DrawString(game.mainFont, "What is your name, Adventurer?", new Vector2(xOffset, yOffset), Color.White);

            spriteBatch.DrawString(game.mainFont, ksr.TextString, new Vector2(xOffset, yOffset + 50), Color.White);

            
        }
    }
}
