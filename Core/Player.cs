using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Capstonia.Core
{
    public class Player : Actor
    {
        public string ArmorType { get; set; } // e.g., leather, chainmail, plate
        public int ArmorValue { get; set; } // e.g., leather, chainmail, plate
        public int Experience { get; set; } // track progress until next level
        public int CurrentExperienceMax { get; set; }
        public int Hunger { get; set; } // 0 = starving, 100 = full
        public int OldHunger { get; set; } // 0 = starving, 100 = full
        public float NewHungerPenalty { get; set; } //hunger penalty to apply to stats
        public float OldHungerPenalty { get; set; } //save old state for updates
        public string WeaponType { get; set; } // e.g., leather, chainmail, plate
        public int WeaponValue { get; set; } // e.g., leather, chainmail, plate
        public int MaxHunger { get; set; } // max hunger = player is full
        public int MinHunger { get; set; } // min hunger = player is famished
        public int Glory { get; set; } // increased after collecting treasure and killing monsters
        private int oldPositionX { get; set; } // save players old positions 
        private int oldPositionY { get; set; }
        public bool LoseTurn { get; set; } // regulator for potentially losing players turn when hunger is 0
        public bool hasActed { get; set; }
        public bool shiftPressed { get; set; }

        // GetHitBonus()
        // DESC:    Calculate and return hit bonus for combat.
        // PARAMS:  None.
        // RETURNS: Return hit bonus.
        public int GetHitBonus()
        {
            return Dexterity - game.BaseDexterity;
        }

        // GetDodgeBonus()
        // DESC:    Calculate and return dodge bonus for combat.
        // PARAMS:  None.
        // RETURNS: Return dodge bonus.
        public int GetDodgeBonus()
        {
            return Dexterity - game.BaseDexterity;
        }

        // GetDamageBonus()
        // DESC:    Calculate and return damange bonus for combat.
        // PARAMS:  None.
        // RETURNS: Return damage bonus.
        public int GetDamageBonus()
        {
            return Strength - game.BaseStrength;
        }

        // GetConstitutionBonus()
        // DESC:    Calculate and return constitution bonus for extra health on level up
        // PARAMS:  None.
        // RETURNS: Return constitution bonus (int)
        public int GetConstitutionBonus()
        {
            return Constitution - game.BaseConstitution;
        }

        // Player
        // DESC:    Constructor that inherits from the base class, game.
        // PARAMS:  A GameManager instance.
        // RETURNS: Instantiate instance of class.
        public Player(GameManager game) : base(game)
        {
            Name = "Empty";
            ArmorType = "Leather Jerkin";
            ArmorValue = 0; // used in dmg calc during battle
            Constitution = 10; // every point above 10 gives a health bonus
            BaseConstitution = 10;
            Dexterity = 10; // every point above 10 gives a dodge bonus
            BaseDexterity = 10;
            //Health = 50; // Health total for Player.  If the values reaches 0, the player is killed
            //MaxHealth = 50; // can grow with constitution
            MaxHealth = 100; // initial health value (out of 100) and can grow with constitution
            CurrHealth = 100; // current health value (out of 100)
            Hunger = 100; // 0 = starving, 100 = full
            OldHunger = 100; // 0 = starving, 100 = full
            NewHungerPenalty = 1.0f; //1.0 = full strength
            OldHungerPenalty = 1.0f; //Save state of old hunger penalty to ensure we don't update too many times
            Level = 0; // 0 = min, 100 = max
            MaxHunger = 100; // cap to not over-feed
            MinHunger = 0; //hunger can't go below 0
            MaxDamage = 5; // max dmg Player can cause
            MinDamage = 1; // min dmg Player can cause
            Name = "Villain"; // name of Player
            Strength = 10;  // every point above 10 gives a dmg bonus
            BaseStrength = 10;
            WeaponType = "Club";
            //WeaponValue = 2;  // used in dmg calc during battle   //NEW - COMMENTED
            WeaponValue = 0;    //NEW
            Glory = 0;
            Level = 1;
            Experience = 0;
            CurrentExperienceMax = 10;
            LoseTurn = false;
            hasActed = false;
            shiftPressed = false;
        }

        // CalculateHungerPenalty()
        // DESC:    Inintially the player is full when the game starts, so their
        //          hunger value is at 100.  With each move, the player's hunger value is
        //          decreased by 1.  To lower their hunger, the player should try to
        //          acquire food from the gameboard.  If a player's hunger decreases to
        //          50 and below, their strength, dexterity, and constitution
        //          will be impacted during combat (see below).  This will continue until
        //          their hunger value raises back above 50.
        // PARAMS:  None.
        // RETURN:  Hunger penalty value as a float.
        public float CalculateHungerPenalty()
        {
            float hungerPenalty = 1.0f;

            if (this.Hunger == 0)
            {
                hungerPenalty = 0.10f; // multiplier for str, dex, const
            }
            else if (this.Hunger > 0 && this.Hunger <= 25)
            {
                hungerPenalty = 0.50f; // multiplier for str, dex, const
            }
            else if (this.Hunger > 25 && this.Hunger <= 50)
            {
                hungerPenalty = 0.90f; // multiplier for str, dex, const
            }
            return hungerPenalty;
        }

        // Move(...)
        // DESC:    Moves player 1 tile from current location.
        // PARAMS:  None.
        // RETURNS: None.
        public void Move()
        {
            Monster monster = null;
            //save players old positions
            oldPositionX = this.X;
            oldPositionY = this.Y;
            hasActed = false;

            // get current keyboard state
            game.currentKeyboardState = Keyboard.GetState();

            //Only update hunger penalty once for each time it changes
            NewHungerPenalty = CalculateHungerPenalty();


            //Output warnings as needed
            HungerWarning();

            HungerStat();
            // move player up
            if ((game.currentKeyboardState.IsKeyDown(Keys.Down) &&
                game.previousKeyboardState.IsKeyUp(Keys.Down)) || (game.currentKeyboardState.IsKeyDown(Keys.NumPad2) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad2)))
            {
                if (game.Level.IsWalkable(game.Player.X, game.Player.Y + 1))
                {
                    //game.Player.Y += 1;
                    game.Level.SetActorPosition(this, X, Y + 1);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X, game.Player.Y + 1);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } // move player down
            else if ((game.currentKeyboardState.IsKeyDown(Keys.Up) &&
                        game.previousKeyboardState.IsKeyUp(Keys.Up)) || (game.currentKeyboardState.IsKeyDown(Keys.NumPad8) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad8)))
            {
                if (game.Level.IsWalkable(game.Player.X, game.Player.Y - 1))
                {
                    //game.Player.Y -= 1;
                    game.Level.SetActorPosition(this, X, Y - 1);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X, game.Player.Y - 1);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } // move player left
            else if ((game.currentKeyboardState.IsKeyDown(Keys.Left) &&
                        game.previousKeyboardState.IsKeyUp(Keys.Left)) || (game.currentKeyboardState.IsKeyDown(Keys.NumPad4) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad4)))
            {
                if (game.Level.IsWalkable(game.Player.X - 1, game.Player.Y))
                {
                    //game.Player.X -= 1;
                    game.Level.SetActorPosition(this, X - 1, Y);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X - 1, game.Player.Y);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } // move player right
            else if ((game.currentKeyboardState.IsKeyDown(Keys.Right) &&
                        game.previousKeyboardState.IsKeyUp(Keys.Right)) || (game.currentKeyboardState.IsKeyDown(Keys.NumPad6) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad6)))
            {
                if (game.Level.IsWalkable(game.Player.X + 1, game.Player.Y))
                {
                    //game.Player.X += 1;
                    game.Level.SetActorPosition(this, X + 1, Y);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X + 1, game.Player.Y);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } // move player upright
            else if ((game.currentKeyboardState.IsKeyDown(Keys.NumPad9) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad9)))
            {
                if (game.Level.IsWalkable(game.Player.X + 1, game.Player.Y - 1))
                {
                    //game.Player.Y += 1;
                    game.Level.SetActorPosition(this, X + 1, Y - 1);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X + 1, game.Player.Y - 1);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } //move player downright
            else if ((game.currentKeyboardState.IsKeyDown(Keys.NumPad3) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad3)))
            {
                if (game.Level.IsWalkable(game.Player.X + 1, game.Player.Y + 1))
                {
                    //game.Player.Y += 1;
                    game.Level.SetActorPosition(this, X + 1, Y + 1);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X + 1, game.Player.Y + 1);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } //move player down left
            else if ((game.currentKeyboardState.IsKeyDown(Keys.NumPad1) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad1)))
            {
                if (game.Level.IsWalkable(game.Player.X - 1, game.Player.Y + 1))
                {
                    //game.Player.Y += 1;
                    game.Level.SetActorPosition(this, X - 1, Y + 1);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X - 1, game.Player.Y + 1);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            } //move player upleft
            else if ((game.currentKeyboardState.IsKeyDown(Keys.NumPad7) &&
                game.previousKeyboardState.IsKeyUp(Keys.NumPad7)))
            {
                if (game.Level.IsWalkable(game.Player.X - 1, game.Player.Y - 1))
                {
                    //game.Player.Y += 1;
                    game.Level.SetActorPosition(this, X - 1, Y - 1);
                    HungerUpdate();
                    hasActed = true;
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X - 1, game.Player.Y - 1);
                    if (monster != null)
                    {
                        Attack(monster);
                        hasActed = true;
                    }
                }
            }
            //testing drop item
            else if(game.currentKeyboardState.IsKeyDown(Keys.D))
            {
                if(game.currentKeyboardState.IsKeyDown(Keys.D1) && game.previousKeyboardState.IsKeyUp(Keys.D1))
                {
                    //Drop item at index 0
                    game.Inventory.DropItem(0);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D2) && game.previousKeyboardState.IsKeyUp(Keys.D2))
                { 
                    //Drop item at index 1
                    game.Inventory.DropItem(1);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D3) && game.previousKeyboardState.IsKeyUp(Keys.D3))
                {
                    //Drop item at index 2
                    game.Inventory.DropItem(2);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D4) && game.previousKeyboardState.IsKeyUp(Keys.D4))
                {
                    //Drop item at index 3
                    game.Inventory.DropItem(3);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D5) && game.previousKeyboardState.IsKeyUp(Keys.D5))
                {
                    //Drop item at index 4
                    game.Inventory.DropItem(4);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D6) && game.previousKeyboardState.IsKeyUp(Keys.D6))
                {
                    //Drop item at index 5
                    game.Inventory.DropItem(5);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D7) && game.previousKeyboardState.IsKeyUp(Keys.D7))
                {
                    //Drop item at index 6
                    game.Inventory.DropItem(6);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D8) && game.previousKeyboardState.IsKeyUp(Keys.D8))
                {
                    //Drop item at index 7
                    game.Inventory.DropItem(7);
                }
                if (game.currentKeyboardState.IsKeyDown(Keys.D9) && game.previousKeyboardState.IsKeyUp(Keys.D9))
                {
                    //Drop item at index 8
                    game.Inventory.DropItem(8);
                }
            }
            //testing display stats
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F1) && game.previousKeyboardState.IsKeyUp(Keys.F1)))
            {
                //Display stats of first item (index 0)
                game.Inventory.DisplayStats(0);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F2) && game.previousKeyboardState.IsKeyUp(Keys.F2)))
            {
                //Display stats of second item (index 1)
                game.Inventory.DisplayStats(1);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F3) && game.previousKeyboardState.IsKeyUp(Keys.F3)))
            {
                //Display stats of third item (index 2)
                game.Inventory.DisplayStats(2);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F4) && game.previousKeyboardState.IsKeyUp(Keys.F4)))
            {
                //Display stats of fourth item (index 3)
                game.Inventory.DisplayStats(3);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F5) && game.previousKeyboardState.IsKeyUp(Keys.F5)))
            {
                //Display stats of fifth item (index 4)
                game.Inventory.DisplayStats(4);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F6) && game.previousKeyboardState.IsKeyUp(Keys.F6)))
            {
                //Display stats of sixth item (index 5)
                game.Inventory.DisplayStats(5);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F7) && game.previousKeyboardState.IsKeyUp(Keys.F7)))
            {
                //Display stats of seventh item (index 6)
                game.Inventory.DisplayStats(6);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F8) && game.previousKeyboardState.IsKeyUp(Keys.F8)))
            {
                //Display stats of eighth item (index 7)
                game.Inventory.DisplayStats(7);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.F9) && game.previousKeyboardState.IsKeyUp(Keys.F9)))
            {
                //Display stats of ninth item (index 8)
                game.Inventory.DisplayStats(8);
            }
            //testing numbers
            else if (game.currentKeyboardState.IsKeyDown(Keys.D1) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D1))
            {
                game.Inventory.UseItem(1);

            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D2) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D2))
            {
                game.Inventory.UseItem(2);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D3) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D3))
            {
                game.Inventory.UseItem(3);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D4) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D4))
            {
                game.Inventory.UseItem(4);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D5) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D5))
            {
                game.Inventory.UseItem(5);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D6) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D6))
            {
                game.Inventory.UseItem(6);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D7) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D7))
            {
                game.Inventory.UseItem(7);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D8) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D8))
            {
                game.Inventory.UseItem(8);
            }
            else if (game.currentKeyboardState.IsKeyDown(Keys.D9) &&
                    game.previousKeyboardState.IsKeyUp(Keys.D9))
            {
                game.Inventory.UseItem(9);
            }
            else if ((game.currentKeyboardState.IsKeyDown(Keys.OemPeriod) &&
                    game.previousKeyboardState.IsKeyUp(Keys.OemPeriod)) || 
                    (game.currentKeyboardState.IsKeyDown(Keys.NumPad5) &&
                    game.previousKeyboardState.IsKeyUp(Keys.NumPad5)))
            {
                if (game.Level.LevelExit.X == X && game.Level.LevelExit.Y == Y)
                {
                    game.mapLevel++;
                    game.GenerateLevel();
                    game.Messages.AddMessage("You have descended to level " + game.mapLevel + ".");
                }
                else
                {
                    game.Messages.AddMessage("You must be on the stairs to go down a level!");
                }
            }
            LoseMovement();
            // save current state to previous and get ready for next move
            game.previousKeyboardState = game.currentKeyboardState;
        }

        // Attack(...)
        // DESC:    Play out monster attack sequence.
        // PARAMS:  Monster instance.
        // RETURNS: None.
        public void Attack(Monster monster)
        {
            if(LoseTurn == false)
            { 
                game.Messages.AddMessage("You attack the " + monster.Name + "!!!");

                // calculate rolls for battle
                int hitRoll = GameManager.Random.Next(1, 20);
                int defenseRoll = GameManager.Random.Next(1, 10);

                // calculate attack & defense rolls
                int hitValue = hitRoll + GetHitBonus();
                int defenseValue = defenseRoll + monster.getDodgeBonus();

                // Player wins tie
                if (hitValue < defenseValue)
                {
                    game.DodgeAttack.Play();
                    game.Messages.AddMessage(monster.Name + " dodges hit!");
                    return;
                }

                // calculate base Player dmg
                int dmgRoll = GameManager.Random.Next(MinDamage, MaxDamage);
                int dmgValue = 2 * dmgRoll + GetDamageBonus();

                // calculate total dmg
                int mitigationValue = GameManager.Random.Next(MinDamage, MaxDamage);
                int totalDmg = dmgValue - mitigationValue;

                if (totalDmg <= 0)
                {
                    game.BlockAttack.Play();
                    game.Messages.AddMessage(monster.Name + " blocks attack!");
                    return;
                }

                // inflict dmg on Capstonian   
                game.PlayRandomFromList(game.MonsterHit);
                game.Messages.AddMessage("Player inflicts " + totalDmg + " dmg on " + monster.Name);
                monster.CurrHealth -= totalDmg;

                if (monster.CurrHealth <= 0)
                {
                    game.HandleMonsterDeath(monster);
                }
             }
        }

        // CheckLevelUp()
        // DESC:    Check Player experience to determine if they have levelled up
        // PARAMS:  None
        // RETURNS: None
        public void CheckLevelUp()
        {

            if(Experience >= CurrentExperienceMax)
            {
                game.LevelUp.Play();
                Level++;
                Experience -= CurrentExperienceMax;
                CurrentExperienceMax *= 2;

                MaxHealth += GameManager.Random.Next(3, 12) + GetConstitutionBonus();

                //Restore health completely - can be changed later
                CurrHealth = MaxHealth;

                game.Messages.AddMessage("============================");
                game.Messages.AddMessage("You are now level " + Level + "!!!");
            }
        }

        // DrawStats(...)
        // DESC:    Draw Player stats to screen.
        // PARAMS:  SpriteBatch instance.
        // RETURNS: None.
        public void DrawStats(SpriteBatch spriteBatch)
        {

            const int iconVertOffset = 50; // center icon vertically in grid cell
            const int iconHorizOffset = 60; // center icon horizontally in grid cell
            const int textVertOffset = 18; // offset for text
            const int textHorizOffset = 240; // offset for text
            const int gridVertOffset = 201; // offset for grid
            const int gridHorizOffset = 672; // offset for grid
            int fudgeFactorIcon; // pixel offset value to center icons
            int fudgeFactorScore = 18; // pixel offset to center text
            int iteration = 2; // iterate for each block

            // draw stats outline
            spriteBatch.Draw(game.PlayerStatsOutline, new Vector2(gridHorizOffset, gridVertOffset), Color.White);

            // draw title
            int horiztOffsetForTitle = 780;
            fudgeFactorScore = 18;
            spriteBatch.DrawString(game.mainFont, "PLAYER STATS", new Vector2(horiztOffsetForTitle, gridVertOffset + fudgeFactorScore), Color.White);

            // draw health
            spriteBatch.Draw(game.health, new Vector2(gridHorizOffset, gridVertOffset + iconVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, "Health", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iconVertOffset + textVertOffset), Color.White);

            //Can't have negative health
            if(CurrHealth < 0)
            {
                CurrHealth = 0;
            }
            spriteBatch.DrawString(game.mainFont, CurrHealth.ToString() + "/" + MaxHealth.ToString(), new Vector2(gridHorizOffset + textHorizOffset, gridVertOffset + iconVertOffset + textVertOffset), Color.White);

            // draw level
            fudgeFactorScore = 25;
            spriteBatch.Draw(game.level, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, "Level", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Level.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // experience level
            fudgeFactorIcon = 1;
            fudgeFactorScore = -5;
            spriteBatch.Draw(game.experience, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset + fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Experience", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Experience.ToString() + "/" + CurrentExperienceMax, new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // strength level
            fudgeFactorScore = 18;
            fudgeFactorIcon = 3;
            spriteBatch.Draw(game.strength, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset + fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Strength", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Strength.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // dexterity level
            fudgeFactorScore = 18;
            fudgeFactorIcon = 2;
            spriteBatch.Draw(game.dexterity, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset - fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Dexterity", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Dexterity.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // constitution level
            fudgeFactorIcon = 2;
            fudgeFactorScore = 18;
            spriteBatch.Draw(game.constitution, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset - fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Constitution", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Constitution.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
        }

        // DrawEquipment(...)
        // DESC:    Draw Player equipment to screen.
        // PARAMS:  SpriteBatch instance.
        // RETURNS: None.
        public void DrawEquipment(SpriteBatch spriteBatch)
        {
            const int iconVertOffset = 50; // center icon vertically in grid cell
            const int iconHorizOffset = 60; // center icon horizontally in grid cell
            const int textVertOffset = 18; // offset for text
            const int textHorizOffset = 240; // offset for text
            const int gridVertOffset = 502; // offset for grid
            const int gridHorizOffset = 672; // offset for grid
            int fudgeFactorIcon; // pixel offset value to center icons
            int fudgeFactorScore = 18; // pixel offset to center text
            int iteration = 2; // iterate for each block

            // draw stats outline
            spriteBatch.Draw(game.PlayerEquipmentOutline, new Vector2(gridHorizOffset, gridVertOffset), Color.White);

            // draw title
            int horiztOffsetForTitle = 760;
            spriteBatch.DrawString(game.mainFont, "PLAYER EQUIPMENT", new Vector2(horiztOffsetForTitle, gridVertOffset + fudgeFactorScore), Color.White);

            // draw armor
            if (game.Equip.Equip["Armor"] == null)
            {
                spriteBatch.Draw(game.armor, new Vector2(gridHorizOffset, gridVertOffset + iconVertOffset), Color.White);
            }
            else
            {
                spriteBatch.Draw(game.Equip.Equip["Armor"].Sprite, new Vector2(gridHorizOffset, gridVertOffset + iconVertOffset), Color.White);

            }
            spriteBatch.DrawString(game.mainFont, ArmorType, new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, "+" + ArmorValue.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iconVertOffset + textVertOffset), Color.White);

            // draw weapon
            fudgeFactorIcon = 3;
            if (game.Equip.Equip["Weapon"] == null)
            {
                spriteBatch.Draw(game.weapon_club, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset + fudgeFactorIcon), Color.White);
            }
            else
            {
                spriteBatch.Draw(game.Equip.Equip["Weapon"].Sprite, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset + fudgeFactorIcon), Color.White);

            }
            spriteBatch.DrawString(game.mainFont, WeaponType, new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, "+" + WeaponValue.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White); //NEW - UNCOMMENTED
            //spriteBatch.DrawString(game.mainFont, "+" + Strength.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White); //NEW - COMMENTED
            ++iteration; // offset for next block
        }


        // HungerWarning()
        // DESC:    Broadcasts a message if the player's hunger level is at certain levels
        // PARAMS:  None
        // RETURNS: None
        private void HungerWarning()
        {
            if (Hunger == 50 && OldHunger != 50)
            {
                OldHunger = Hunger;
                game.Messages.AddMessage("You start to feel hungry.  Stats decreased by 10%");
            }
            else if (Hunger == 25 && OldHunger != 25)
            {
                OldHunger = Hunger;
                game.Messages.AddMessage("You start to feel the onset of starvation.  Stats decreased by 50%");
            }
            else if (Hunger == 0 && OldHunger != 0)
            {
                OldHunger = Hunger;
                game.Messages.AddMessage("You are famished. Stats decreased by 90%");
            }
            else if(Hunger > 50 && OldHunger <= 50)
            {
                OldHunger = Hunger;
                game.Messages.AddMessage("You feel a lot stronger after consuming some food");
                game.Messages.AddMessage("Stats restored to 100%");
            }
            else if (Hunger > 25 && OldHunger <= 25)
            {
                OldHunger = Hunger;
                game.Messages.AddMessage("You feel a little bit stronger after consuming some food");
                game.Messages.AddMessage("Stats restored to 90%");

            }
            else if (Hunger > 0 && OldHunger == 0)
            {
                OldHunger = Hunger;
                game.Messages.AddMessage("You feel a tiny bit stronger after consuming some food");
                game.Messages.AddMessage("Stats restored to 50%");

            }
        }

        // HungerStat()
        // DESC:    Determines the new value of stats after hunger penalty is changed
        // PARAMS:  None
        // RETURNS: None
        private void HungerStat()
        {
            if (NewHungerPenalty != OldHungerPenalty)
            {

                //Update constitution, dexterity, and strength accordingly
                Constitution = (int)(BaseConstitution * NewHungerPenalty);
                game.Messages.AddMessage("Constitution: " + Constitution + " HungerPenalty: " + NewHungerPenalty);
                Dexterity = (int)(BaseDexterity * NewHungerPenalty);
                Strength = (int)(BaseStrength * NewHungerPenalty);

                //Set old penalty to new penalty for next check
                OldHungerPenalty = NewHungerPenalty;
            }
        }

        // HungerUpdate();
        // DESC:    Decrements hunger with each step. If hunger is 0, 1/4 chance to lose turn
        // PARAMS:  None
        // RETURNS: None
        private void HungerUpdate()
        {
            int x;
            //Only Decrement Hunger if Player has moved// 
            if (oldPositionX != this.X || oldPositionY != this.Y)
            {
                //1 in 4 chance to decrement Hunger per turn
                x = Capstonia.GameManager.Random.Next(0, 4);
                if (x == 0)
                {
                    Hunger -= 1;
                }

                if (Hunger < MinHunger)
                {
                    Hunger = MinHunger;
                }

                //Draw Hunger to screen
                //game.Messages.AddMessage("Hunger: " + Hunger);
            }

            //Give player 1 in 4 chance of losing turn if hunger is 0
            if( Hunger == 0)
            {
                x = Capstonia.GameManager.Random.Next(0, 4);
                if (x == 0)
                {
                    LoseTurn = true;
                    game.Messages.AddMessage("You are too hungry to move. Lose a turn");
                }

            }
            else
            {
                LoseTurn = false;
            }
        }

        // LoseMovement()
        // DESC:    If player has lsot a turn, reset attempted move to previos position and loop monster movement
        // PARAMS:  None
        // RETURNS: None
        private void LoseMovement()
        {
            if (LoseTurn)
            {
                game.Level.SetActorPosition(this, oldPositionX, oldPositionY);
                LoseTurn = false;
                foreach (Monster minion in game.Monsters)
                {
                   if(game.IsInRoomWithPlayer(minion.X, minion.Y))
                   {
                        minion.FindPath();
                   }
                }

            }
        }
    }
}
