using System;



namespace Capstonia.Items.ArmorTier
{
    public class GoldChest : Armor
    {
        public GoldChest(GameManager game) : base(game)
        {
            ArmorType = "Gold the Poor Repellent Armor";
            Defense = Defense = Capstonia.GameManager.Random.Next(5, 9);
            Sprite = game.armor_gold_chest;
        }
    }
}
