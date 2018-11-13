using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstonia.Items.WeaponTier
{
    public class LanceVorpal: Weapon
    {
        public LanceVorpal(GameManager game) : base(game)
        {
            Sprite = game.weapon_lance_vorpal;
            weaponType = "Archon Lance";
            Strength = StrengthGet(1, 3);

        }
    }
}
