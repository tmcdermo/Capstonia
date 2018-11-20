using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Dragon : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Dragon(GameManager game) : base(game)
        {
            Level = 10;
            // every point above 10 gives a health bonus
            Constitution = 18;
            // every point above 10 gives a dodge bonus
            Dexterity = 10;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 30;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 30;
            // max dmg Capstonian can cause
            MaxDamage = 40;
            // min dmg Capstonain can cause
            MinDamage = 15;
            // name of monster
            Name = "Dragon";
            // every point above 10 gives a dmg bonus
            Strength = 20;

            MinGlory = 18;
            MaxGlory = 25;
            Sprite = game.dragon;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}