using System;
using System.Collections.Generic;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    public class Food: Item
    {
        RogueSharp.Random.DotNetRandom Die = new RogueSharp.Random.DotNetRandom();

        public Food(GameManager game): base(game)
        {
            Name = "Food";
            Damage = 0;
            Defense = 0;
            Value = ValuePoints();
            History = "ChittyChittyBangBang";
            Interactive = true;
            Consumable = true;
            MaxStack = 5;
            CurrentStack = 0;
        }
        
        private int ValuePoints()
        {
            return Die.Next(10, 25); //Hunger replenish value//
        }

        public override void AddStat()
        {
            game.Player.Hunger += Value;
            if(game.Player.Hunger > game.Player.MaxHunger)
            {
                game.Player.Hunger = game.Player.MaxHunger; // same concept can't eat over capacity
            }
        }

        public override void RemoveStat()
        {
            game.Messages.AddMessage("Food only works if it goes in 1 way and out another.");
        }

        public override void Broadcast()
        {
            string tmp = String.Format("You found a drumstick: boosts your hunger by {0}", Value);
            game.Messages.AddMessage("tmp");
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override bool UseItem()
        {
            //If item is picked up
            AddStat();

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }
    }
}
