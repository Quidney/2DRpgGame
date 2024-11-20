using System.Drawing;
using System.Drawing.Drawing2D;

namespace _2DRpgGame.Classes
{
    internal static class Collision
    {
        internal static bool IsColliding(Rectangle rect, GraphicsPath path)
        {
            bool TopLeft = path.IsVisible(new Point(rect.X, rect.Y));
            bool TopRight = path.IsVisible(new Point(rect.X + rect.Width, rect.Y));
            bool BotLeft = path.IsVisible(new Point(rect.X, rect.Y + rect.Height));
            bool BotRight = path.IsVisible(new Point(rect.X + rect.Width, rect.Y + rect.Height));

            bool TopMiddle = path.IsVisible(new Point(rect.X + (rect.Width / 2), rect.Y));
            bool LeftMiddle = path.IsVisible(new Point(rect.X, rect.Y + (rect.Height / 2)));
            bool RightMiddle = path.IsVisible(new Point(rect.X + rect.Width, rect.Y + (rect.Height / 2)));
            bool BotMiddle = path.IsVisible(new Point(rect.X + (rect.Width / 2), rect.Y + rect.Height));

            bool Center = path.IsVisible(new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2)));

            bool TopQuarterOne = path.IsVisible(new Point(rect.X + (rect.Width / 4), rect.Y));
            bool TopQuarterThree = path.IsVisible(new Point(rect.X + ((rect.Width / 4) * 3), rect.Y));

            bool LeftQuarterOne = path.IsVisible(new Point(rect.X, rect.Y + (rect.Height / 4)));
            bool LeftQuarterThree = path.IsVisible(new Point(rect.X, rect.Y + ((rect.Height / 4) * 3)));

            bool RightQuarterOne = path.IsVisible(new Point(rect.X + rect.Width, rect.Y + (rect.Height / 4)));
            bool RightQuarterThree = path.IsVisible(new Point(rect.X + rect.Width, rect.Y + ((rect.Height / 4) * 3)));

            bool BotQuarterOne = path.IsVisible(new Point(rect.X + (rect.Width / 4), rect.Y + rect.Height));
            bool BotQuarterThree = path.IsVisible(new Point(rect.X + ((rect.Width / 4) * 3), rect.Y + rect.Height));

            return (TopLeft || TopRight || BotLeft || BotRight || TopMiddle || LeftMiddle || RightMiddle || BotMiddle || Center ||
                    TopQuarterOne || TopQuarterThree || LeftQuarterOne || LeftQuarterThree || RightQuarterOne || RightQuarterThree || BotQuarterOne || BotQuarterThree);
        }

        internal static bool IsColliding(RectangleF rect, GraphicsPath path)
        {
            bool TopLeft = path.IsVisible(new PointF(rect.X, rect.Y));
            bool TopRight = path.IsVisible(new PointF(rect.X + rect.Width, rect.Y));
            bool BotLeft = path.IsVisible(new PointF(rect.X, rect.Y + rect.Height));
            bool BotRight = path.IsVisible(new PointF(rect.X + rect.Width, rect.Y + rect.Height));

            bool TopMiddle = path.IsVisible(new PointF(rect.X + (rect.Width / 2), rect.Y));
            bool LeftMiddle = path.IsVisible(new PointF(rect.X, rect.Y + (rect.Height / 2)));
            bool RightMiddle = path.IsVisible(new PointF(rect.X + rect.Width, rect.Y + (rect.Height / 2)));
            bool BotMiddle = path.IsVisible(new PointF(rect.X + (rect.Width / 2), rect.Y + rect.Height));

            bool Center = path.IsVisible(new PointF(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2)));

            bool TopQuarterOne = path.IsVisible(new PointF(rect.X + (rect.Width / 4), rect.Y));
            bool TopQuarterThree = path.IsVisible(new PointF(rect.X + ((rect.Width / 4) * 3), rect.Y));

            bool LeftQuarterOne = path.IsVisible(new PointF(rect.X, rect.Y + (rect.Height / 4)));
            bool LeftQuarterThree = path.IsVisible(new PointF(rect.X, rect.Y + ((rect.Height / 4) * 3)));

            bool RightQuarterOne = path.IsVisible(new PointF(rect.X + rect.Width, rect.Y + (rect.Height / 4)));
            bool RightQuarterThree = path.IsVisible(new PointF(rect.X + rect.Width, rect.Y + ((rect.Height / 4) * 3)));

            bool BotQuarterOne = path.IsVisible(new PointF(rect.X + (rect.Width / 4), rect.Y + rect.Height));
            bool BotQuarterThree = path.IsVisible(new PointF(rect.X + ((rect.Width / 4) * 3), rect.Y + rect.Height));

            return (TopLeft || TopRight || BotLeft || BotRight || TopMiddle || LeftMiddle || RightMiddle || BotMiddle || Center ||
                    TopQuarterOne || TopQuarterThree || LeftQuarterOne || LeftQuarterThree || RightQuarterOne || RightQuarterThree || BotQuarterOne || BotQuarterThree);
        }
    }
}
