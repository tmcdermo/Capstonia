using System;



namespace Capstonia.Items.ArmorTier
{
    public class GoldChest : Armor
    {
        public GoldChest(GameManager game) : base(game)
        {
            ArmorType = "Gold Armor";
            ArmorTier = 1;
            Defense = getArmorValue();
            Sprite = game.armor_gold_chest;
        }
    }
}
