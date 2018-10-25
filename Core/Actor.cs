using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Capstonia.Controller;
using Capstonia.Interfaces;
using RogueSharp;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Core
{
    public class Actor : IActor
    {
        // IActor
        private int constitution; // every point above 10 gives a health bonus
        public int Constitution { get { return constitution; } set { constitution = value; } }
        private int dexterity; // every point above 10 gives a dodge bonus
        public int Dexterity { get { return dexterity; } set { dexterity = value; } }
        private int health; // health total for Capstonian; if the values reaches 0, the Capstonain is killed
        public int Health { get { return health; } set { health = value; } }
        private int maxDamage; // max dmg Capstonian can cause
        public int MaxDamage { get { return maxDamage; } set { maxDamage = value; } }
        private int minDamage; // min dmg Capstonain can cause
        public int MinDamage { get { return minDamage; } set { minDamage = value; } }
        private string name; // name of actor
        public string Name { get { return name; } set { name = value; } }
        private int strength; // every point above 10 gives a dmg bonus
        public int Strength { get { return strength; } set { strength = value; } }

        // IDrawable
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }

        protected GameManager game;

        public Actor(GameManager game)
        {
            this.game = game;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle currRoom = game.Level.GetPlayerRoom();
            float multiplier = game.scale * game.tileSize;
            var drawPosition = new Vector2((X - currRoom.Left) * multiplier, (Y - currRoom.Top) * multiplier);

            if (game.Player == this)
            {
                spriteBatch.Draw(Sprite, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
            }
            else
            {
                if(game.IsInRoomWithPlayer(X, Y))
                {
                    // should not need to render floor here
                    //spriteBatch.Draw(game.floor, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(Sprite, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                }
            }
        }

        public void OldDraw(SpriteBatch spriteBatch)
        {
            float testScale = 0.5f;

            if (game.Player == this)
            {
                 // scale sprite 
                float multiplier = testScale * Sprite.Width;

                // draw sprite
                spriteBatch.Draw(Sprite, new Vector2(X * multiplier, Y * multiplier), null, Color.White, 0f, Vector2.Zero, testScale, SpriteEffects.None, 0f);
            }
            else
            {
                // Actor is not player, only draw if in same room as player
                //if (game.IsInRoomWithPlayer(X, Y))
                //{
                //    game.SetLevelCell(X, Y, ObjectType.Monster, level.GetCell(X, Y).IsExplored);
                //}                
            }
        }
    }
}


