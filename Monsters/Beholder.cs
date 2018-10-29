using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Beholder : Monster
    {
        // constructor
        public Beholder(GameManager game) : base(game)
        {
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 10;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            Health = 50;
            // max dmg Capstonian can cause
            MaxDamage = 3;
            // min dmg Capstonain can cause
            MinDamage = 1;
            // name of monster
            Name = "Beholder";
            // every point above 10 gives a dmg bonus
            Strength = 10;
            MinLevel = 1;
            MaxLevel = 3;
            Sprite = game.beholder;
            
        }



        // MOVE()
        // DESC: Checks for "IsInRoomWithPlayer" and either moves randomly or target movement towards player
        // PARAMS:None
        // RETURNS: None
        public override void Move()
        {
            //Randomized - when not in same room

            //Target Based - In room + Aggro'd

            if (game.IsInRoomWithPlayer(this.X, this.Y))
            {
                targetBased();
            }
            else
            {
                // not in same room
                randomizedMovement();
            }
        }

        protected override void randomizedMovement()
        {
            // Random Direction 
            int randNumber;

            // Recast int back to enum Type 
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/enumeration-types
            int newX;
            int newY;
            // loop to make sure new coordinates are reachable for monster
            do
            {
                randNumber = Capstonia.GameManager.Random.Next(0, Enum.GetNames(typeof(MonsterDirection)).Length - 1);
                MonsterDirection switchCase = (MonsterDirection)randNumber;
                newX = this.X;
                newY = this.Y;
                switch (switchCase)
                {
                    case MonsterDirection.North:
                        newY -= 1;
                        break;
                    case MonsterDirection.NorthEast:
                        newY -= 1;
                        newX += 1;
                        break;
                    case MonsterDirection.East:
                        newX += 1;
                        break;
                    case MonsterDirection.SouthEast:
                        newY += 1;
                        newX += 1;
                        break;
                    case MonsterDirection.South:
                        newY += 1;
                        break;
                    case MonsterDirection.SouthWest:
                        newY += 1;
                        newX -= 1;
                        break;
                    case MonsterDirection.West:
                        newX -= 1;
                        break;
                    case MonsterDirection.NorthWest:
                        newY -= 1;
                        newX -= 1;
                        break;
                }
            } while (!game.Level.IsWalkable(newX,newY)); // This is assuming that door// hall tiles are set appropriately
                                                         // Under "PlaceMonster in LevelGen" comment ensures that isWalkAble == not door or wall
        }
        protected override void targetBased()
        {
            int playerPosX = game.Player.X;
            int playerPosY = game.Player.Y;
            bool attackStatus = false;
            //setting up radius
            Rectangle monsterRadius = new Rectangle(this.X - 4, this.Y - 4, 8, 8);
            //same room with player && with-in radius

            bool aggro = monsterRadius.Contains(playerPosX, playerPosY);
            if (!aggro)
            {
                randomizedMovement();
            }
            else
            {
                attackStatus = CanAttack();
                if (attackStatus)
                {
                    //TODO - Add AttackFunction here
                    game.Messages.AddMessage("Stabby Stabby"); // remmove after implementation (if you want to)
                }
                else
                {
                    bool linearX = linearXCheck();
                    bool linearY = linearYCheck();
                    bool isTop = topCheck();
                    bool isRight = rightCheck();
                    int movement =-1;
                    //Check L / R of player vs monster position
                    if ( linearX || linearY)
                    {
                        
                        if (linearX)
                        {
                            if (isRight)
                                MoveEast();
                            else
                                MoveWest();
                        }
                        else
                        {
                            if (isTop)
                                MoveNorth();
                            else
                                MoveSouth();
                        }
                    }
                    else
                    {
                        if ( isTop && isRight)
                        {
                            movement = Capstonia.GameManager.Random.Next(0, 2);
                        }
                        else if( isTop && !isRight)
                        {
                            movement = Capstonia.GameManager.Random.Next(2, 4);
                        }
                        else  if(!isTop && isRight)
                        {
                            movement = Capstonia.GameManager.Random.Next(4, 6);
                        }
                        else if(!isTop && !isRight)
                        {
                            
                            movement = Capstonia.GameManager.Random.Next(6, 8);
                            if (movement == 8)
                                movement = 0;
                            
                        }

                        moveCases(movement);


                    }
                }
                // Player above or below 
                //Player left or right
                //Movement itself with a CHECK
                // CHECK being == if trying to move into cell with player
                // do no movement update but attack instead
            }
        }
        
        private bool linearXCheck()
        {
            return (this.X == game.Player.X);
        }
        private bool linearYCheck()
        {
            return (this.Y == game.Player.Y);
        }
        private bool topCheck()
        {
            return (this.Y > game.Player.Y);
        }
        private bool rightCheck()
        {
            return (this.X < game.Player.X);
        }
        protected override bool CanAttack()
        {
            int playerPosX = game.Player.X;
            int playerPosY = game.Player.Y;
            //setting up radius
            Rectangle attackRadius = new Rectangle(this.X - 1, this.Y - 1, 2, 2);
            // should be attack-able if player is within attack radius

            if (attackRadius.Contains(playerPosX, playerPosY))
                return true;
            
            return false;
        }

        protected override void moveCases(int switchCase)
        {
            switch (switchCase)
            {
                case -1:
                    game.Messages.AddMessage("Shouldn't be seeing this in moveCases");
                    break;
                case (int) MonsterDirection.North:
                    MoveNorth();
                    break;
                case (int)MonsterDirection.NorthEast:
                    MoveNorthEast();
                    break;
                case (int)MonsterDirection.East:
                    MoveEast();
                    break;
                case (int)MonsterDirection.SouthEast:
                    MoveSouthEast();
                    break;
                case (int)MonsterDirection.South:
                    MoveSouth();
                    break;
                case (int)MonsterDirection.SouthWest:
                    MoveSouthWest();
                    break;
                case (int)MonsterDirection.West:
                    MoveWest();
                    break;
                case (int)MonsterDirection.NorthWest:
                    MoveNorthWest();
                    break;
            }
        }
        protected override void MoveNorth()
        {
            this.Y -= 1;
        }
        protected override void MoveNorthEast()
        {
            this.Y -= 1;
            this.X += 1;
        }
        protected override void MoveEast()
        {
            this.X += 1;
        }
        protected override void MoveSouthEast()
        {
            this.Y += 1;
            this.X += 1;
        }
        protected override void MoveSouth()
        {
            this.Y += 1;
        }
        protected override void MoveSouthWest()
        {
            this.Y += 1;
            this.X -= 1;
        }
        protected override void MoveWest()
        {
            this.X -= 1;
        }
        protected override void MoveNorthWest()
        {
            this.Y -= 1;
            this.X -= 1;
        }


    }
}