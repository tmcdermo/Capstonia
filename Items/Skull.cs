using System;
using System.Collections.Generic;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    public class Skull:  Item
    {
      

        public Skull(GameManager game): base(game)
        {
            Name = "Score";
            Damage = 0;
            Defense = 0;
            Value = ValuePoints();
            History = "ChittyChittyBangBang";
            Interactive = true;
            Consumable = false;
            MaxStack = 1;
            //Sprite = game.skull;
        }

        private int ValuePoints()
        {
            return Capstonia.GameManager.Random.Next(1, 50); // returns a value for skull object between 1 and 50 inclusive
        }

        public override void AddStat()
        {
            //Should be adding to SCORE here//
        }
        public override void RemoveStat()
        {
            game.Messages.AddMessage("Losing is never fun, so not allowed.");
        }

        public override void Broadcast()
        {
            //https://stackoverflow.com/questions/7227413/c-sharp-variables-in-strings //
            string tmp = String.Format("Skull worth {0} glory", Value);
            game.Messages.AddMessage(tmp);
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override void UseItem()
        {
            //If item is picked up
            AddStat();
            game.Messages.AddMessage("Found a spooky skull worth " + Value + " value");

        }


    }
}
