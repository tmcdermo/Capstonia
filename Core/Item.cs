using System;
using System.Collections.Generic;
using Capstonia.Interfaces;
using RogueSharp;

namespace Capstonia.Core
{
    public class Item : IItem,IDrawable
    {
        //IItems Interface
        /// <summary>
        /// Variables are perks that can be added to the player base stats
        /// Interactive - Can the item be used i.e. armor vs lamp 
        /// consumable - potions etc
        /// </summary>
        private string name;
        public string Name { get { return name; } set { name = value; } }
        private int damage;
        public int Damage { get { return damage; } set { damage = value; } }
        private int defense;
        public int Defense { get { return defense; } set { defense = value; } }
        private int _value; // had to _value cause value is keyword
        public int Value { get { return _value; } set { _value = value; } }
        private string history;
        public string History { get { return history; } set { history = value; } }
        private bool interactive;
        public bool Interactive { get { return interactive; } set { interactive = value; } }
        private bool consumable;
        public bool Consumable { get { return consumable; } set { consumable = value; } }
        private bool isEquipped;
        public bool IsEquipped { get { return isEquipped; } set { isEquipped = value; } }
        private int maxStack;
        public int MaxStack { get { return maxStack; } set { maxStack = value; } }


        //IDrawable
        public int X { get; set; }
        public int Y { get; set; }
        public float Scale { get; set; }

        protected GameManager game;

        public  Item(GameManager instance)
        {
            game = instance;
            Scale = game.scale;
        }

        /// <summary>
        /// Currently giving Items ability to access and add/remove stats
        /// ****Long term we might privatize these and generate an "equip/remove" items that'll call these
        /// </summary>
        public virtual void AddStat() { }
        public virtual void RemoveStat() { }
        public virtual void Broadcast() { }
        public void Draw(IMap Level)
        {
            //Implement this flow later
            //Only draw if item is in same room as player
            if (game.IsInRoomWithPlayer(X, Y))
            {
                game.SetLevelCell(X, Y, ObjectType.Item, Level.GetCell(X, Y).IsExplored);
            }
        }

        //TODO - Not sure why we need this, but was in the tutorial. Might remove later?
        public bool Use()
        {
            return UseItem();
        }

        //Virtual function that is overridden by child classes
        public virtual bool UseItem()
        {
            return false;
        }
    }
}
