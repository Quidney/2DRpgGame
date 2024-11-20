using System;
using System.Drawing;

namespace _2DRpgGame.Classes
{
    internal static class Mathf
    {
        internal static float Clamp(float value, float min, float max)
        {
            return value > max ? max : value < min ? min : value;
        }
        internal static int Clamp(int value, int min, int max)
        {
            return value > max ? max : value < min ? min : value;
        }

        internal static Point GetCenterOfRectangle(Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
    }
}
