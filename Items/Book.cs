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
            Strength
        }

        //Consitution Advancement//
        private DeweyDecimal genre;
        public DeweyDecimal Genre { get { return genre; } set { genre = value; } }

        RogueSharp.Random.DotNetRandom Die = new RogueSharp.Random.DotNetRandom();

        public Book(GameManager game): base(game)
        {
            Name = "Book";
            Damage = 0;
            Defense = 0;
            Value = 10;
            History = "Read something for once.";
            Interactive = true;
            Consumable = true;
            Genre = BookPick();
            MaxStack = 1;
            Sprite = game.book;

        }

        //Randomly choose a book attribute//
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
                    //IFFFFFFY//
                    int y = game.Player.Constitution / 10;
                    game.Player.MaxHealth += y;
                    break;
                case DeweyDecimal.Dexterity:
                    game.Player.Dexterity += Value;
                    break;
                case DeweyDecimal.Strength:
                    game.Player.Strength += Value;
                    break;
            }
        }

        public override void RemoveStat()
        {
            //game.Messages.AddMessage("Cannot unread a book you nitwit.");
        }

        public override void Broadcast()
        {
            game.Messages.AddMessage("Look Book!");
        }

        // UseItem()
        // DESC:    Overrides parent class function and uses the item
        // PARAMS:  None.
        // RETURNS: Bool. True if item is used, False otherwise.
        public override bool UseItem()
        {
            //If item is picked up
            AddStat();
            game.Messages.AddMessage("Something something book side");

            //TODO - RETURN FALSE JUST THERE FOR COMPILATION REASONS, WILL UPDATE
            return false;
        }
    }
}
