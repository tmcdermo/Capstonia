using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Wolf : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Wolf(GameManager game) : base(game)
        {
            Level = 4;
            // every point above 10 gives a health bonus
            Constitution = 11;
            // every point above 10 gives a dodge bonus
            Dexterity = 14;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 12;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 12;
            // max dmg Capstonian can cause
            MaxDamage = 7;
            // min dmg Capstonain can cause
            MinDamage = 3;
            // name of monster
            Name = "Wolf";
            // every point above 10 gives a dmg bonus
            Strength = 11;

            MinGlory = 4;
            MaxGlory = 6;
            Sprite = game.wolf;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}