using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstonia.Items.WeaponTier
{
    public class SwordAcid: Weapon
    {
        public SwordAcid(GameManager game): base(game)
        {
            Sprite = game.weapon_sword_acid;
            weaponType = "Toxic Waste";
            Damage = DamageGet(1, 3);
        }
    }
}
