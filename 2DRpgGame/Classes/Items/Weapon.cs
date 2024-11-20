using System;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal abstract class Weapon : Equipment, ICloneable
    {
        internal float BaseDamage { get; private set; }
        internal float Damage { get; private set; }
        internal float Cooldown { get; private set; }
        internal float Weight { get; private set; }
        internal bool ChargedAttack { get; private set; }

        internal Weapon(string _name, string _description, float _value, Bitmap _sprite, float _baseDamage, float _cooldown, float _weight, bool _chargedAttack) : base(_name, _description, _value, _sprite)
        {
            BaseDamage = _baseDamage;
            Cooldown = _cooldown;
            Weight = _weight;
            ChargedAttack = _chargedAttack;

            Damage = BaseDamage * Equipment.RarityDamageModifier[Rarity];
        }

        public object Clone()
        {
            if (this is MeleeWeapon mWeapon)
            {
                return mWeapon.Clone();
            }
            else if (this is RangedWeapon rWeapon)
            {
                return rWeapon.Clone();
            }
            return null;
        }
    }
}
