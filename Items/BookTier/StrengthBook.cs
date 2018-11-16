using System;

namespace Capstonia.Items.BookTier
{
    public class StrengthBook:Book
    {
        public StrengthBook(GameManager game):  base(game)
        {
            Genre = DeweyDecimal.Strength;
            Value = GetAttributeValue();
            Sprite = game.bookStr;
        }


       
    }
}
