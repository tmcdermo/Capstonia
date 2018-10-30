using RogueSharp;

namespace Capstonia.Interfaces
{
    public class IBehavior
    {
        public virtual void Move() { }
        protected virtual void randomizedMovement() { }
        protected virtual void targetBased() { }
        protected virtual bool CanAttack() { return false; }
        protected virtual void moveCases(int switchCase) { }
        protected virtual void MoveNorth() { }
        protected virtual void MoveNorthEast() { }
        protected virtual void MoveEast() { }
        protected virtual void MoveSouthEast() { }
        protected virtual void MoveSouth() { }
        protected virtual void MoveSouthWest() { }
        protected virtual void MoveWest() { }
        protected virtual void MoveNorthWest() { }
    }
}


