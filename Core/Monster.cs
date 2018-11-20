using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Capstonia.Interfaces;
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
            MaxHealth = 50; // max health total for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 50; // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxDamage = 3; // max dmg Capstonian can cause
            MinDamage = 1; // min dmg Capstonain can cause
            Name = "Minstrel"; // name of Capstonian
            Strength = 10;  // every point above 10 gives a dmg bonus
            MinGlory = 1;
            MaxGlory = 3;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
        }

        public void Attack()
        {
            if(game.Player.CurrHealth > 0)
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
                    game.DodgeAttack.Play();
                    game.Messages.AddMessage("You dodge the " + Name + "'s attack!");
                    return;
                }

                // calculate base Player dmg
                int dmgRoll = GameManager.Random.Next(MinDamage, MaxDamage);
                int dmgValue = 2 * dmgRoll + getDamageBonus();

                // calculate total dmg
                int mitigationValue = GameManager.Random.Next(MinDamage, MaxDamage);
                int totalDmg = dmgValue - mitigationValue;

                if (totalDmg <= 0)
                {
                    game.BlockAttack.Play();
                    game.Messages.AddMessage("You block the " + Name + "'s attack!");
                    return;
                }

                // inflict dmg on Capstonian
                game.PlayRandomFromList(game.PlayerHit);
                game.Messages.AddMessage(Name + " inflicts " + totalDmg + " dmg on you!! ");
                game.Player.CurrHealth -= totalDmg;

                if (game.Player.CurrHealth <= 0)
                {
                    game.HandlePlayerDeath(Name);
                }
            }
            
        }

        // MOVE()
        // DESC: Checks for "IsInRoomWithPlayer" and finds shortest path to move to player's position
        // PARAMS:None
        // RETURNS: None
        public void Move()
        {
            //TODO - Make this to where it is called whenever player attacks without moving
            //TODO - tried to make bool playerHasActed - did not work - the game updates too fast
            //TODO - Removing the "only when player has moved" statement causes enemeies to attack too fast for player reliably act
            //Only call once player has moved - this retains turn based movement
            if (game.Player.X != oldPlayerX || game.Player.Y != oldPlayerY || game.Player.hasActed) 
            {
                //Check if monster is in room with player and move towards player if it is, otherwise refill HP
                if (game.IsInRoomWithPlayer(this.X, this.Y))
                {
                    FindPath();
                }
                else
                {
                    this.CurrHealth = this.MaxHealth;
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
                else if((game.Player.X == nextSpot.X) && (game.Player.Y == nextSpot.Y))
                {
                    Attack();
                }
            }
            else
            {
                Attack();
            }
        }


        //fixPos()
        // DESC: Sets the tile at (x,y) to be either walkable or not walkable
        // PARAMS: x (int), y (int), status (bool)
        // RETURNS: None
        public void FixPos(int x, int y, bool status)
        {
            game.Level.SetIsWalkable(x, y, status);
        }

        // DrawStats(...)
        // DESC:    Draw Player equipment to screen.
        // PARAMS:  SpriteBatch instance.
        // RETURNS: None.
        public void DrawStats(SpriteBatch spriteBatch)
        {
            const int iconVertOffset = 52; // center icon vertically in grid cell
            const int iconHorizOffset = 60; // center icon horizontally in grid cell
            const int textVertOffset = 13; // offset for text
            const int textHorizOffset = 240; // offset for text
            const int gridVertOffset = 502 + 153; // offset for grid
            const int gridHorizOffset = 672; // offset for grid
            int fudgeFactorScore = 18; // pixel offset to center text

            // draw stats outline
            spriteBatch.Draw(game.MonsterStatsOutline, new Vector2(gridHorizOffset, gridVertOffset), Color.White);

            // draw title
            int horiztOffsetForTitle = 760;
            int fudgeFactorTitle = 5;
            spriteBatch.DrawString(game.mainFont, "MONSTER STATS", new Vector2(horiztOffsetForTitle + fudgeFactorTitle, gridVertOffset + fudgeFactorScore), Color.White);

            int iteration = 1; // used to offset
            const int maxNumberOfMonsters = 3; // max number of monsters that can be displayed in the grid
            foreach (Monster enemy in game.Monsters)
            {
                // the monster stats grid is currently fixed size and we are only allowing 3 to be displayed
                if (game.IsInRoomWithPlayer(enemy.X, enemy.Y)  && iteration <= maxNumberOfMonsters)
                {
                    // draw 2nd monster if there is one
                    //spriteBatch.Draw(game.Monsters[iteration].Sprite, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset), Color.White);
                    //spriteBatch.DrawString(game.mainFont, game.Monsters[iteration].Name, new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
                    //spriteBatch.DrawString(game.mainFont, game.Monsters[iteration].CurrHealth.ToString() + "/" + game.Monsters[iteration].MaxHealth.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
                    spriteBatch.Draw(enemy.Sprite, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset), Color.White);
                    spriteBatch.DrawString(game.mainFont, enemy.Name, new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
                    spriteBatch.DrawString(game.mainFont, enemy.CurrHealth.ToString() + "/" + enemy.MaxHealth.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
                    ++iteration;
                }
            }
        }

        // GetMonsterExperience
        // DESC:    Returns value of Monster experience determined by level
        // PARAMS:  None
        // RETURNS: Experience value (int)
        public int GetMonsterExperience()
        {
            switch (Level)
            {
                case 1: return 1;
                case 2: return 2;
                case 3: return 4;
                case 4: return 8;
                case 5: return 16;
                case 6: return 32;
                case 7: return 64;
                case 8: return 128;
                case 9: return 256;
                case 10: return 512;
            }

            // should never reach this
            return -1;
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
