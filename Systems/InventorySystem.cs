﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstonia.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capstonia.Systems
{
    //TupleList
    // Required to make our lives easier found this tutorial
    //https://whatheco.de/2011/11/03/list-of-tuples/
    // So we can create a 2D like structure for our inventory where we can keep track of how many items we have in each slot

    public class TupleList<T1,T2>: List<Tuple<T1, T2>>
    {
        public void Add(T1 item1, T2 item2)
        {
            Add(new Tuple<T1, T2>(item1, item2));
        }
    }



    // InventorySystem class
    // DESC:  Contains attributes and methods for the Inventory

    public class InventorySystem
    {
        private GameManager game;
       // public readonly List<Item> Inventory;   //public because player needs to manipulate inventory
        private readonly int maxItems = 9;
        private int currentItems = 0;

        //https://stackoverflow.com/questions/8002455/how-to-easily-initialize-a-list-of-tuples //
        public readonly TupleList<Item, int> Inventory;
        public readonly Queue<Item> potStack  = new Queue<Item>();
        public readonly Queue<Item> foodStack = new Queue<Item>();
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
            Inventory = new TupleList<Item, int>();

        }

        
        // AddItem()
        // DESC:    Adds item to the inventory.
        // PARAMS:  Item object.
        // RETURNS: None.
        public void AddItem(Item iType)
        {
            //bool to help find item in list

            //Cycle through inventory and look for names that match the name parameter
            
            bool isFound = false;
            for (int x = 0; x < Inventory.Count(); x++)
            {
                Item tmp;
                int count;
                
                if(Inventory[x].Item1.Name == iType.Name)
                {
                    isFound = true;
                    //if item exists check to see if we can still stack
                    if (Inventory[x].Item2 != iType.MaxStack)
                    {
                        count = Inventory[x].Item2;
                        tmp = Inventory[x].Item1;
                        count++;
                        Inventory[x] = Tuple.Create(tmp, count); // update  tuple value
                        //store the item to our array if its a potion//
                        if (tmp.Name == "Potion")
                        {
                            potStack.Enqueue(iType);
                        }
                        else if (tmp.Name == "Food")
                        {
                            foodStack.Enqueue(iType);
                        }
                    }
                    //if can't stack
                    else
                    {
                        if (currentItems == maxItems)
                        {
                            game.Messages.AddMessage("Inventory full! Cannot carry any more items");
                        }
                        else
                        {
                            Inventory.Add(Tuple.Create(iType, 1));
                            currentItems++;
                            if (iType.Name == "Potion")
                            {
                                potStack.Enqueue(iType);
                            }
                            else if (iType.Name == "Food")
                            {
                                foodStack.Enqueue(iType);
                            }
                            break;
                        }

                    }
                        
                    
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
                    Inventory.Add(Tuple.Create(iType, 1));
                    currentItems++;
                    if(iType.Name == "Potion")
                    {
                        potStack.Enqueue(iType);
                    }
                    else if(iType.Name == "Food")
                    {
                        foodStack.Enqueue(iType);
                    }
                }
            }
        }

        //TODO - Final Item in inventory not being removed
        // RemoveItem()
        // DESC:    Removes item to the inventory.
        // PARAMS:  Item object.
        // RETURNS: None.
        public void RemoveItem(Item iType)
        {
            //Cycle through inventory and look for names that match the name parameter
            //For loop rather than foreach so that we can use the RemoveAt() member function for the List


            //Go through all items and decrease counter then remove if 0
            for(int x = 0; x < Inventory.Count(); x++)
            {
                int tmpCount = Inventory[x].Item2;
                if(Inventory[x].Item1.Name == iType.Name)
                {
                    tmpCount--;
                    if (tmpCount <= 0)
                    {
                        Inventory.RemoveAt(x);
                        currentItems--;
                        break;
                    }
                    Item tmp = Inventory[x].Item1;
                    Inventory[x] = Tuple.Create(tmp, tmpCount);
                }

            }


        }

        // useItem()
        // DESC:    attempts to use item in inventory slot
        // PARAMS:  slot number.
        // RETURNS: None.
        public void UseItem(int slot)
        {
            //Ensure there is an item in that slot
            if (0 < slot && slot < currentItems + 1)
            {
                int index = slot - 1;
                //Inventory[index].Broadcast();
                if (Inventory[index].Item1.Name == "Potion")
                {
                    usePotion(index);
                    RemoveItem(Inventory[index].Item1);
                }
                else if(Inventory[index].Item1.Name == "Food")
                {
                    useFood(index);
                    RemoveItem(Inventory[index].Item1);
                }
                else
                {
                    Inventory[index].Item1.UseItem();
                    RemoveItem(Inventory[index].Item1);
                }
            }
            //TODO - Else print out message saying nothing in that slot
        }

        // usePotion()
        // DESC:    uses Potion and access the potStack Queue accordingly
        // PARAMS:  index #.
        // RETURNS: None.
        private void usePotion(int index)
        {
            Item uses = potStack.Dequeue();
            uses.UseItem();
        }
        // useFood()
        // DESC:    uses food and access the foodStack Queue accordingly
        // PARAMS:  index #.
        // RETURNS: None.
        private void useFood(int index)
        {
            Item uses = foodStack.Dequeue();
            uses.UseItem();
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
            foreach (Tuple<Item,int> things in Inventory)
            {
                switch (things.Item1.Name)
                {
                    case "Armor":
                        spriteBatch.Draw(game.armor, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.Item2, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Food":
                        spriteBatch.Draw(game.food, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.Item2, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Weapon":
                        spriteBatch.Draw(game.weapon, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.Item2, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Book":
                        spriteBatch.Draw(game.book, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.Item2, quantityCoords[index], Color.White);
                        index++;
                        break;
                    case "Potion":
                        spriteBatch.Draw(game.potion, coords[index], Color.White);
                        spriteBatch.DrawString(game.mainFont, "x" + things.Item2, quantityCoords[index], Color.White);
                        index++;
                        break;

                }
            }
        }
    }
}