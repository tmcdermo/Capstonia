﻿using Microsoft.Xna.Framework.Input;

namespace Capstonia.Core
{
    public class Player : Actor
    {
        // TODO - create armor class?
        private string armorType; // e.g., leather, chainmail, plate
        public string ArmorType { get { return armorType; } set { armorType = value; } }
        private int armorValue; // e.g., leather, chainmail, plate
        public int ArmorValue { get { return armorValue; } set { armorValue = value; } }
        private int hunger; // 0 = full, 100 = starving
        public int Hunger { get { return hunger; } set { hunger = value; } }
        // TODO - create weapon class?
        private string weaponType; // e.g., leather, chainmail, plate
        public string WeaponType { get { return weaponType; } set { weaponType = value; } }
        private int weaponValue; // e.g., leather, chainmail, plate
        public int WeaponValue { get { return weaponValue; } set { weaponValue = value; } }
        private int maxhealth; // cap to heal to until increased etc.
        public int MaxHealth { get { return maxhealth; } set { maxhealth = value; } }
        private int maxhunger;
        public int MaxHunger { get { return maxhunger; } set { maxhunger = value; } }
        // constructor
        public Player(GameManager game) : base(game)
        {
            ArmorType = "Leather Jerkin";
            ArmorValue = 0; // used in dmg calc during battle
            Constitution = 10; // every point above 10 gives a health bonus
            Dexterity = 10; // every point above 10 gives a dodge bonus
            Health = 50; // Health total for Player.  If the values reaches 0, the player is killed
            MaxHealth = 50; // can grow with constitution
            Hunger = 100; // 0 = full, 100 = starving
            MaxHunger = 100; // cap to not over-feed
            MaxDamage = 5; // max dmg Player can cause
            MinDamage = 1; // min dmg Player can cause
            Name = "Villain"; // name of Player
            Strength = 10;  // every point above 10 gives a dmg bonus
            WeaponType = "Club";
            WeaponValue = 2;  // used in dmg calc during battle
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
        // PARAMS:  GameManager instance
        // RETURNS: None.
        public void Move(GameManager game)
        {
            // get current keyboard state
            game.currentKeyboardState = Keyboard.GetState();

            // move player up
            if (game.currentKeyboardState.IsKeyDown(Keys.Down) && 
                game.previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (game.Level.IsWalkable(game.Player.X, game.Player.Y + 1))
                {
                    game.Player.Y += 1;
                }
            } // move player down
            else if (game.currentKeyboardState.IsKeyDown(Keys.Up) &&
                     game.previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (game.Level.IsWalkable(game.Player.X, game.Player.Y - 1))
                {
                    game.Player.Y -= 1;
                }
            } // move player left
            else if (game.currentKeyboardState.IsKeyDown(Keys.Left) &&
                     game.previousKeyboardState.IsKeyUp(Keys.Left))
            {
                if (game.Level.IsWalkable(game.Player.X - 1, game.Player.Y))
                {
                    game.Player.X -= 1;
                }
            } // move player right
            else if (game.currentKeyboardState.IsKeyDown(Keys.Right) &&
                     game.previousKeyboardState.IsKeyUp(Keys.Right))
            {
                if (game.Level.IsWalkable(game.Player.X + 1, game.Player.Y))
                {
                    game.Player.X += 1;
                }
            }

            // save current state to previous and get ready for next move
            game.previousKeyboardState = game.currentKeyboardState;
        }
    }
}
