using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using RogueSharp;
using Rectangle = RogueSharp.Rectangle;
using Point = RogueSharp.Point;
using Capstonia;
using Capstonia.Systems;
using Capstonia.Monsters;

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
            float multiplier = game.tileSize * game.scale;

            Rectangle currRoom = GetPlayerRoom();

            for(int x = currRoom.Left; x <= currRoom.Right; x++)
            {
                for(int y = currRoom.Top; y <= currRoom.Bottom; y++)
                {
                    var drawPosition = new Vector2((x - currRoom.Left) * multiplier, (y - currRoom.Top) * multiplier);

                    if(GetCell(x, y).IsWalkable || (game.Player.X == x && game.Player.Y == y))
                    {
                        spriteBatch.Draw(game.floor, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        bool monsterPresent = false;
                        // loop through all monsters
                        foreach (var monster in game.Monsters)
                        {
                            // check if monsters are in room with player
                            if (game.IsInRoomWithPlayer(monster.X, monster.Y))
                            {
                                // if monster is on the tile, render floor instead of wall
                                if (monster.X == x && monster.Y == y)
                                {
                                    spriteBatch.Draw(game.floor, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                                    monsterPresent = true;
                                    break;
                                }
                            }
                        }
                        if (!monsterPresent)
                        {
                            spriteBatch.Draw(game.wall, drawPosition, null, Color.White, 0f, Vector2.Zero, game.scale, SpriteEffects.None, 0f);
                        }
                    }
                }
            }

            LevelExit.Draw(spriteBatch);
        }

        // OldDraw()
        // DESC:    Old method of drawing level.  Kept for debugging purposes.           
        // PARAMS:  SpriteBatch containing assets
        // RETURNS: None.
        public void OldDraw(SpriteBatch spriteBatch)
        {
            float testScale = 0.5f;

            // Loop through each cell and substitute it for a tile from our tileset
            foreach (Cell cell in GetAllCells())
            {
                var position = new Vector2(cell.X * game.tileSize * testScale, cell.Y * game.tileSize * testScale);
                if (cell.IsWalkable || (cell.X == game.Player.X && cell.Y == game.Player.Y))
                {
                    spriteBatch.Draw(game.floor, position, null, Color.White, 0f, Vector2.Zero, testScale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(game.wall, position, null, Color.White, 0f, Vector2.Zero, testScale, SpriteEffects.None, 0f);
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
            SetActorPosition(player, player.X, player.Y);

        }

        // AddMonster()
        // DESC:    Add Monster to game.
        // PARAMS:  Monster to add
        // RETURNS: None.
        public void AddMonster(Monster monster)
        {
            // make sure to flag monster location as not walkable
            SetIsWalkable(monster.X, monster.Y, false);
            // add monster to monster container
            game.Monsters.Add(monster);
        }

        // AddItem()
        // DESC:    Add Item to game.
        // PARAMS:  Item to add
        // RETURNS: None.
        public void AddItem(Item item)
        {
            // add item to item container
            game.Items.Add(item);
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

                game.Messages.AddMessage(game.Player.X + ", " + game.Player.Y);

                // Update FOV if Player was just repositioned
                //if (actor is Player)
                //{
                //    UpdatePlayerFieldOfView(actor as Player);
                //}
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
                if (game.Player.X >= room.Left && game.Player.X <= room.Right && game.Player.Y >= room.Top && game.Player.Y <= room.Bottom )
                {
                    return room;
                }                    
            }

            // should never reach this as player should always be on board
            game.Messages.AddMessage("IT HIT THE FAN!!!");
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

        //public void UpdatePlayerFieldOfView(Player player)
        //{
        //    ComputeFov(player.X, player.Y, 40, true);
        //    foreach(Cell cell in GetAllCells())
        //    {
        //        if(IsInFov(cell.X, cell.Y))
        //        {
        //            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
        //        }
        //    }
        //}

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

        // IsMonster()
        // DESC:    Checks if Monster is standing in location             
        // PARAMS:  x(int), y(int)
        // RETURNS: Monster if there, null if not
        public Monster IsMonster(int x, int y)
        {
            foreach(Monster monster in game.Monsters)
            {
                if(monster.X == x && monster.Y == y)
                {
                    return monster;
                }
            }

            return null;
        }
    }
}
