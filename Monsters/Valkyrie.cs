using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Valkyrie : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Valkyrie(GameManager game) : base(game)
        {
            Level = 8;
            // every point above 10 gives a health bonus
            Constitution = 10;
            // every point above 10 gives a dodge bonus
            Dexterity = 14;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 24;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 24;
            // max dmg Capstonian can cause
            MaxDamage = 16;
            // min dmg Capstonain can cause
            MinDamage = 10;
            // name of monster
            Name = "Valkyrie";
            // every point above 10 gives a dmg bonus
            Strength = 15;

            MinGlory = 10;
            MaxGlory = 14;
            Sprite = game.valkyrie;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}