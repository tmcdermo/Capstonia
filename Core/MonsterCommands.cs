namespace Capstonia.Core
{
    // UserInputCommands enum
    // DESC:  The Monsters can move one tile per turn, though they do have the
    //        ability to move diagonally.  Therefore, the monster has 8 possible
    //        moves (assuming there are no obstructions, such as walls) they
    //        can make each turn.
    public enum MonsterDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
}
