using System;



namespace Capstonia.Items.ArmorTier
{
    public class EmeraldChest : Armor
    {
        public EmeraldChest(GameManager game) : base(game)
        {
            ArmorType = "Rayquaza Armor";
            ArmorTier = 3;
            Defense = Defense = Capstonia.GameManager.Random.Next(7,11);
            Sprite = game.armor_emerald_chest;
        }
    }
}
