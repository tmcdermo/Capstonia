using System;



namespace Capstonia.Items.ArmorTier
{
    public class SteelChest : Armor
    {
        public SteelChest(GameManager game) : base(game)
        {
            ArmorType = "Steel Armor";
            ArmorTier = 1;
            Defense = Defense = Capstonia.GameManager.Random.Next(3, 7);
            Sprite = game.armor_steel_chest;
        }
    }
}
