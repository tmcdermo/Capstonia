using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Skeleton : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Skeleton(GameManager game) : base(game)
        {
            Level = 3;
            // every point above 10 gives a health bonus
            Constitution = 7;
            // every point above 10 gives a dodge bonus
            Dexterity = 12;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 10;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 10;
            // max dmg Capstonian can cause
            MaxDamage = 5;
            // min dmg Capstonain can cause
            MinDamage = 1;
            // name of monster
            Name = "Skeleton";
            // every point above 10 gives a dmg bonus
            Strength = 10;

            MinGlory = 3;
            MaxGlory = 5;
            Sprite = game.skeleton;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}