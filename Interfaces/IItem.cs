namespace Capstonia.Interfaces
{
    public interface IItem
    {
        string Name { get; set; }
        int Damage { get; set; }
        int Defense { get; set; }
        int Value { get; set; } //  score contribution 
        string History { get; set; } // descriptor we want to print out possibly
        bool Interactive { get; set; } // usable or not
        bool Consumable { get; set; } // i.e. potions/food
        bool IsEquipped { get; set; } // Sets whether or not armor or weapon is equipped
        int MaxStack { get; set;  }  //max amount that can be stacked
        bool Use();
    }
}
