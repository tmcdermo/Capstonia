using Capstonia.Controller;
using Capstonia.Interfaces;
using Capstonia.Monsters;
using System;
using Rectangle = RogueSharp.Rectangle;
using Path = RogueSharp.Path;
using ICell = RogueSharp.ICell;

namespace Capstonia.Core
{

    public class Monster : Actor, IBehavior
    {
        //Used for preventing too many updates per second
        int oldPlayerX;
        int oldPlayerY;
        Path instructions;

        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }

        // constructor
        public Monster(GameManager game) : base(game)
        {
            Constitution = 10; // every point above 10 gives a health bonus
            Dexterity = 10; // every point above 10 gives a dodge bonus
            Health = 50; // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxDamage = 3; // max dmg Capstonian can cause
            MinDamage = 1; // min dmg Capstonain can cause
            Name = "Minstrel"; // name of Capstonian
            Strength = 10;  // every point above 10 gives a dmg bonus
            MinLevel = 1;
            MaxLevel = 3;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
        }

        //Draw
       // MOVE()
        // DESC: Checks for "IsInRoomWithPlayer" and either moves randomly or target movement towards player
        // PARAMS:None
        // RETURNS: None
        public void Move()
        {
            //Target Based - In room
            if (game.Player.X != oldPlayerX || game.Player.Y != oldPlayerY)
            {
                if (game.IsInRoomWithPlayer(this.X, this.Y))
                {
                    //targetBased();
                    testPF();
                }

                oldPlayerX = game.Player.X;
                oldPlayerY = game.Player.Y;

            }

        }

        public void testPF()
        {

            ICell MonsterCell = game.Level.GetCell(X,Y); // current cell
            ICell PlayerCell = game.Level.GetCell(game.Player.X, game.Player.Y); // target cell
            fixPos(X, Y, true);
            fixPos(PlayerCell.X, PlayerCell.Y, true);
            RogueSharp.PathFinder diffPath = new RogueSharp.PathFinder(game.Level, 1.41);
            fixPos(X, Y, false);
            fixPos(PlayerCell.X, PlayerCell.Y, false);
            instructions = diffPath.ShortestPath(MonsterCell, PlayerCell);
            if (instructions != null)
            {
                
                fixPos(this.X, this.Y, true);
                TakeStep(instructions);
                fixPos(this.X, this.Y, false);
            }
        }

        public void TakeStep(Path nextStep)
        {
            ICell nextSpot = nextStep.TryStepForward();
            if (nextStep.Length > 2) //Path list has 2 items left in it when the only items are Monster Location and Player location ( i.e. next to each other)
            {
                if (game.Level.IsWalkable(nextSpot.X, nextSpot.Y))
                {
                    this.X = nextSpot.X;
                    this.Y = nextSpot.Y;
                }
            }
            else
            {
                game.Messages.AddMessage("stabby stabby");
                //TO-DO :: add attack function here.
            }
        }

        public void targetBased()
        {
            int playerPosX = game.Player.X;
            int playerPosY = game.Player.Y;
            bool attackStatus = false;

            //same room with player && with-in radius

            game.Messages.AddMessage(String.Format("Before X : {0} Y: {1}", this.X, this.Y));
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
                        if (isTop)
                        {
                            fixPos(this.X, this.Y, true);
                            MoveNorth();
                            fixPos(this.X, this.Y, false);
                        }
                        else
                        {
                            fixPos(this.X, this.Y, true);
                            MoveSouth();
                            fixPos(this.X, this.Y, false);

                        }
                    }
                    else
                    {
                        if (isRight)
                        {
                            fixPos(this.X, this.Y, true);
                            MoveEast();
                            fixPos(this.X, this.Y, false);

                        }
                        else
                        {
                            fixPos(this.X, this.Y, true);
                            MoveWest();
                            fixPos(this.X, this.Y, false);
                        }
                    }
                }
                else
                {
                    if (isTop && isRight)
                    {
                        fixPos(this.X, this.Y, true);

                        do
                        {
                            movement = Capstonia.GameManager.Random.Next(0, 2);
                            moveCases(movement);
                        } while (!game.Level.IsWalkable(this.X, this.Y));
                        fixPos(this.X, this.Y, false);
                    }
                    else if (isTop && !isRight)
                    {
                        fixPos(this.X, this.Y, true);
                        do
                        {
                            movement = Capstonia.GameManager.Random.Next(6, 8);
                            if (movement == 8)
                            {
                                movement = 0;
                            }
                            moveCases(movement);
                        } while (!game.Level.IsWalkable(this.X, this.Y));
                        fixPos(this.X, this.Y, false);
                    }
                    else if (!isTop && isRight)
                    {
                        fixPos(this.X, this.Y, true);
                        do
                        {
                            movement = Capstonia.GameManager.Random.Next(2, 4);
                            moveCases(movement);
                        } while (!game.Level.IsWalkable(this.X, this.Y));
                        fixPos(this.X, this.Y, false);
                    }
                    else if (!isTop && !isRight)
                    {

                        fixPos(this.X, this.Y, true);
                        do
                        {
                            movement = Capstonia.GameManager.Random.Next(4, 6);
                            moveCases(movement);
                        } while (!game.Level.IsWalkable(this.X, this.Y));
                        fixPos(this.X, this.Y, false);

                    }
                }
                // Player above or below 
                //Player left or right
                //Movement itself with a CHECK
                // CHECK being == if trying to move into cell with player
                // do no movement update but attack instead
            }
            game.Messages.AddMessage(String.Format("After X : {0} Y: {1}", this.X, this.Y));
        }

        
        public bool linearXCheck()
        {
            return (this.X == game.Player.X);
        }
        public bool linearYCheck()
        {
            return (this.Y == game.Player.Y);
        }
        public bool topCheck()
        {
            return (this.Y > game.Player.Y);
        }
        public bool rightCheck()
        {
            return (this.X < game.Player.X);
        }
        public bool CanAttack()
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

        public void moveCases(int switchCase)
        {
                switch (switchCase)
                {
                    case -1:
                        game.Messages.AddMessage("Shouldn't be seeing this in moveCases");
                        break;
                    case (int)MonsterDirection.North:
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
        public void MoveNorth()
        {
            this.Y -= 1;
        }
        public void MoveNorthEast()
        {
            this.Y -= 1;
            this.X += 1;
        }
        public void MoveEast()
        {
            this.X += 1;
        }
        public void MoveSouthEast()
        {
            this.Y += 1;
            this.X += 1;
        }
        public void MoveSouth()
        {
            this.Y += 1;
        }
        public void MoveSouthWest()
        {
            this.Y += 1;
            this.X -= 1;
        }
        public void MoveWest()
        {
            this.X -= 1;
        }
        public void MoveNorthWest()
        {
            this.Y -= 1;
            this.X -= 1;
        }

        public void fixPos(int x, int y,bool status)
        {
            game.Level.SetIsWalkable(x , y, status);
        }

    }
}
