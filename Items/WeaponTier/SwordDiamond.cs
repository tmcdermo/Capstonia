using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstonia.Items.WeaponTier
{
    public class SwordDiamond : Weapon
    {
        public SwordDiamond(GameManager game): base(game)
        {
            Sprite = game.weapon_sword_diamond;
            weaponType = "Hard Carbon Sword";
            Damage = DamageGet(1, 2);
        }
    }
}
