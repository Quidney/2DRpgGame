using System;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal class Material : Item, ICloneable
    {
        internal Material(string _name, string _tooltip, float _value, Bitmap _sprite) : base(_name, _tooltip, _value, _sprite)
        {

        }

        public object Clone()
        {
            return new Material(Name, Tooltip, Value, Sprite);
        }
    }
}
