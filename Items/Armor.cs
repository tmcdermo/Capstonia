using System;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    /// <summary>
    /// Armor should provide an increase in Players Defense and possibly increase
    /// Min / Max Damage as well.
    /// </summary>
    public class Armor: Item
    {
        RogueSharp.Random.DotNetRandom Die = new RogueSharp.Random.DotNetRandom();
        public Armor(GameManager game): base(game)
        {
            Name = "Armor";
            Damage = Die.Next(0,2);
            Defense = Die.Next(1,5);
            Value = 0;
            History = "Bullet Stopping Cotton Threads";
            Interactive = true;
            Consumable = false;
            IsEquipped = false;
        }

        /// <summary>
        /// Function should be called via OnEquip to add and remove stat as needed
        /// </summary>
        public override void AddStat()
        {
            game.Player.ArmorValue += Defense;
            game.Player.MinDamage += Damage;
            game.Player.MaxDamage += Damage;
        }
        /// <summary>
        /// Similar to AddState on UnEquip we remove previous stats
        /// </summary>
        public override void RemoveStat()
        {
            game.Player.ArmorValue -= Defense;
            game.Player.MinDamage -= Damage;
            game.Player.MaxDamage -= Damage;
        }

        public override void Broadcast()
        {
            game.Messages.AddMessage("Armor, Mi Amor!");
        }

        //Overrides parent class function
        protected override bool UseItem()
        {
            //If armor is equipped
            AddStat();

            //If armor is removed
            RemoveStat();

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }
    }
}
