using RogueSharp;

namespace Capstonia.Interfaces
{
    public interface IDrawable
    {
        int X { get; set; }
        int Y { get; set; }

        void Draw(IMap level);
    }
}
