
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Capstonia.Interfaces;
using IDrawable = Capstonia.Interfaces.IDrawable;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Core
{
    public class Item : IItem, IDrawable
    {
        //IItems Interface
        /// <summary>
        /// Variables are perks that can be added to the player base stats
        /// Interactive - Can the item be used i.e. armor vs lamp 
        /// consumable - potions etc
        /// </summary>
        private string name;
        public string Name { get { return name; } set { name = value; } }
        //private int strength;
        //public int Strength { get { return strength; } set { strength = value; } }
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

        public Texture2D Sprite { get; set; }

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
        public void Draw(SpriteBatch spriteBatch)
        {
            //Only draw if item is in same room as player
            if (game.IsInRoomWithPlayer(X, Y))
            {
                Rectangle currRoom = game.Level.GetPlayerRoom();
                float multiplier = game.scale * game.tileSize;
                var drawPosition = new Vector2((X - currRoom.Left) * multiplier, (Y - currRoom.Top) * multiplier);

                spriteBatch.Draw(Sprite, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
            }
        }


        //Virtual function that is overridden by child classes
        public virtual void UseItem() {}

        // Undo what Use item does //
        public virtual void RemoveItem()
        {
            RemoveStat();
        }
    }
}
