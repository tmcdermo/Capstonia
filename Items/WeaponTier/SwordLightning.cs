using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstonia.Items.WeaponTier
{
    public class SwordLightning : Weapon
    {
        public SwordLightning(GameManager game) : base(game)
        {
            Sprite = game.weapon_sword_lightning;
            weaponType = "Hellfire";
            Damage = DamageGet(1, 4);
        }
    }
}
