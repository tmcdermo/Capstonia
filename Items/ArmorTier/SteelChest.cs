using System;



namespace Capstonia.Items.ArmorTier
{
    public class SteelChest : Armor
    {
        public SteelChest(GameManager game) : base(game)
        {
            ArmorType = "Steel Armor";
            ArmorTier = 1;
            Defense = getArmorValue();
            Sprite = game.armor_steel_chest;
        }
    }
}
