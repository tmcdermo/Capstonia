using System;



namespace Capstonia.Items.ArmorTier
{
    public class BloodChest : Armor
    {
        public BloodChest(GameManager game) : base(game)
        {
            ArmorType = "Blood Armor";
            ArmorTier = 3;
            Defense = getArmorValue();
            Sprite = game.armor_diamond_chest;
        }
    }
}