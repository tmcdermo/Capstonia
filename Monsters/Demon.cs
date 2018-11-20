using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Demon : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Demon(GameManager game) : base(game)
        {
            Level = 10;
            // every point above 10 gives a health bonus
            Constitution = 15;
            // every point above 10 gives a dodge bonus
            Dexterity = 18;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 10;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 10;
            // max dmg Capstonian can cause
            MaxDamage = 35;
            // min dmg Capstonain can cause
            MinDamage = 25;
            // name of monster
            Name = "Demon";
            // every point above 10 gives a dmg bonus
            Strength = 18;

            MinGlory = 18;
            MaxGlory = 25;
            Sprite = game.demon;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}