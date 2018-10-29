using Capstonia.Controller;
using Capstonia.Monsters;

namespace Capstonia.Core
{
    public class Monster : Actor
    {
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }

        // constructor
        public Monster(GameManager game) : base(game)
        {
            Constitution = 10; // every point above 10 gives a health bonus
            Dexterity = 10; // every point above 10 gives a dodge bonus
            Health = 50; // health total for Capstonian; if the values reaches 0, the Capstonain is killed
            MaxDamage = 3; // max dmg Capstonian can cause
            MinDamage = 1; // min dmg Capstonain can cause
            Name = "Minstrel"; // name of Capstonian
            Strength = 10;  // every point above 10 gives a dmg bonus
            MinLevel = 1;
            MaxLevel = 3;
        }
    }
}
