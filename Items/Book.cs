using System;
using System.Collections.Generic;
using Capstonia.Core;
using RogueSharp;

namespace Capstonia.Items
{   
    public class Book:Item
    {
        /// <summary>
        /// Three different types of book that can drop
        ///</summary>
        public enum DeweyDecimal
        {
            Constitution,
            Dexterity,
            Strength,
            Evil,
            None
        }

        //Consitution Advancement//
        private DeweyDecimal genre;
        public DeweyDecimal Genre { get { return genre; } set { genre = value; } }


        public Book(GameManager game): base(game)
        {
            Name = "Book";
            Genre = DeweyDecimal.None;
            Strength = 0;
            Defense = 0;
            Value = 0;
            History = "Read something for once.";
            Interactive = true;
            Consumable = true;
            MaxStack = 1;
            Sprite = game.book;

        }

        // Removed Pick Book function to refactor and expand on more book choices
        /*/Randomly choose a book attribute//
        //https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
        /// <summary>
        /// Enum values stored into an Array, Randomize a value between 0 and Length of array minus 1
        /// </summary>
        /// <returns>Enum DeweyDecimal value</returns>
        private DeweyDecimal BookPick()
        {
            Array Lottery = Enum.GetValues(typeof(DeweyDecimal));
            int x = Die.Next(0, Lottery.Length - 1);
            return (DeweyDecimal)Lottery.GetValue(x);
        }
        */

        //Tentatively Will have the item class itself permnantly buff the Players 
        public override void AddStat()
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum
            // can be accessed via ObjectType.Variable
            DeweyDecimal caseSwitch = Genre;
            switch (caseSwitch)
            {
                case DeweyDecimal.Constitution:
                    game.Player.Constitution += Value;
                    int y = game.Player.Constitution - 10; // every point above 10
                    game.Player.MaxHealth += y;
                    break;
                case DeweyDecimal.Dexterity:
                    game.Player.Dexterity += Value;
                    break;
                case DeweyDecimal.Strength:
                    game.Player.Strength += Value;
                    break;
                case DeweyDecimal.Evil:
                    game.Player.Dexterity -= Value;
                    game.Player.Strength -= Value;
                    game.Player.Constitution -= Value;
                    int z = game.Player.Constitution - 10;
                    game.Player.MaxHealth = 100 + z;
                    if(game.Player.CurrHealth > game.Player.MaxHealth)
                    {
                        game.Player.CurrHealth = game.Player.MaxHealth;
                    }
                    break;
                case DeweyDecimal.None:
                    break;
            }
        }

        public override void RemoveStat()
        {
            //game.Messages.AddMessage("Cannot unread a book you nitwit.");
        }

        public override void Broadcast()
        {
            game.Messages.AddMessage(String.Format("Book of {0} : Knowledge is power.",Genre));
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override void UseItem()
        {
            AddStat();
            if (Genre != DeweyDecimal.Evil)
                game.Messages.AddMessage(String.Format("Gained {0} {2} from the Book of {1}", Value, Genre, Genre));
            else
                game.Messages.AddMessage(String.Format("Lost {0} from all 3 stats.", Value));

        }

        // Function GetStrength() : Scales book to level
        protected int GetAttributeValue()
        {
            int baseGain = 1;
            float levelScale = game.Player.Level / (float)2.0;
            int levelGain = (int)Math.Ceiling(levelScale);
            return Capstonia.GameManager.Random.Next(baseGain, levelGain);
        }
    }
}
