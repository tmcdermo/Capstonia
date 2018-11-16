using System;



namespace Capstonia.Items.ArmorTier
{
    public class BloodChest : Armor
    {
        public BloodChest(GameManager game) : base(game)
        {
            ArmorType = "Blood Armor";
            ArmorTier = 5;
            Defense = Defense = Capstonia.GameManager.Random.Next(9, 13);
            Sprite = game.armor_diamond_chest;
        }
    }
}