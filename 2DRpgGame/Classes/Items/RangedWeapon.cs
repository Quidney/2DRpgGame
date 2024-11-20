using System;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal class RangedWeapon : Weapon, ICloneable
    {
        internal AttackTypes AttackType { get; set; }
        internal float ProjectileRange;
        internal Projectile Projectile;
        internal RangedWeapon(string _name, string _tooltip, float _value, Bitmap _sprite, float _baseDamage, bool _chargedAttack, float _projectileRange, float _cooldown, float _weight, Projectile _projectile, AttackTypes _attackType) : base(_name, _tooltip, _value, _sprite, _baseDamage, _cooldown, _weight, _chargedAttack)
        {
            AttackType = _attackType;
            ProjectileRange = _projectileRange;
            Projectile = _projectile;
        }

        internal enum AttackTypes
        {
            None = 0,
            Single = 1,
            Double = 2,
            Triple = 3,
            Quadruple = 4,
            Penta = 5
        }

        public new object Clone()
        {
            return new RangedWeapon(Name, Tooltip, Value, Sprite, BaseDamage, ChargedAttack, ProjectileRange, Cooldown, Weight, Projectile, AttackType);
        }
    }
}
