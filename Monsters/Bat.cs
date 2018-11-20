using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Bat : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Bat(GameManager game) : base(game)
        {
            Level = 1;
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 15;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 5;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 5;
            // max dmg Capstonian can cause
            MaxDamage = 2;
            // min dmg Capstonain can cause
            MinDamage = 1;
            // name of monster
            Name = "Giant Bat";
            // every point above 10 gives a dmg bonus
            Strength = 10;

            MinGlory = 1;
            MaxGlory = 2;
            Sprite = game.bat;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}