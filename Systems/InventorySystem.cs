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
        private readonly int maxItems = 9;
        private int currentItems = 0;

        //coordinates for each slot on the inventory outline
        private Vector2[] coords =
        {
            new Vector2(670,50),
            new Vector2(779,50),
            new Vector2(888,50),
            new Vector2(670,100),
            new Vector2(779,100),
            new Vector2(888,100),
            new Vector2(670,150),
            new Vector2(779,150),
            new Vector2(888,150),
        };

        //coordinates for each slot on the inventory quantity outline
        private Vector2[] quantityCoords =
        {
            new Vector2(670 + 50,50 + 5),
            new Vector2(779 + 50,50 + 5),
            new Vector2(888 + 50,50 + 5),
            new Vector2(670 + 50,100 + 5),
            new Vector2(779 + 50,100 + 5),
            new Vector2(888 + 50,100 + 5),
            new Vector2(670 + 50,150 + 5),
            new Vector2(779 + 50,150 + 5),
            new Vector2(888 + 50,150 + 5),
        };


        // InventorySystem()
        // DESC:    Constructor.
        // PARAMS:  GameManager object.
        // RETURNS: None.
        public InventorySystem(GameManager game)
        {
            this.game = game;
            Inventory = new List<Item>();

        }

        
        // AddItem()
        // DESC:    Adds item to the inventory.
        // PARAMS:  Item object.
        // RETURNS: None.
        public void AddItem(Item name)
        {
            //bool to help find item in list
            bool isFound = false;

            //Cycle through inventory and look for names that match the name parameter
            foreach (Item thing in Inventory)
            {
                if (thing == name)     //Check if item is in inventory and increase quantity
                {
                    isFound = true;
                    if(thing.CurrentStack != thing.MaxStack)    //add to current stack if there is still room
                    {
                        thing.CurrentStack++;
                        Inventory[currentItems - 1].Broadcast();
                    }
                    else
                    {
                        //Add item if inventory does not contain the item and inventory is not at max capacity
                        if (currentItems == maxItems)
                        {
                            game.Messages.AddMessage("Inventory full! Cannot carry any more items");
                        }
                        else
                        {
                            Inventory.Add(name);
                            currentItems++;
                            Inventory[currentItems - 1].CurrentStack++;
                            Inventory[currentItems - 1].Broadcast();
                        }
                    }
                    break;
                }
            }


            //Add item if inventory does not contain the item and inventory is not at max capacity
            if (isFound == false)
            {
                if(currentItems == maxItems)
                {
                    game.Messages.AddMessage("Inventory full! Cannot carry any more items");
                }
                else
                {
                    Inventory.Add(name);
                    currentItems++;
                    Inventory[currentItems - 1].CurrentStack++;
                    Inventory[currentItems - 1].Broadcast();
                }
            }
        }

        //TODO - Final Item in inventory not being removed
        // RemoveItem()
        // DESC:    Removes item to the inventory.
        // PARAMS:  Item object.
        // RETURNS: None.
        public void RemoveItem(Item name)
        {
            int i;

            //Cycle through inventory and look for names that match the name parameter
            //For loop rather than foreach so that we can use the RemoveAt() member function for the List
            for (i = 0; i < currentItems; i++)
            {
                //Check if the passed in item parameter matches some item in the inventory
                if (name == Inventory[i])                   
                {
                    //Decrement currentStack for the item and remove its stats from the player
                    Inventory[i].CurrentStack--;
                    Inventory[i].RemoveStat();

                    //Check to see if current stack is 0 after being decremented
                    if (Inventory[i].CurrentStack == 0)
                    {
                        //Remove item at that current inventory slot and -1 total items in inventory
                        Inventory.RemoveAt(i);
                        currentItems--;
                    }
                    break;
                }
            }
            

            /*
            //Go through all items and decrease counter then remove if 0
            foreach(Item thing in Inventory)
            {
                if(thing == name)
                {
                    thing.CurrentStack--;
                    thing.RemoveStat();

                    if (thing.CurrentStack == 0)
                    {
                        //Inventory[i].RemoveStat();
                        Inventory.RemoveAt(currentItems - 1);
                        currentItems--;
                    }
                    break;
                }
            }
            */
        }

        // useItem()
        // DESC:    attempts to use item in inventory slot
        // PARAMS:  slot number.
        // RETURNS: None.
        public void UseItem(int slot)
        {
            //Ensure there is an item in that slot
            if (0 < slot && slot < currentItems)
            {
                int index = slot - 1;
                //Inventory[index].Broadcast();
                Inventory[index].UseItem();
                RemoveItem(Inventory[index]);   
            }
            //TODO - Else print out message saying nothing in that slot
        }

        // Draw()
        // DESC:    Draws the contents of the inventory to the screen
        // PARAMS:  None.
        // RETURNS: None.

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw our skeleton //
            spriteBatch.Draw(game.Outline, new Vector2(672, 1), Color.White);
            spriteBatch.DrawString(game.mainFont, "INVENTORY", new Vector2(795, 15), Color.White);
            int index = 0; // used for accessing coordinates
            foreach (Item things in Inventory)
            {
                switch (things.Name)
                {
                    case "Armor":
                        spriteBatch.Draw(game.armor, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.CurrentStack, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Food":
                        spriteBatch.Draw(game.food, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.CurrentStack, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Weapon":
                        spriteBatch.Draw(game.weapon, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.CurrentStack, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Book":
                        spriteBatch.Draw(game.book, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.CurrentStack, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Potion":
                        spriteBatch.Draw(game.potion, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.CurrentStack, quantityCoords[index], Color.White);
                        index++;
                        break;

                }
            }
        }
    }
}
