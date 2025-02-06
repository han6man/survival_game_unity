using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.GamePlay
{
    public class Weapon
    {
        const int MIN_DAMAGE = 0;
        int damage;
        public int Damage 
        {
            get {
                return damage;
            }
            set {
                damage = (int) Mathf.Max(MIN_DAMAGE, value);
            }
        }
    }
}
