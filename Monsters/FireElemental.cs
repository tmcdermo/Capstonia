using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class FireElemental : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public FireElemental(GameManager game) : base(game)
        {
            Level = 7;
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 13;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 19;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 19;
            // max dmg Capstonian can cause
            MaxDamage = 15;
            // min dmg Capstonain can cause
            MinDamage = 9;
            // name of monster
            Name = "Fire Elemental";
            // every point above 10 gives a dmg bonus
            Strength = 10;

            MinGlory = 8;
            MaxGlory = 12;
            Sprite = game.fireelemental;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}