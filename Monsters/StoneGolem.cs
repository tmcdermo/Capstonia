using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class StoneGolem : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public StoneGolem(GameManager game) : base(game)
        {
            Level = 6;
            // every point above 10 gives a health bonus
            Constitution = 15;
            // every point above 10 gives a dodge bonus
            Dexterity = 6;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 20;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 20;
            // max dmg Capstonian can cause
            MaxDamage = 11;
            // min dmg Capstonain can cause
            MinDamage = 7;
            // name of monster
            Name = "Stone Golem";
            // every point above 10 gives a dmg bonus
            Strength = 16;

            MinGlory = 6;
            MaxGlory = 10;
            Sprite = game.stonegolem;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}