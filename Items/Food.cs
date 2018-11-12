using System;
using System.Collections.Generic;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    public class Food: Item
    {

        public Food(GameManager game): base(game)
        {
            Name = "Food";
            Strength = 0;
            Defense = 0;
            Value = ValuePoints();
            History = "ChittyChittyBangBang";
            Interactive = true;
            Consumable = true;
            MaxStack = 5;
            Sprite = game.food;
        }
        
        private int ValuePoints()
        {
            return Capstonia.GameManager.Random.Next(10, 25); //Hunger replenish value//
        }

        public override void AddStat()
        {
            game.Player.Hunger += Value;
            if(game.Player.Hunger > game.Player.MaxHunger)
            {
                game.Player.Hunger = game.Player.MaxHunger; // same concept can't eat over capacity
            }
            game.Messages.AddMessage("Feasted on the flesh of your enemites and recovered " + Value + " hunger");
        }

        public override void RemoveStat()
        {
            //game.Messages.AddMessage("Food only works if it goes in 1 way and out another.");
        }

        public override void Broadcast()
        {
            string tmp = String.Format("Food boosts your hunger by {0}", Value);
            game.Messages.AddMessage(tmp);
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override bool UseItem()
        {
            AddStat();

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }
    }
}
