using System;
namespace Capstonia.Items.BookTier
{
    public class EvilBook:Book
    {
        public EvilBook(GameManager game): base(game)
        {
            Genre = DeweyDecimal.Evil;
            Value = GetAttributeValue();
            Sprite = game.BookBad;
        }
    }
}
