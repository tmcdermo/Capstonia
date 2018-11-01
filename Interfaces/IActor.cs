namespace Capstonia.Interfaces
{
    public interface IActor
    {
        int Constitution { get; set; } // every point above 10 gives a health bonus
        int Dexterity { get; set; } // every point above 10 gives a dodge bonus
        int MaxHealth { get; set; } // Max health total for Actor; if the values reaches 0, the actor is killed
        int CurrHealth { get; set; } // Current health total for Actor; if the values reaches 0, the actor is killed
        int MaxDamage { get; set; } // max dmg Actor can cause
        int MinDamage { get; set; } // min dmg Actor can cause
        string Name { get; set; } // name of Actor
        int Strength { get; set; } // every point above 10 gives a dmg bonus 
    }
}