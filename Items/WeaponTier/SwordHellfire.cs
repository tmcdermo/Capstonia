using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstonia.Items.WeaponTier
{
    public class SwordHellfire:Weapon
    {
        public SwordHellfire(GameManager game): base(game)
        {
            Sprite = game.weapon_sword_hellfire;
            weaponType = "Hellfire";
            Strength = StrengthGet(1, 4);
        }
    }
}
