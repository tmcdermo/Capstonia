using Capstonia.Controller;
using Capstonia.Monsters;

namespace Capstonia.Core
{
    public class Monster : Actor
    {
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int MinGlory { get; set; }
        public int MaxGlory { get; set; }

        public virtual int getHitBonus()
        {
            return Dexterity - game.BaseDexterity;
        }
        public virtual int getDodgeBonus()
        {
            return Dexterity - game.BaseDexterity;
        }

        public virtual int getDamageBonus()
        {
            return Strength - game.BaseStrength;
        }

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
            MinGlory = 1;
            MaxGlory = 3;
        }

        public void Attack()
        {
            game.Messages.AddMessage(Name + " attacks YOU!");

            // calculate rolls for battle
            int hitRoll = GameManager.Random.Next(1, 20);
            int defenseRoll = GameManager.Random.Next(1, 20);

            // calculate attack & defense rolls
            int hitValue = hitRoll + getHitBonus();
            int defenseValue = defenseRoll + game.Player.getDodgeBonus();

            // Player wins tie
            if (hitValue < defenseValue)
            {
                game.Messages.AddMessage("You dodge the " + Name + "'s attack!");
                return;
            }

            // calculate base Player dmg
            int dmgRoll = GameManager.Random.Next(MinDamage, MaxDamage);
            int dmgValue = dmgRoll + getDamageBonus();

            // calculate total dmg
            int totalDmg = dmgValue - defenseValue;

            if (totalDmg <= 0)
            {
                game.Messages.AddMessage("You block the " + Name + "'s attack!");
                return;
            }

            // inflict dmg on Capstonian
            game.Messages.AddMessage(Name + " inflicts " + totalDmg + " dmg on you!! ");
            game.Player.Health -= totalDmg;

            if (game.Player.Health <= 0)
            {
                game.HandlePlayerDeath();
            }
        }
    }    
}
