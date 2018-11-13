using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstonia.Items.WeaponTier
{
    public class AxeGold : Weapon
    {
        public  AxeGold(GameManager game): base(game)
        {
            Sprite = game.weapon_axe_gold;
            weaponType = "Gold Axe";
            Strength = StrengthGet(1, 2);
        }
            
    }
}
