using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Capstonia.Core;

namespace Capstonia.Systems
{
    public class Instructions
    {
        private GameManager game;
        private List<string> Instruct;

        public Instructions(GameManager game)
        {
            this.game = game;

            Instruct = new List<string>();

            Instruct.Add("Welcome to Dungeons of Capstonia!");
            Instruct.Add("Press <SPACE> to go to the Main Menu");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Dungeons of Capstonia is a roguelike game that uses random generation to create a different layout every time you play");
            Instruct.Add("In this twist on a classic tale of adventure, instead of a Hero, you play as a Villain");
            Instruct.Add("You will discover many perils in your adventure");
            Instruct.Add("Locate items throughout the dungeon to help you on your journey");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Objective: Dive deep into the dungeon and collect as much Glory as possible");
            Instruct.Add("You collect Glory by defeating monsters and picking up skulls located throughout the dungeon");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Move: Arrow keys or NumPad");
            Instruct.Add("Using the NumPad allows you to move diagonally");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Attack: Simply attempt to move into the same space as a monster and you will automatically attack the monster with your equipped weapon");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Use item: 1-9 (row of numbers above letters)");
            Instruct.Add("Number corresponds to placement in inventory");
            Instruct.Add("Top left slot is 1, top middle slot is 2, etc.");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Check item description: F#");
            Instruct.Add("This shows info such as how much health a potion will restore or how much strength a weapon has");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Drop item: Hold d + #");
            Instruct.Add(Environment.NewLine);
            Instruct.Add("Move to next level: 5 on the NumPad or .");
            Instruct.Add(Environment.NewLine);

        }


        public void Update()
        {
            // get current keyboard state
            game.currentKeyboardState = Keyboard.GetState();

            if (game.currentKeyboardState.IsKeyUp(Keys.Space) && game.previousKeyboardState.IsKeyDown(Keys.Space))
            {
                game.MenuDown.Play();
                game.state = GameState.MainMenu;
            }


            // save current state to previous and get ready for next move
            game.previousKeyboardState = game.currentKeyboardState;
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

            foreach (string message in Instruct)
            {
                spriteBatch.DrawString(game.mainFont, message, new Vector2(xOffset, yOffset), Color.White);
                yOffset += 18;
            }
        }
    }
}
