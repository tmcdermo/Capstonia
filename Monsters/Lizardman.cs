using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Lizardman : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Lizardman(GameManager game) : base(game)
        {
            Level = 3;
            // every point above 10 gives a health bonus
            Constitution = 13;
            // every point above 10 gives a dodge bonus
            Dexterity = 10;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 12;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 12;
            // max dmg Capstonian can cause
            MaxDamage = 6;
            // min dmg Capstonain can cause
            MinDamage = 3;
            // name of monster
            Name = "Lizardman";
            // every point above 10 gives a dmg bonus
            Strength = 10;

            MinGlory = 3;
            MaxGlory = 6;
            Sprite = game.lizardman;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;            
        }
    }
}