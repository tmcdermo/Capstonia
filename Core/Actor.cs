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
        public int Constitution { get ; set; } // every point above 10 gives a health bonus
        public int Dexterity { get; set; } // every point above 10 gives a dodge bonus
        //public float Constitution { get; set; } // every point above 10 gives a health bonus
        //public float Dexterity { get; set; } // every point above 10 gives a dodge bonus
        public int MaxHealth { get; set; } // Max health total for Capstonian; if the values reaches 0, the Capstonain is killed
        public int CurrHealth { get; set; } // Current health total for Capstonian; if the values reaches 0, the Capstonain is killed
        public int Level { get; set; } // actor level, which wil impact other stats
        public int MaxDamage { get; set; } // max dmg Capstonian can cause
        public int MinDamage { get; set; } // min dmg Capstonain can cause
        public string Name { get; set; } // name of actor
        public int Strength { get; set; } // every point above 10 gives a dmg bonus
        //public float Strength { get; set; } // every point above 10 gives a dmg bonus

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


