using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Wraith : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Wraith(GameManager game) : base(game)
        {
            Level = 8;
            // every point above 10 gives a health bonus
            Constitution = 12;
            // every point above 10 gives a dodge bonus
            Dexterity = 20;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 20;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 20;
            // max dmg Capstonian can cause
            MaxDamage = 16;
            // min dmg Capstonain can cause
            MinDamage = 4;
            // name of monster
            Name = "Wraith";
            // every point above 10 gives a dmg bonus
            Strength = 12;

            MinGlory = 9;
            MaxGlory = 13;
            Sprite = game.wraith;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}