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
        public int MinGlory { get; set; }
        public int MaxGlory { get; set; }

        public virtual int getHitBonus()
        {
            return Dexterity - game.BaseDexterity;
        }
        public virtual int getDodgeBonus()
        {
            return Dexterity - game.BaseDexterity;
        }

        public virtual int getDamageBonus()
        {
            return Strength - game.BaseStrength;
        }

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
            MinGlory = 1;
            MaxGlory = 3;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
        }

        public void Attack()
        {
            game.Messages.AddMessage(Name + " attacks YOU!");

            // calculate rolls for battle
            int hitRoll = GameManager.Random.Next(1, 20);
            int defenseRoll = GameManager.Random.Next(1, 20);

            // calculate attack & defense rolls
            int hitValue = hitRoll + getHitBonus();
            int defenseValue = defenseRoll + game.Player.GetDodgeBonus();

            // Player wins tie
            if (hitValue < defenseValue)
            {
                game.Messages.AddMessage("You dodge the " + Name + "'s attack!");
                return;
            }

            // calculate base Player dmg
            int dmgRoll = GameManager.Random.Next(MinDamage, MaxDamage);
            int dmgValue = dmgRoll + getDamageBonus();

            // calculate total dmg
            int totalDmg = dmgValue - defenseValue;

            if (totalDmg <= 0)
            {
                game.Messages.AddMessage("You block the " + Name + "'s attack!");
                return;
            }

            // inflict dmg on Capstonian
            game.Messages.AddMessage(Name + " inflicts " + totalDmg + " dmg on you!! ");
            game.Player.Health -= totalDmg;

            if (game.Player.Health <= 0)
            {
                game.HandlePlayerDeath();
            }
        }

        // MOVE()
        // DESC: Checks for "IsInRoomWithPlayer" and either moves randomly or target movement towards player
        // PARAMS:None
        // RETURNS: None
        public void Move()
        {
            //Only call once player has moved - this retains turn based movement
            if (game.Player.X != oldPlayerX || game.Player.Y != oldPlayerY)
            {
                //Check if monster is in room with player and attack if it is
                if (game.IsInRoomWithPlayer(this.X, this.Y))
                {
                    FindPath();
                }

                //Update player coordinates for following call to monster move
                oldPlayerX = game.Player.X;
                oldPlayerY = game.Player.Y;

            }


        }

        //findPath()
        // DESC: Unlock walkability of monster and player cell so we can find a path, and then relock (true/false)
        // Graph shortest path and pass it into Take Step Function to facilitate movement
        // PARAMS: None
        // RETURNS: None
        public void FindPath()
        {

            ICell MonsterCell = game.Level.GetCell(X, Y); // current cell
            ICell PlayerCell = game.Level.GetCell(game.Player.X, game.Player.Y); // target cell
            FixPos(X, Y, true);     //Set isWalkable of old position to true
            FixPos(PlayerCell.X, PlayerCell.Y, true);
            RogueSharp.PathFinder diffPath = new RogueSharp.PathFinder(game.Level, 1.41);
            FixPos(X, Y, false);    //Set isWalkable of new position to false
            FixPos(PlayerCell.X, PlayerCell.Y, false);

            //Get shortest path between monster and player and take a step while setting isWalkable
            instructions = diffPath.ShortestPath(MonsterCell, PlayerCell);
            if (instructions != null)
            {
                FixPos(this.X, this.Y, true);
                TakeStep(instructions);
                FixPos(this.X, this.Y, false);
            }
        }

        //TakeStep()
        // DESC: Take one step in the shortest path towards player
        // PARAMS: Path containing the next step to take
        // RETURNS: None
        public void TakeStep(Path nextStep)
        {
            ICell nextSpot = nextStep.TryStepForward();
            if (nextStep.Length > 2) //Path list has 2 items left in it when the only items are Monster Location and Player location ( i.e. next to each other)
            {
                if (game.Level.IsWalkable(nextSpot.X, nextSpot.Y)) // prevents walking onto players or walls might not be needed (?)
                {
                    this.X = nextSpot.X;
                    this.Y = nextSpot.Y;
                }
            }
            else
            {
                game.Messages.AddMessage("stabby stabby");

                //Added call to Attack - error is because it hasn not been merged with master yet - should resolve
                Attack();
            }
        }

        //CanAttack()
        // DESC: Checks if player is within 1 square of monster thus making it attackable
        // PARAMS: None
        // RETURNS: bool - whether or not player can be attacked
        public bool CanAttack()
        {
            int playerPosX = game.Player.X;
            int playerPosY = game.Player.Y;

            //setting up radius of 1 unit around monster
            Rectangle attackRadius = new Rectangle(this.X - 1, this.Y - 1, 2, 2);

            // should be attack-able if player is within attack radius
            if (attackRadius.Contains(playerPosX, playerPosY))
                return true;

            return false;
        }

        //fixPos()
        // DESC: Sets the tile at (x,y) to be either walkable or not walkable
        // PARAMS: x (int), y (int), status (bool)
        // RETURNS: None
        public void FixPos(int x, int y, bool status)
        {
            game.Level.SetIsWalkable(x, y, status);
        }


        // OLD CODE LEFT HERE TO SHOW WORK IN VIDEO///
        /*
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
        */
    }
}
