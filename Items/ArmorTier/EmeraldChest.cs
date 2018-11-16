using System;



namespace Capstonia.Items.ArmorTier
{
    public class EmeraldChest : Armor
    {
        public EmeraldChest(GameManager game) : base(game)
        {
            ArmorType = "Rayquaza Armor";
            ArmorTier = 2;
            Defense = getArmorValue();
            Sprite = game.armor_emerald_chest;
        }
    }
}
