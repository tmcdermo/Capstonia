using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstonia.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capstonia.Systems
{
    // InventorySystem class
    // DESC:  Contains attributes and methods for the Inventory

    public class InventorySystem
    {
        private GameManager game;
        public readonly List<Item> Inventory;   //public because player needs to manipulate inventory
        private readonly int maxItems = 10;
        private int currentItems = 0;



        // InventorySystem()
        // DESC:    Constructor.
        // PARAMS:  GameManager object.
        // RETURNS: None.
        public InventorySystem(GameManager game)
        {
            Inventory = new List<Item>();
            this.game = game;
        }

        // AddItem()
        // DESC:    Adds item to the inventory.
        // PARAMS:  Item object.
        // RETURNS: None.
        public void AddItem(Item name)
        {
            Inventory.Add(name);
            currentItems++;
        }

        // RemoveItem()
        // DESC:    Removes item to the inventory.
        // PARAMS:  Item object.
        // RETURNS: None.
        public void RemoveItem(Item name)
        {
            int i;

            //Cycle through inventory and look for names that match the name parameter
            for (i = 0; i < maxItems; i++)
            {
                if (name.Name == Inventory[i].Name)
                {
                    Inventory.RemoveAt(i);
                    currentItems--;
                    break;
                }
            }
        }

        // Draw()
        // DESC:    Draws the contents of the inventory to the screen
        // PARAMS:  None.
        // RETURNS: None.
        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle screen, SpriteFont font)
        {
            //TODO - WHY DOESN'T THIS WORK???
            Vector2 displayVector = new Vector2(1200, 10);
            spriteBatch.DrawString(font, "Inventory", displayVector, Color.White);
            spriteBatch.Draw(texture, screen, Color.Black);
            displayVector.Y += 15;


            int i;
            for (i = 0; i < currentItems; i++)
            {
                //TODO - Draw name of object to screen
                //Output should look like "i. Inventory[i].Name"
                spriteBatch.DrawString(font, "i. Inventory[i].Name", displayVector, Color.White);
                displayVector.Y += 15;
            }
        }
    }
}
