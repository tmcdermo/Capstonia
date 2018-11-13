using System;
using System.Collections.Generic;
using Capstonia.Core;
using RogueSharp;

namespace Capstonia.Items
{
    public class Weapon : Item
    {
        private string weapontype;
        public string weaponType { get; set; }
        public Weapon(GameManager game) : base(game)
        {
            Name = "Weapon";
            weaponType = "Club";
            Strength = StrengthGet(0,1);
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
            game.Player.WeaponType = weaponType;
        }

        public override void RemoveStat()
        {
            game.Player.Strength -= this.Strength;
            game.Player.WeaponType = "";
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

        // Level factor to increase weapon potency as needed 
        // pretty much at every 2 levels we get +2 atk at this rate
        public virtual int StrengthGet(int low, int high)
        {
            return ((game.Player.Level / 2) * 1) + Capstonia.GameManager.Random.Next(low, high);
        }

        
        

    }
}
