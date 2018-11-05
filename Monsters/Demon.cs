﻿using Capstonia.Core;
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
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 10;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 10;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 10;
            // max dmg Capstonian can cause
            MaxDamage = 3;
            // min dmg Capstonain can cause
            MinDamage = 1;
            // name of monster
            Name = "Demon";
            // every point above 10 gives a dmg bonus
            Strength = 10;
            Level = 10;
            MinGlory = 1;
            MaxGlory = 3;
            Sprite = game.demon;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}