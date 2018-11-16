using System;



namespace Capstonia.Items.ArmorTier
{
    public class LeatherChest: Armor
    {
        public LeatherChest(GameManager game): base(game)
        {
            ArmorType = "Leather Armor";
            ArmorTier = 0;
            Defense = getArmorValue();
            Sprite = game.armor_leather_chest;
        }
    }
}
