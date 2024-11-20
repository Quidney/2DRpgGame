using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Properties;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _2DRpgGame.Classes
{
    internal class Enemy : Entity
    {
        static RectangleF _hitbox = new RectangleF(new PointF(), new SizeF(60, 78));

        float MoveSpeed;

        internal Enemy(string _name, float _maxHp, PointF _location, float _moveSpeed) : base(_name, _maxHp, _hitbox, new PointF(5, 10), Resources.PlayerIdle, Resources.PlayerWalk, Resources.PlayerDeath, _location, false)
        {
            GameDraw.AddPaintEvent(Enemy_Paint, GameDraw.Layers.Entities);
            MoveSpeed = _moveSpeed;
        }

        int walkAnim = 0;
        int idleAnim = 0;
        int deathAnim = 0;
        private void Enemy_Paint(object sender, PaintEventArgs e)
        {
            if (isDead) return;

            using (Font f = new Font("Poor Richard", 22f))
            {
                using (SolidBrush b = new SolidBrush(Color.Black))
                {
                    e.Graphics.DrawString(Name, f, b, new PointF(healthbar.HealthbarGFX.X, healthbar.HealthbarGFX.Y - 40));
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)healthbar.HealthbarGFX.X, (int)healthbar.HealthbarGFX.Y, (int)healthbar.HealthbarGFX.Width, (int)healthbar.HealthbarGFX.Height));
                }
                using (SolidBrush b = new SolidBrush(healthbar.HealthbarColor))
                {
                    e.Graphics.FillRectangle(b, healthbar.HealthbarCurrentHealthGFX);
                }
            }

            if (dying)
            {
                Sprite = DeathAnimation[deathAnim];

                if (GameUpdate.FrameCounter % DeathAnimationSpeed == 0)
                {
                    deathAnim++;
                    if (deathAnim >= DeathAnimation.Length)
                    {
                        deathAnim--;
                        Die();
                    }
                }

            }
            else
            {
                if (isIdle)
                {
                    Sprite = IdleAnimation[idleAnim];

                    if (GameUpdate.FrameCounter % IdleAnimationSpeed == 0)
                    {
                        idleAnim++;
                        if (idleAnim >= IdleAnimation.Length)
                            idleAnim = 0;
                    }
                }
                else
                {
                    Sprite = WalkingAnimation[walkAnim];


                    if (GameUpdate.FrameCounter % WalkingAnimationSpeed == 0)
                    {
                        walkAnim++;
                        if (walkAnim >= WalkingAnimation.Length)
                            walkAnim = 0;
                    }
                }
            }


            e.Graphics.DrawImage(Sprite, new Point((int)(Location.X + -Player.ScrollX), (int)(Location.Y + -Player.ScrollY)));

            if (!dying && !isDead)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(Hitbox);

                    if (Collision.IsColliding(Player.Hitbox, path))
                    {
                        Player.TakeDamage(15f);
                    }
                }
            }

#if (DEBUG)
            e.Graphics.DrawRectangle(Pens.Blue, Hitbox);
#endif
        }

        internal void Update()
        {
            if (!isDead)
            {
                healthbar.Update(new Point((int)(Location.X + -Player.ScrollX), (int)(Location.Y - Player.ScrollY)), HP);
                SetPosition(Location);

                if (!dying)
                    MoveTowardsPlayer();
            }
        }

        bool spriteFlipped = true;
        private void MoveTowardsPlayer()
        {
            float xOffset = Location.X + -Player.ScrollX - Player.Location.X + ((Player.Sprite.Width * Player.Sprite.PhysicalDimension.Width) / 4);
            float x = (int)Math.Floor(xOffset) > 0 ? MoveSpeed : (int)Math.Floor(xOffset) < 0 ? -MoveSpeed : 0;

            float yOffset = Location.Y + -Player.ScrollY - Player.Location.Y + ((Player.Sprite.Height * Player.Sprite.PhysicalDimension.Height) / 4);
            float y = (int)Math.Floor(yOffset) > 0 ? MoveSpeed : (int)Math.Floor(yOffset) < 0 ? -MoveSpeed : 0;

            SetPosition(new PointF(Location.X - x, Location.Y - y));

            if (x != 0 || y != 0)
            {
                isIdle = false;
                isWalking = true;
            }
            else
            {
                isIdle = true;
                isWalking = false;
            }

            if (x < 0 && !spriteFlipped)
            {
                foreach (Bitmap[] animation in Animations)
                {
                    foreach (Bitmap img in animation)
                    {
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        img.SetResolution(16, 16);
                    }
                }
                spriteFlipped = true;
            }

            if (x > 0 && spriteFlipped)
            {
                foreach (Bitmap[] animation in Animations)
                {
                    foreach (Bitmap img in animation)
                    {
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        img.SetResolution(16, 16);
                    }
                }
                spriteFlipped = false;
            }
        }

        private void Die()
        {
            isDead = true;

            for (int i = 0; i < GameUpdate.Random.Next(1, 6); i++)
            {
                Coin coin = new Coin(new PointF(Location.X + GameUpdate.Random.Next(-35, 35), Location.Y + GameUpdate.Random.Next(-35, 35)));
                GameUpdate.Coins.Add(coin);
            }

            Player.GainXP(GameUpdate.Random.Next(1, 150));

            SetPosition(new Point(-1000, -1000));

            Entity.Enemies.Remove(this);

            GameDraw.RemovePaintEvent(Enemy_Paint);

            this.Dispose();
        }
    }
}
