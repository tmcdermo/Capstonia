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
            int xOffset = 50;
            int yOffset = 50;

            //Set vectors to draw sprites in the corners of the screen
            Vector2 topLeft = new Vector2(0, 0);
            Vector2 topRight = new Vector2(game.graphics.PreferredBackBufferWidth - 48, 0);
            Vector2 bottomLeft = new Vector2(0, game.graphics.PreferredBackBufferHeight - 48);
            Vector2 bottomRight = new Vector2(game.graphics.PreferredBackBufferWidth - 48, game.graphics.PreferredBackBufferHeight - 48);

            //Place sprites of the player in the corners of the screen
            spriteBatch.Draw(game.Player.Sprite, topLeft, Color.White);
            spriteBatch.Draw(game.Player.Sprite, topRight, Color.White);
            spriteBatch.Draw(game.Player.Sprite, bottomLeft, Color.White);
            spriteBatch.Draw(game.Player.Sprite, bottomRight, Color.White);

            spriteBatch.DrawString(game.pressStart2PFont, "What is your name, Adventurer?", new Vector2(xOffset, yOffset), Color.White);

            spriteBatch.DrawString(game.pressStart2PFont, ksr.TextString, new Vector2(xOffset, yOffset + 50), Color.White);

            
        }
    }
}
