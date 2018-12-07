using System;
using System.Collections.Generic;
using Capstonia.Core;
using Capstonia.Items;

namespace Capstonia.Systems
{
    public class Equipment
    {
        private GameManager game;
        //Dictionary so only 1 value per item
        public Dictionary<string, Item> Equip;

        public Equipment(GameManager instance)
        {
            game = instance;
            Equip = new Dictionary<string, Item>();
            Equip.Add("Armor", null);
            Equip.Add("Weapon", null);
        }
        // Wear():
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.add?view=netcore-2.1
        // Equips items and transfer any equipped item being replaced to inventory
        public void Wear(Item newItem)
        {
            // place holder to move oldItem back to Inventory//
            Item oldItem;

            oldItem = Strip(newItem.Name);
            Equip[newItem.Name] = newItem ;
            newItem.UseItem();
            TransferBackpack(oldItem);

            //game.Messages.AddMessage("weaponValue: " + game.Player.WeaponValue);    //NEW
            //game.Messages.AddMessage("maxDamage: " + game.Player.MaxDamage);    //NEW
        }

        private Item Strip(string name)
        {
            if(Equip[name] != null)
            {
                Item tmpHolder = Equip[name];
                Equip[name] = null;
                tmpHolder.RemoveItem();
                return tmpHolder;
            }
            else if (name == "Armor")
            {
                Item LeatherJerkin = new Armor(game);
                return LeatherJerkin;
            }
            else
            {
                Item Club = new Weapon(game);
                Club.Damage = 0;
                return Club;
            }
        }

        private void TransferBackpack(Item oldItem)
        {
            if(oldItem != null)
                game.Inventory.AddItem(oldItem);
           
        }

        public void BroadcastGear()
        {
            game.Messages.AddMessage(String.Format("Current Armor: {0} , Defense Value: {1}", ((Armor)Equip["Armor"]).ArmorType, Equip["Armor"].Defense));
            game.Messages.AddMessage(String.Format("Current Weapon: {0} , Attack Value: {1}", (Equip["Weapon"]).Name, Equip["Weapon"].Value));
        }

    }
}
