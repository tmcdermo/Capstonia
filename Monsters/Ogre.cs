using Capstonia.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Rectangle = RogueSharp.Rectangle;

namespace Capstonia.Monsters
{
    public class Ogre : Monster
    {
        int oldPlayerX;
        int oldPlayerY;
        // constructor
        public Ogre(GameManager game) : base(game)
        {
            Level = 7;
            // every point above 10 gives a health bonus
            Constitution = 16;
            // every point above 10 gives a dodge bonus
            Dexterity = 7;
            // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxHealth = 22;
            // current health for Capstonian; if the values reaches 0, the Capstonain is killed
            CurrHealth = 22;
            // max dmg Capstonian can cause
            MaxDamage = 15;
            // min dmg Capstonain can cause
            MinDamage = 6;
            // name of monster
            Name = "Ogre";
            // every point above 10 gives a dmg bonus
            Strength = 16;

            MinGlory = 9;
            MaxGlory = 12;
            Sprite = game.ogre;
            oldPlayerX = game.Player.X;
            oldPlayerY = game.Player.Y;
            
        }
    }
}