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
            Name = "Valkyrie";
            // every point above 10 gives a dmg bonus
            Strength = 10;
            Level = 8;
            MinGlory = 1;
            MaxGlory = 3;
            Sprite = game.valkyrie;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}