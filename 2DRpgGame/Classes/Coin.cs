using _2DRpgGame.Classes;
using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Properties;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace _2DRpgGame.Classes
{
    internal class Coin : IDisposable
    {
        internal PointF Location { get; set; }

        internal int AnimationSpeed;
        internal Bitmap[] Animation;
        internal Bitmap Sprite;
        internal RectangleF Hitbox;

        float width;
        float height;

        bool collected = false;

        internal Coin(PointF _location)
        {
            GameDraw.AddPaintEvent(CoinPaint, GameDraw.Layers.Entities);

            Location = _location;

            Animation = new Bitmap[Resources.CoinFlip.GetFrameCount(FrameDimension.Time)];

            Bitmap coin = Resources.CoinFlip;
            for (int i = 0; i < Animation.Length; i++)
            {
                coin.SelectActiveFrame(FrameDimension.Time, i);
                coin.SetResolution(32, 32);
                Animation[i] = (Bitmap)coin.Clone();
            }
            AnimationSpeed = GameUpdate.FrameLimit / Animation.Length;

            Sprite = Animation[0];

            width = Sprite.Width * Sprite.PhysicalDimension.Width;
            height = Sprite.Height * Sprite.PhysicalDimension.Height;

            Hitbox = new RectangleF(new PointF(Location.X - width / 2, Location.Y - height / 2), new SizeF(width / 3, width / 3));

            coinAnim = GameUpdate.FrameCounter % AnimationSpeed;
        }

        int coinAnim = 0;
        private void CoinPaint(object sender, PaintEventArgs e)
        {
            if (collected) return;

            if (GameUpdate.FrameCounter >= GameUpdate.FrameLimit)
                GameUpdate.FrameCounter = 0;

            Sprite = Animation[coinAnim];

            if (GameUpdate.FrameCounter % AnimationSpeed == 0)
            {
                coinAnim++;
                if (coinAnim >= Animation.Length)
                    coinAnim = 0;
            }
#if (DEBUG)
            using (SolidBrush b = new SolidBrush(Color.LightGray)) 
            {
                e.Graphics.FillRectangle(b, Hitbox);
            }
#endif
            e.Graphics.DrawImage(Sprite, new PointF(Location.X + -Player.ScrollX + width / 8, Location.Y + -Player.ScrollY + width / 8));

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(Hitbox);


                if (Collision.IsColliding(Entity.Player.Hitbox, path))
                {
                    collected = true;
                    SetLocation(new Point(-1000, -1000));

                    Player.AddGold(1);

                    this.Dispose();
                }
            }
        }
        private void SetLocation(PointF _position)
        {
            Location = _position;
            Hitbox.Location = new Point((int)(Location.X + -Player.ScrollX), (int)(Location.Y + -Player.ScrollY));
        }

        internal void Update()
        {
            if (collected)
                return;
            SetLocation(Location);
        }

        public void Dispose()
        {
            Sprite = null;
            Animation = null;

            GameUpdate.Coins.Remove(this);
            GameDraw.RemovePaintEvent(CoinPaint);
        }
    }
}
