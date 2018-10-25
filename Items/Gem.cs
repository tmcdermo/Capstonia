using System;
using System.Collections.Generic;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    public class Gem : Item
    {

        public Gem(GameManager game) : base(game)
        {
            Name = "Score";
            Damage = 0;
            Defense = 0;
            Value = ValuePoints();
            History = "Be they worth something?";
            Interactive = true;
            Consumable = false;
            MaxStack = 1;
        }

        private int ValuePoints()
        {
            return Capstonia.GameManager.Random.Next(1, 25); // returns a value for skull object between 1 and 50 inclusive
        }

        public override void AddStat()
        {
            //Should be adding to SCORE here//
        }
        public override void RemoveStat()
        {
            game.Messages.AddMessage("Something something useless gems");
        }

        public override void Broadcast()
        {
            //https://stackoverflow.com/questions/7227413/c-sharp-variables-in-strings //
            string tmp = String.Format("Found {0} worth of Gems!.", Value);
            game.Messages.AddMessage(tmp);
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override bool UseItem()
        {
            //If item is picked up
            AddStat();
            game.Messages.AddMessage("Found a gem worth " + Value + " value");

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }


    }
}
