using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Lich : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Lich(GameManager game) : base(game)
        {
            Level = 9;
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 12;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 18;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 18;
            // max dmg Capstonian can cause
            MaxDamage = 20;
            // min dmg Capstonain can cause
            MinDamage = 15;
            // name of monster
            Name = "Lich";
            // every point above 10 gives a dmg bonus
            Strength = 17;

            MinGlory = 14;
            MaxGlory = 16;
            Sprite = game.lich;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}