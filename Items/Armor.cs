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
        
        public Armor(GameManager game): base(game)
        {
            Name = "Armor";
            Damage = Capstonia.GameManager.Random.Next(2);
            Defense = Capstonia.GameManager.Random.Next(1,5);
            Value = 0;
            History = "Bullet Stopping Cotton Threads";
            Interactive = true;
            Consumable = false;
            IsEquipped = false;
            MaxStack = 1;
            Sprite = game.armor;
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
        public override bool UseItem()
        {
            //Add damage and defense values and dipslay on message log
            AddStat();
            game.Messages.AddMessage("Equipped armor with +" + Damage + " damage and +" + Defense + " defense");


            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }
    }
}
