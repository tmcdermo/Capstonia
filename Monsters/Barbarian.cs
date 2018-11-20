using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Barbarian : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Barbarian(GameManager game) : base(game)
        {
            Level = 7;
            // every point above 10 gives a health bonus
            Constitution = 12;
            // every point above 10 gives a dodge bonus
            Dexterity = 11;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 17;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 17;
            // max dmg Capstonian can cause
            MaxDamage = 13;
            // min dmg Capstonain can cause
            MinDamage = 4;
            // name of monster
            Name = "Barbarian";
            // every point above 10 gives a dmg bonus
            Strength = 15;

            MinGlory = 7;
            MaxGlory = 11;
            Sprite = game.barbarian;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}