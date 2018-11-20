using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Beholder : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Beholder(GameManager game) : base(game)
        {
            Level = 5;
            // every point above 10 gives a health bonus
            Constitution = 12;
            // every point above 10 gives a dodge bonus
            Dexterity = 12;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 16;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 16;
            // max dmg Capstonian can cause
            MaxDamage = 10;
            // min dmg Capstonain can cause
            MinDamage = 1;
            // name of monster
            Name = "Beholder";
            // every point above 10 gives a dmg bonus
            Strength = 10;

            MinGlory = 5;
            MaxGlory = 9;
            Sprite = game.beholder;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}