using System;
using System.Collections.Generic;
using Capstonia.Core;
using RogueSharp;

namespace Capstonia.Items
{
    public class Weapon : Item
    {
        public Weapon(GameManager game) : base(game)
        {
            Name = "Weapon";
            Strength = Capstonia.GameManager.Random.Next(5, 15);
            Defense = 0;
            Value = 0;
            History = "Close your eyes and swing it around.";
            Interactive = true;
            Consumable = false;
            IsEquipped = false;
            MaxStack = 1;
            Sprite = game.weapon_club;
        }

        public override void AddStat()
        {
            game.Player.Strength += this.Strength;
        }

        public override void RemoveStat()
        {
            game.Player.Strength -= this.Strength;
        }
        public override void Broadcast()
        {
            game.Messages.AddMessage(Name + " has " + Strength + " strength");
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override bool UseItem()
        {
            //If weapon is equipped
            AddStat();
            game.Messages.AddMessage("Equipped weapon with +" + Strength + " strength");

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }

    }
}
