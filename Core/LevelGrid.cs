using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using RogueSharp;
using Rectangle = RogueSharp.Rectangle;
using Capstonia;
using Capstonia.Systems;

namespace Capstonia.Core
{
    public class LevelGrid : Map
    {

        private GameManager game;
        private LevelGenerator levelGenerator;

        public Exit LevelExit { get; set; }

        public List<Rectangle> Rooms;    

        // constructor 
        public LevelGrid(GameManager game)
        {
            this.game = game;

            Rooms = new List<Rectangle>();
        }

        // Draw()
        // DESC:    Displays the current level onscreen.            
        // PARAMS:  SpriteBatch containing assets
        // RETURNS: None.
        public void Draw(SpriteBatch spriteBatch)
        {


            Rectangle currRoom = GetPlayerRoom();

            for(int x = currRoom.Left; x <= currRoom.Right; x++)
            {
                for(int y = currRoom.Top; y <= currRoom.Bottom; y++)
                {
                    Cell currCell = (Cell)GetCell(x, y);
                    //var drawPosition = new Vector2(currCell.X * game.tileSize * game.scale, currCell.Y * game.tileSize * game.scale);
                    var drawPosition = new Vector2((x - currRoom.Left) * game.tileSize * game.scale, (y - currRoom.Top) * game.tileSize * game.scale);

                    if(currCell.IsWalkable || (currCell.X == game.Player.X && currCell.Y == game.Player.Y))
                    {
                        spriteBatch.Draw(game.floor, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(game.wall, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                    }
                }
            }

            //Exit.Draw(this);

            
        }

        // OldDraw()
        // DESC:    Old method of drawing level.  Kept for debugging purposes.           
        // PARAMS:  SpriteBatch containing assets
        // RETURNS: None.
        public void OldDraw(SpriteBatch spriteBatch)
        {

            // Loop through each cell and substitute it for a tile from our tileset
            foreach (Cell cell in GetAllCells())
            {
                var position = new Vector2(cell.X * game.tileSize * game.scale, cell.Y * game.tileSize * game.scale);
                if (cell.IsWalkable || (cell.X == game.Player.X && cell.Y == game.Player.Y))
                {
                    spriteBatch.Draw(game.floor, position, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(game.wall, position, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                }
            }

            //Exit.Draw(this);
        }

        // AddPlayer()
        // DESC:    Add a player to the game.       
        // PARAMS:  player(Player)
        // RETURNS: None.  
        public void AddPlayer(Player player)
        {
            game.Player = player;
            SetIsWalkable(player.X, player.Y, false);

        }

        // SetActorPosition(...)
        // DESC:    Place actor on level.     
        // PARAMS:  An Actor instance and the x, y coordinates for where the
        //          Actor should be placed on the level.
        // RETURNS: Returns a Boolean.  True = succesful placement; 
        //          False = failure to place on level.
        public bool SetActorPosition(Actor actor, int x, int y)
        {
            // Only place Actor if Cell is walkable
            if (GetCell(x, y).IsWalkable)
            {
                // Flag Actor's previous location as walkable
                SetIsWalkable(actor.X, actor.Y, true);

                // Update actor's position
                actor.X = x; 
                actor.Y = y;

                // Flag Actor's current location as not walkable
                SetIsWalkable(actor.X, actor.Y, false);

                // OpenDoor(actor, x, y);

                // Update FOV if Player was just repositioned
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView(actor as Player);
                }
                return true;
            }
            return false;
        }

        // GetPlayerRoom()
        // DESC:    Search through rooms for room player is currently in.       
        // PARAMS:  None
        // RETURNS: Returns a Rectangle representing the room the player is in.
        public Rectangle GetPlayerRoom()
        {
            foreach(Rectangle room in Rooms)
            {
                if (room.Contains(game.Player.X, game.Player.Y))
                    return room;
            }

            // should never reach this as player should always be on board
            return Rooms[0];
        }


        // SetIsWalkable()
        // DESC:    Makes a cell walkable so the player can pass over it.             
        // PARAMS:  x(int), y(int), isWalkable(bool)
        // RETURNS: None.
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y) as Cell;
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        // SetLevelTileForCell()
        // DESC:    TODO
        // PARAMS:  TODO
        // RETURNS: TODO
        //private void SetLevelTileForCell(Cell cell)
        //{
        //    if (cell.IsWalkable) // floor cell
        //    {
        //        game.SetLevelCell(cell.X, cell.Y, ObjectType.Floor, cell.IsExplored);
        //    }
        //    else
        //    {
        //        if (game.Player.X == cell.X && game.Player.Y == cell.Y)
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Player, cell.IsExplored);
        //        }
        //        else if (cell.X == 0)
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Left, cell.IsExplored);
        //        }
        //        else if (cell.X == GameManager.levelWidth - 1)
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Right, cell.IsExplored);
        //        }
        //        else if (cell.Y == 0)
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Top, cell.IsExplored);
        //        }
        //        else if (cell.Y == GameManager.levelHeight - 1)
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Bottom, cell.IsExplored);
        //        }
        //        else if (IsWalkable(cell.X - 1, cell.Y))
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Right, cell.IsExplored);
        //        }
        //        else if (IsWalkable(cell.X + 1, cell.Y))
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Left, cell.IsExplored);
        //        }
        //        else if (IsWalkable(cell.X, cell.Y - 1))
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Top, cell.IsExplored);
        //        }
        //        else
        //        {
        //            game.SetLevelCell(cell.X, cell.Y, ObjectType.Wall_Bottom, cell.IsExplored);
        //        }
        //    }          
        //}

        public void UpdatePlayerFieldOfView(Player player)
        {
            ComputeFov(player.X, player.Y, 40, true);
            foreach(Cell cell in GetAllCells())
            {
                if(IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        //public bool IsInRoomWithPlayer(int x, int y)
        //{
        //    // get room player is in
        //    int RoomIndex;
        //    for (RoomIndex = 0; RoomIndex < Rooms.Count; RoomIndex++)
        //    {
        //        if (Rooms[RoomIndex].Contains(game.Player.X, game.Player.Y))
        //        {
        //            // found player room, now check if passed in coordinates exist within it
        //            if (Rooms[RoomIndex].Contains(x, y))
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    // player should always be located in the list of Rooms so we should never reach this point
        //    return false;
        //}
    }
}