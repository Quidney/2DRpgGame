using System;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal class MeleeWeapon : Weapon, ICloneable
    {
        internal AttackTypes AttackType { get; set; }
        internal float AttackDuration;
        internal MeleeWeapon(string _name, string _tooltip, float _value, Bitmap _sprite, float _baseDamage, bool _chargedAttack, float _attackDuration, float _cooldown, float _weight, AttackTypes _attackType) : base(_name, _tooltip, _value, _sprite, _baseDamage, _cooldown, _weight, _chargedAttack)
        {
            AttackType = _attackType;
            AttackDuration = _attackDuration;
        }

        internal enum AttackTypes
        {
            Swing,
            Impale,
        }

        public new object Clone()
        {
            return new MeleeWeapon(Name, Tooltip, Value, Sprite, BaseDamage, ChargedAttack, AttackDuration, Cooldown, Weight, AttackType);
        }
    }
}
