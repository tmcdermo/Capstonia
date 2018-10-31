using RogueSharp;

namespace Capstonia.Interfaces
{
    public interface IBehavior
    {
        void Move();
        void FindPath();
        void TakeStep(RogueSharp.Path nextStep);
        bool CanAttack();
        void FixPos(int x, int y, bool status);
    }
}


