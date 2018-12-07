using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = RogueSharp.Rectangle;
using IDrawable = Capstonia.Interfaces.IDrawable;


namespace Capstonia.Core
{
    public class Exit : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Scale { get; set; }
        public Texture2D Sprite;

        private GameManager game;

        // constructor
        public Exit(GameManager game)
        {
            this.game = game;
            Scale = game.scale;
            Sprite = game.exit;
        }

        // Draw()
        // DESC:    Draws the exit to the screen        
        // PARAMS:  
        // RETURNS: None.
        public void Draw(SpriteBatch spriteBatch)
        {
            if(game.IsInRoomWithPlayer(X, Y))
            {
                Rectangle currRoom = game.Level.GetPlayerRoom();
                float multiplier = game.scale * game.tileSize;
                var drawPosition = new Vector2((X - currRoom.Left) * multiplier, (Y - currRoom.Top) * multiplier);

                spriteBatch.Draw(Sprite, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
            }

        }
    }
}

