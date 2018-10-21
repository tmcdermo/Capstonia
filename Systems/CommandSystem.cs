using Capstonia.Core;

namespace Capstonia.Systems
{
    // CommandSystem class
    // DESC:  Contains attributes and methods Actor mechanics (e.g., Player movement)
    // NOTES: Heavily inspired by: https://github.com/Olivexe/RogueSharpTutorialUnityPort
    public class CommandSystem
    {
        private GameManager game;
        public bool IsPlayerTurn { get; set; }

        // CommandSystem()
        // DESC:    Constructor.
        // PARAMS:  GameManager object.
        // RETURNS: None.
        public CommandSystem(GameManager game)
        {
            this.game = game;
        }

        // MovePlayer(...)
        // DESC:    Moves player 1 tile from current location (diagonal
        //          moves allowed).
        // PARAMS:  Direction enum.
        // RETURNS: Returns a Boolean.  True = valid move, False = invalid move.
        public bool MovePlayer(Direction direction)
        {
            // current Player location
            int x = game.Player.X;
            int y = game.Player.Y;

            // validate a proper move was requested
            switch (direction)
            {
                case Direction.UpLeft:
                    {
                        x = game.Player.X - 1;
                        y = game.Player.Y + 1;
                        break;
                    }
                case Direction.Up:
                    {
                        y = game.Player.Y + 1;
                        break;
                    }
                case Direction.UpRight:
                    {
                        x = game.Player.X + 1;
                        y = game.Player.Y + 1;
                        break;
                    }

                case Direction.Left:
                    {
                        x = game.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = game.Player.X + 1;
                        break;
                    }
                case Direction.DownLeft:
                    {
                        x = game.Player.X - 1;
                        y = game.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = game.Player.Y - 1;
                        break;
                    }
                case Direction.DownRight:
                    {
                        x = game.Player.X + 1;
                        y = game.Player.Y - 1;
                        break;
                    }
                default:
                    {
                        // invalid move request
                        return false;
                    }
            }

            if (game.Level.SetActorPosition(game.Player, x, y))
            {
                return true;
            }

            return false;
        }

        // EndPlayerTurn()
        // DESC:    Update when Player's turn was successful.
        // PARAMS:  None.
        // RETURNS: None.
        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }
    }
}
