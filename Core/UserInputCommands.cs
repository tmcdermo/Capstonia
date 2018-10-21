namespace Capstonia.Core
{
    // UserInputCommands enum
    // DESC:  The Player can move one tile per turn, though they do have the
    //        ability to move diagonally.  Therefore, the player has 8 possible
    //        moves (assuming there are no obstructions, such as walls) they
    //        can make each turn.  Also note that the player will be able to
    //        to change levels and close the game.
    // NOTES: Heavily inspired by: https://github.com/Olivexe/RogueSharpTutorialUnityPort
    public enum UserInputCommands
    {
        UpLeft,
        Up,
        UpRight,
        Left,
        Right,
        DownLeft,
        Down,
        DownRight,
        // only applicable when sprite is on tile that represents the ability
        // to travel to a different level (e.g., stairs).
        ChangeLevel,
        CloseGame,
        None
    }
}
