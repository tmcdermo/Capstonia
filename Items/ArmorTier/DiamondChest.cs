using System;



namespace Capstonia.Items.ArmorTier
{
    public class DiamondChest : Armor
    {
        public DiamondChest(GameManager game) : base(game)
        {
            ArmorType = "Diamond Armor";
            ArmorTier = 2;
            Defense = getArmorValue();
            Sprite = game.armor_diamond_chest;
        }
    }
}