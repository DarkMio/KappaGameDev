using UnityEngine;
using System;


//15.05.2016, Fabian Wendland
namespace Item
{
    [Serializable]

    public class ItemStats
    {

        public int MinDamage;
        public int MaxDamage;
        public float AttackSpeed;
        public int Armor;

        public static ItemStats operator+(ItemStats lhs, ItemStats rhs)
        {
            return new ItemStats
            {
                MinDamage = lhs.MinDamage + rhs.MinDamage,
                MaxDamage = lhs.MaxDamage + rhs.MaxDamage,
                AttackSpeed = lhs.AttackSpeed + rhs.AttackSpeed,
                Armor = lhs.Armor + rhs.Armor,
            };
        }
        public static ItemStats Interpolate(ItemStats a, ItemStats b, float f)
        {
            f = Mathf.Clamp01(f);
            return new ItemStats
            {
                MinDamage = (int)Mathf.Lerp(a.MinDamage, b.MinDamage, f),
                MaxDamage = (int)Mathf.Lerp(a.MaxDamage, b.MaxDamage, f),
                AttackSpeed = Mathf.Lerp(a.AttackSpeed, b.AttackSpeed, f),
                Armor = (int)Mathf.Lerp(a.Armor, b.Armor, f),
            };
        }
    }
}