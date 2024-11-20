using System;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal class Armor : Equipment, ICloneable
    {
        internal float Defense;
        internal Types Type;

        internal Armor(string _name, string _tooltip, float _value, Bitmap _sprite, Types _type, float _defense) : base(_name, _tooltip, _value, _sprite)
        {
            Defense = _defense;
            Type = _type;
        }

        internal enum Types
        {
            Invalid,
            Helmet,
            Chestplate,
            Leggings,
            Boots
        }

        public object Clone()
        {
            return new Armor(Name, Tooltip, Value, Sprite, Type, Defense);
        }
    }
}
