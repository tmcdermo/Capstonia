using System;
using System.Collections.Generic;
using RogueSharp;
using Capstonia.Core;

namespace Capstonia.Items
{
    public class Potion: Item
    {

        public enum Pots
        {
            Healing,
            Greater_Healing
        }

        private Pots brew;
        public Pots Brew { get { return brew; } set { brew = value; } }

        public Potion(GameManager game): base(game)
        {
            Name = "Potion";
            Brew = PotionType();
            Damage = 0;
            Defense = 0;
            Value = HealValue();
            History = "Never too early for a drink.";
            Interactive = true;
            Consumable = true;
            MaxStack = 5;
            Sprite = game.potion;
        }
        private Pots PotionType()
        {
            Array store = Enum.GetValues(typeof(Pots));
            int x = Capstonia.GameManager.Random.Next(0, store.Length - 1);
            return (Pots)store.GetValue(x);
        }
        private int HealValue()
        {
            int tmp;

            switch (this.Brew)
            {
                case Pots.Healing:
                    tmp= 5;
                    break;
                case Pots.Greater_Healing:
                    tmp = 10;
                    break;
                default:
                    tmp = 0;
                    break;
            }

            return tmp;
        }

        public override void AddStat()
        {
            //Add potion usage//
            game.Player.CurrHealth += Value;
            if (game.Player.CurrHealth > game.Player.MaxHealth)
                game.Player.CurrHealth = game.Player.MaxHealth; // can't heal over max health so set to max
            game.Messages.AddMessage(String.Format("Feasted on the blood of your enemies and recovered {0} health", Value));
        }

        public override void RemoveStat()
        {
            //game.Messages.AddMessage("No self hurt here.");
        }
        public override void Broadcast()
        {
            game.Messages.AddMessage("Potion heals " + Value + " health");
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override void UseItem()
        {
            //Call AddStat to restore health
            AddStat();   
        }
    }
}
