using RogueSharp;
using Capstonia.Controller;
using Capstonia.Interfaces;


namespace Capstonia.Core
{
    public class Exit : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Scale { get; set; }

        private GameManager game;

        // constructor
        public Exit(GameManager game)
        {
            this.game = game;
            Scale = game.scale;
        }

        // Draw()
        // DESC:    Draws the exit to the screen        
        // PARAMS:  level(IMap)
        // RETURNS: None.
        public void Draw(IMap level)
        {
            // Ensure Player has found exit before drawing
            if(level.GetCell(X, Y).IsExplored)
            {
                game.SetLevelCell(X, Y, ObjectType.Exit, true);
            }
        }
    }
}

