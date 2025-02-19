using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class Weapon
    {
        private const int MIN_DAMAGE = 0;
        private int damage;
        public int Damage
        {
            get { return damage; }
            set { damage = (int)MathF.Max(MIN_DAMAGE, value); }
        }

        public int GetDamage(int modifier)
        {
            return (int)MathF.Max(MIN_DAMAGE, Damage * modifier);
        }

        public int GetDamage(params int[] modifiers)
        {
            int effect = 0;
            foreach (var mod in modifiers)
            {
                effect += mod;
            }
            return (int)MathF.Max(MIN_DAMAGE, Damage * effect);
        }

        private void Test()
        {
            GetDamage(1, 2, 4, 5, 635);
        }


    }
}
