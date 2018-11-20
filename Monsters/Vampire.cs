using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Vampire : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Vampire(GameManager game) : base(game)
        {
            Level = 9;
            // every point above 10 gives a health bonus
            Constitution = 16;
            // every point above 10 gives a dodge bonus
            Dexterity = 15;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 25;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 25;
            // max dmg Capstonian can cause
            MaxDamage = 16;
            // min dmg Capstonain can cause
            MinDamage = 3;
            // name of monster
            Name = "Vampire";
            // every point above 10 gives a dmg bonus
            Strength = 12;

            MinGlory = 12;
            MaxGlory = 15;
            Sprite = game.vampire;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}