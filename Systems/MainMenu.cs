using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Capstonia.Core;

namespace Capstonia.Systems
{
    public class MainMenu
    {
        private GameManager game;
        private List<string> Options;

        public MainMenu(GameManager game)
        {
            this.game = game;

            Options = new List<string>();

            Options.Add("Dungeons of Capstonia");
            Options.Add(Environment.NewLine);
            Options.Add("Choose an Option, Adventurer!");
            Options.Add("1) Play Game!");
            Options.Add("2) Game Instructions");
            Options.Add("3) View Leaderboard");
            Options.Add("4) Credits");
            Options.Add("5) Exit");
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.Leaderboard.CloseFile();
                game.Exit();
            }                
            else if (Keyboard.GetState().IsKeyDown(Keys.D1) || Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                game.state = GameState.PlayerCreation;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2) || Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                game.state = GameState.Instructions;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3) || Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                game.state = GameState.Leaderboard;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D4) || Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                game.state = GameState.Credits;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5) || Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            {
                game.Leaderboard.CloseFile();
                game.Exit();
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

            foreach (string message in Options)
            {
                spriteBatch.DrawString(game.mainFont, message, new Vector2(xOffset, yOffset), Color.White);
                yOffset += 18;
            }
            
        }
    }
}
