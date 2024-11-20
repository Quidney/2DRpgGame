using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal abstract class Item
    {
        internal string Name { get; private set; }
        internal string Tooltip { get; private set; }
        internal float Value { get; private set; }

        internal Bitmap Sprite;

        internal Item(string _name, string _tooltip, float _value, Bitmap _sprite)
        {
            Name = _name;
            Tooltip = _tooltip;
            Value = _value;
            Sprite = _sprite;
        }

        internal void ChangeName(string _name)
        {
            Name = _name;
        }
        internal void ChangeDescription(string _description)
        {
            Tooltip = _description;
        }
        internal void ChangeValue(float _value)
        {
            Value = _value;
        }
    }
}
