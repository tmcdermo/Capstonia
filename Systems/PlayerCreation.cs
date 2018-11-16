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
            {
                game.MenuDown.Play();
                game.state = GameState.MainMenu;
            }                

            ksr.UpdateInput();

            // If string legnth is greater than 30 or <enter>
            // has been pressed, continue.
            if (ksr.IsFinished || ksr.TextString.Length == 30) 
            {
                game.Player.Name = ksr.TextString;
                ksr.TextString = "";
                ksr.IsFinished = false;
                game.state = GameState.GamePlay;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int xOffset; // horizontal offset
            int yOffset; // vertical offset

            // draw avatar
            xOffset = 285;
            yOffset = 230;
            spriteBatch.Draw(game.darkKnightLarge, new Vector2(xOffset, yOffset), Color.White);

            // place text above avatar
            xOffset = 200;
            yOffset = 150;
            spriteBatch.DrawString(game.pressStart2PFont, "What is your name, Adventurer?", new Vector2(xOffset, yOffset), Color.White);

            // place player's name below avatar
            int centerOffset = 500;
            yOffset = 680;
            int numChars = ksr.TextString.Length;
            spriteBatch.DrawString(game.pressStart2PFont, ksr.TextString, new Vector2((int)(centerOffset - (10.5 * numChars)), yOffset), Color.White);

        }
    }
}
