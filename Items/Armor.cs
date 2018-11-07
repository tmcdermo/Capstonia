using System;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
  
    /// <summary>
    /// Armor should provide an increase in Players Defense
    /// </summary>
    public class Armor: Item
    {
        private string armorType;
        public string ArmorType { get { return armorType; } set { armorType = value; } }
        private int armorMultiplier;
        public int ArmorMultiplier { get { return armorMultiplier; } set { armorMultiplier = value; } }
        public Armor(GameManager game): base(game)
        {
            Name = "Armor";
            ArmorType = "Leather Jerkin";
            Strength = 0;
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
            game.Player.ArmorType = ArmorType;
        }
        /// <summary>
        /// Similar to AddState on UnEquip we remove previous stats
        /// </summary>
        public override void RemoveStat()
        {
            game.Player.ArmorValue -= Defense;
            game.Player.ArmorType = "";
        }

        public override void Broadcast()
        {
            game.Messages.AddMessage(ArmorType + " has " + Defense + " defense");
        }

        //Overrides parent class function
        public override bool UseItem()
        {
            //Add damage and defense values and dipslay on message log
            AddStat();
            game.Messages.AddMessage("Equipped armor with +" + Defense + " defense");


            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }

    }
}
