using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Capstonia.Core
{
    public class Player : Actor
    {
        // TODO - create armor class?
        private string armorType; // e.g., leather, chainmail, plate
        public string ArmorType { get; set; }
        private int armorValue; // e.g., leather, chainmail, plate
        public int ArmorValue { get; set; }
        public int Experience { get; set; } // track progress until next level
        private int hunger; // 0 = full, 100 = starving
        public int Hunger { get; set; }
        // TODO - create weapon class?
        private string weaponType; // e.g., leather, chainmail, plate
        public string WeaponType { get; set; }
        private int weaponValue; // e.g., leather, chainmail, plate
        public int WeaponValue { get; set; }
        private int maxhealth; // cap to heal to until increased etc.
        public int MaxHealth { get; set; }
        private int maxhunger;
        public int MaxHunger { get; set; }
        public int Glory { get; set; }

        public int getHitBonus()
        {
            return Dexterity - game.BaseDexterity;
        }
        public int getDodgeBonus()
        {
            return Dexterity - game.BaseDexterity;
        }

        public int getDamageBonus()
        {
            return Strength - game.BaseStrength;
        }

            // constructor
            public Player(GameManager game) : base(game)
        {
            ArmorType = "Leather Jerkin";
            ArmorValue = 0; // used in dmg calc during battle
            Constitution = 10; // every point above 10 gives a health bonus
            Dexterity = 10; // every point above 10 gives a dodge bonus
            Health = 50; // Health total for Player.  If the values reaches 0, the player is killed
            MaxHealth = 50; // can grow with constitution
            Health = 100; // initial health value (out of 100)
            Hunger = 100; // 0 = full, 100 = starving
            Level = 0; // 0 = min, 100 = max
            MaxHunger = 100; // cap to not over-feed
            MaxDamage = 5; // max dmg Player can cause
            MinDamage = 1; // min dmg Player can cause
            Name = "Villain"; // name of Player
            Strength = 10;  // every point above 10 gives a dmg bonus
            WeaponType = "Club";
            WeaponValue = 2;  // used in dmg calc during battle
            Glory = 0;
        }

        // CalculateHungerPenalty
        //
        // DESC: Inintially the player is full when the game starts, so their
        // hunger value is at 0.  With each move, the player's hunger value is
        // increased by 1.  To lower their hunger, the player should try to
        // acquire food from the gameboard.  If a player's hunger increases to
        // 50 and above (max = 100), their strength, dexterity, and constitution
        // will be impacted during combat (see below).  This will continue until
        // their hunger value drops back below 50.
        //
        // PARAMS: N/A
        public float CalculateHungerPenalty()
        {
            float hungerPenalty = 1.0f;

            if (this.Hunger == 100)
            {
                hungerPenalty = 0.10f; // multiplier for str, dex, const
            }
            else if (this.Hunger > 0 && this.Hunger <= 75)
            {
                hungerPenalty = 0.75f; // multiplier for str, dex, const
            }
            else if (this.Hunger > 25 && this.Hunger <= 50)
            {
                hungerPenalty = 0.50f; // multiplier for str, dex, const
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

            // get current keyboard state
            game.currentKeyboardState = Keyboard.GetState();

            // move player up
            if (game.currentKeyboardState.IsKeyDown(Keys.Down) && 
                game.previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (game.Level.IsWalkable(game.Player.X, game.Player.Y + 1))
                {
                    //game.Player.Y += 1;
                    game.Level.SetActorPosition(this, X, Y + 1);
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X, game.Player.Y + 1);
                    if(monster != null)
                    {
                        Attack(monster);
                    }
                }
            } // move player down
            else if (game.currentKeyboardState.IsKeyDown(Keys.Up) &&
                     game.previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (game.Level.IsWalkable(game.Player.X, game.Player.Y - 1))
                {
                    //game.Player.Y -= 1;
                    game.Level.SetActorPosition(this, X, Y - 1);
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X, game.Player.Y - 1);
                    if (monster != null)
                    {
                        Attack(monster);
                    }
                }
            } // move player left
            else if (game.currentKeyboardState.IsKeyDown(Keys.Left) &&
                     game.previousKeyboardState.IsKeyUp(Keys.Left))
            {
                if (game.Level.IsWalkable(game.Player.X - 1, game.Player.Y))
                {                 
                    //game.Player.X -= 1;
                    game.Level.SetActorPosition(this, X - 1, Y);
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X - 1, game.Player.Y);
                    if (monster != null)
                    {
                        Attack(monster);
                    }
                }
            } // move player right
            else if (game.currentKeyboardState.IsKeyDown(Keys.Right) &&
                     game.previousKeyboardState.IsKeyUp(Keys.Right))
            {
                if (game.Level.IsWalkable(game.Player.X + 1, game.Player.Y))
                {
                    //game.Player.X += 1;
                    game.Level.SetActorPosition(this, X + 1, Y);
                }
                else
                {
                    monster = game.Level.IsMonster(game.Player.X + 1, game.Player.Y);
                    if (monster != null)
                    {
                        Attack(monster);
                    }
                }
            }
            //testing numbers
            else if(game.currentKeyboardState.IsKeyDown(Keys.D1)&&
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

            // save current state to previous and get ready for next move
            game.previousKeyboardState = game.currentKeyboardState;
        }

        public void Attack(Monster monster)
        {
            game.Messages.AddMessage("You attack the " + monster.Name + "!!!");

            // calculate rolls for battle
            int hitRoll = GameManager.Random.Next(1, 20);
            int defenseRoll =  GameManager.Random.Next(1, 20);

            // calculate attack & defense rolls
            int hitValue = hitRoll + getHitBonus();
            int defenseValue = defenseRoll + monster.getDodgeBonus();

            // Player wins tie
            if (hitValue < defenseValue)
            {
                game.Messages.AddMessage(monster.Name + " dodges hit!");
                return;
            }

            // calculate base Player dmg
            int dmgRoll =  GameManager.Random.Next(MinDamage, MaxDamage);
            int dmgValue = dmgRoll + getDamageBonus();

            // calculate total dmg
            int totalDmg = dmgValue - defenseValue;

            if (totalDmg <= 0)
            {
                game.Messages.AddMessage(monster.Name + " blocks attack!");
                return;
            }

            // inflict dmg on Capstonian
            game.Messages.AddMessage("Player inflicts " + totalDmg + " dmg on " + monster.Name);
            monster.Health -= totalDmg;

            if (monster.Health <= 0)
            {
                game.HandleMonsterDeath(monster);
            }
        }


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
            spriteBatch.DrawString(game.mainFont, "PLAYER STATS", new Vector2(horiztOffsetForTitle, gridVertOffset + fudgeFactorScore), Color.White);

            // draw health
            spriteBatch.Draw(game.health, new Vector2(gridHorizOffset, gridVertOffset + iconVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, "Health", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Health.ToString() + "/100", new Vector2(gridHorizOffset + textHorizOffset, gridVertOffset + iconVertOffset + textVertOffset), Color.White);

            // draw level
            spriteBatch.Draw(game.level, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, "Level", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Level.ToString() + "/100", new Vector2(gridHorizOffset + textHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // experience level
            fudgeFactorIcon = 1;
            spriteBatch.Draw(game.experience, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset + fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Experience", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Experience.ToString() + "/100", new Vector2(gridHorizOffset + textHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // strength level
            fudgeFactorIcon = 3;
            spriteBatch.Draw(game.strength, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset + fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Strength", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Strength.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // dexterity level
            fudgeFactorIcon = 2;
            spriteBatch.Draw(game.dexterity, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset - fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Dexterity", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Dexterity.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            ++iteration; // offset for next block

            // constitution level
            fudgeFactorIcon = 2;
            spriteBatch.Draw(game.constitution, new Vector2(gridHorizOffset, gridVertOffset + iteration * iconVertOffset - fudgeFactorIcon), Color.White);
            spriteBatch.DrawString(game.mainFont, "Constitution", new Vector2(gridHorizOffset + iconHorizOffset, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
            spriteBatch.DrawString(game.mainFont, Constitution.ToString(), new Vector2(gridHorizOffset + textHorizOffset + fudgeFactorScore, gridVertOffset + iteration * iconVertOffset + textVertOffset), Color.White);
        }
    }
}
