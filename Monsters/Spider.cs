using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Spider : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Spider(GameManager game) : base(game)
        {
            Level = 3;
            // every point above 10 gives a health bonus
            Constitution = 11;
            // every point above 10 gives a dodge bonus
            Dexterity = 13;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 13;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 13;
            // max dmg Capstonian can cause
            MaxDamage = 5;
            // min dmg Capstonain can cause
            MinDamage = 3;
            // name of monster
            Name = "Giant Spider";
            // every point above 10 gives a dmg bonus
            Strength = 11;

            MinGlory = 3;
            MaxGlory = 6;
            Sprite = game.spider;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}