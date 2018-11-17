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
            Damage = DamageGet(0,1);
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
            //Changes from ading to strength to adding to max damage to help compensate for hunger penalties
            //Also it a weapon doesn't inheritely make you stronger, just adds damage you can do, so it makes sense
            //game.Player.Strength += this.Strength;  //NEW - COMMENTED
            game.Player.MaxDamage += this.Damage;  //NEW
            game.Player.WeaponValue += this.Damage;    //NEW
            game.Player.WeaponType = weaponType;
        }

        public override void RemoveStat()
        {
            //Changes from ading to strength to adding to max damage to help compensate for hunger penalties
            //Also it a weapon doesn't inheritely make you stronger, just adds damage you can do, so it makes sense
            //game.Player.Strength -= this.Strength;    //NEW - COMMENTED
            game.Player.MaxDamage -= this.Damage;  //NEW
            game.Player.WeaponValue = 0;    //NEW
            game.Player.WeaponType = "";
        }
        public override void Broadcast()
        {
            game.Messages.AddMessage(Name + " does " + Damage + " damage");
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override void UseItem()
        {
            game.WeaponSound.Play();
            //If weapon is equipped
            AddStat();
            game.Messages.AddMessage("Equipped weapon with +" + Damage + " damage");
        }

        // Level factor to increase weapon potency as needed 
        // pretty much at every 2 levels we get +2 atk at this rate
        public virtual int DamageGet(int low, int high)
        {
            return ((game.Player.Level / 2) * 1) + Capstonia.GameManager.Random.Next(low, high);
        }

        
        

    }
}
