using System;
using System.Collections.Generic;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    public class Chest : Item
    {
        RogueSharp.Random.DotNetRandom Die = new RogueSharp.Random.DotNetRandom();

        public Chest(GameManager game) : base(game)
        {
            Name = "Chest";
            Damage = 0;
            Defense = 0;
            Value = ValuePoints();
            History = "PennyPincher";
            Interactive = true;
            Consumable = false;
            MaxStack = 1;
            //Sprite = game.chest;
        }

        private int ValuePoints()
        {
            return Die.Next(50, 100); // returns a value for skull object between 1 and 50 inclusive
        }

        public override void AddStat()
        {
            //Should be adding to SCORE here//
        }
        public override void RemoveStat()
        {
            game.Messages.AddMessage("Better be glad it's not a mimic.");
        }

        public override void Broadcast()
        {
            //https://stackoverflow.com/questions/7227413/c-sharp-variables-in-strings //
            string tmp = String.Format("Found {0} worth of gold in the chest!.", Value);
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
            game.Messages.AddMessage("Found a chest with " + Value + " value");

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }


    }
}
