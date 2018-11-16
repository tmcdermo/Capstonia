using System;
namespace Capstonia.Items.BookTier
{
    public class DexterityBook: Book
    {
        public DexterityBook(GameManager game):base(game)
        {
            Genre = DeweyDecimal.Dexterity;
            Value = GetAttributeValue();
            Sprite = game.BookDex;
        }
    }
}
