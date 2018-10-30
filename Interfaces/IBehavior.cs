using RogueSharp;

namespace Capstonia.Interfaces
{
    public interface IBehavior
    {
        void Move();
        void targetBased();
        bool CanAttack();
        void moveCases(int switchCase);
        void MoveNorth();
        void MoveNorthEast();
        void MoveEast();
        void MoveSouthEast();
        void MoveSouth();
        void MoveSouthWest();
         void MoveWest();
         void MoveNorthWest();
    }
}


