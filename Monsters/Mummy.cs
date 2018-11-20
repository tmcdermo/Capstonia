using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Mummy : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Mummy(GameManager game) : base(game)
        {
            Level = 4;
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 9;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 15;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 15;
            // max dmg Capstonian can cause
            MaxDamage = 6;
            // min dmg Capstonain can cause
            MinDamage = 4;
            // name of monster
            Name = "Mummy";
            // every point above 10 gives a dmg bonus
            Strength = 13;

            MinGlory = 4;
            MaxGlory = 6;
            Sprite = game.mummy;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}