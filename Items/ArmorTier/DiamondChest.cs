using System;



namespace Capstonia.Items.ArmorTier
{
    public class DiamondChest : Armor
    {
        public DiamondChest(GameManager game) : base(game)
        {
            ArmorType = "Diamond Armor";
            Defense = Defense = Capstonia.GameManager.Random.Next(9, 13);
            Sprite = game.armor_diamond_chest;
        }
    }
}