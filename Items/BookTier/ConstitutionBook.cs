using System;
namespace Capstonia.Items.BookTier
{
    public class ConstitutionBook:Book
    {
        public ConstitutionBook(GameManager game) : base(game)
        {
            Genre = DeweyDecimal.Constitution;
            Value = GetAttributeValue();
            Sprite = game.BookCst;
        }
    }
}
