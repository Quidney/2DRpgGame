using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Classes.Items;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace _2DRpgGame.Classes
{
    internal static class DrawWeapon
    {
        private static Weapon Weapon;

        private static bool impaleReturn;
        private static bool isAttacking;
        private static bool hideWeapon;
        private static float rotationLock;
        private static int animFrame = 0;

        private static Bitmap WeaponSprite;

        static List<Projectile> Projectiles = new List<Projectile>();

        internal static void StartAttacking()
        {
            rotationLock = Player.MouseRotation;

            if (rotationLock < 0)
                rotationLock += 360;
            if (rotationLock >= 360)
                rotationLock -= 360;

            Debug.WriteLine(rotationLock);
            animFrame = 0;
            impaleReturn = false;
            isAttacking = true;
            hideWeapon = true;
            enemiesHit = new List<Enemy>();

            if (Inventory.HeldItem is RangedWeapon rWeapon)
            {
                hideWeapon = false;
                int shotCount = (int)rWeapon.AttackType;
                for (int i = 1; i <= shotCount; i++)
                {
                    float _rotation;
                    if (i != 1)
                        _rotation = (rotationLock + (shotCount % i == 1 ? -i : i * 3)) - 45;
                    else
                        _rotation = (rotationLock - 45);
                    Projectiles.Add(rWeapon.Projectile.Clone(new PointF(Player.Location.X + Player.ScrollX, Player.Location.Y + Player.ScrollY), _rotation, rWeapon));
                }

            }
        }
        internal static void EndAttacking()
        {
            isAttacking = false;
            Debug.WriteLine(animFrame);
        }

        internal static void Draw(Weapon _weapon)
        {
            Weapon = _weapon;

            WeaponSprite = (Bitmap)Weapon.Sprite.Clone();
            if (Weapon is MeleeWeapon)
                WeaponSprite.SetResolution(64, 64);

            GameDraw.RemovePaintEvent(Weapon_Paint);
            GameDraw.AddPaintEvent(Weapon_Paint, GameDraw.Layers.Entities);
        }

        internal static void Update()
        {
            foreach (Projectile projectile in new List<Projectile>(Projectiles))
            {
                projectile.Move();
                projectile.LifetimeLeft--;

                if (projectile.LifetimeLeft <= 0)
                {
                    Projectiles.Remove(projectile);
                    projectile.Dispose();
                }
            }

            animFrame += 3;
        }

        private static void Weapon_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (!isAttacking || !hideWeapon)
            {
                g.TranslateTransform(Player.Location.X, Player.Location.Y);
                float rotation = Player.MouseRotation;
                if (float.IsNaN(rotation))
                    rotation = 0;

                if (Weapon is MeleeWeapon)
                {
                    g.RotateTransform(-rotation - 15);
                    g.TranslateTransform(-Player.Location.X, -Player.Location.Y - 40);
                }
                else if (Weapon is RangedWeapon)
                {
                    g.RotateTransform(-rotation + 45);
                    g.TranslateTransform(-Player.Location.X - 10, -Player.Location.Y - 55);
                }
                g.DrawImage(WeaponSprite, Player.Location);
                g.ResetTransform();

            }
            else
            {
                if (Weapon is MeleeWeapon mWeapon)
                    MeleeAttackAnimations(g, mWeapon);
            }

            DrawProjectiles(g);
        }


        private static void MeleeAttackAnimations(Graphics g, MeleeWeapon _weapon)
        {
            switch (_weapon.AttackType)
            {
                case MeleeWeapon.AttackTypes.Swing:
                    SwingAnimation(g, _weapon);
                    break;
                case MeleeWeapon.AttackTypes.Impale:
                    ImpaleAnimation(g, _weapon);
                    break;
            }
        }

        private static List<Enemy> enemiesHit = new List<Enemy>();


        private static void SwingAnimation(Graphics g, MeleeWeapon _weapon)
        {
            g.TranslateTransform(Player.Location.X, Player.Location.Y);
            g.RotateTransform(-rotationLock - 22.5f + animFrame * (40f / _weapon.AttackDuration));
            g.DrawImage(WeaponSprite, new PointF(0, -65));
            g.ResetTransform();

            using (GraphicsPath path = new GraphicsPath())
            {
                using (Matrix matrix = new Matrix())
                {
                    matrix.Translate(Player.Location.X, Player.Location.Y);
                    matrix.Rotate(-rotationLock - 22.5f + 45f + animFrame * (40f / _weapon.AttackDuration));
                    path.AddRectangle(new RectangleF(new PointF(Player.Location.X, Player.Location.Y - 100), new Size(50, 100)));
                    matrix.Translate(-(Player.Location.X), -(Player.Location.Y));
                    path.Transform(matrix);
                }

#if (DEBUG)
                g.DrawPath(Pens.Black, path);
#endif

                foreach (Enemy enemy in Entity.Enemies)
                {
                    if (!enemiesHit.Contains(enemy) && !enemy.dying && !enemy.isDead)
                    {
                        if (Collision.IsColliding(enemy.Hitbox, path))
                        {
                            float extraDamage = 0;
                            if (Inventory.HeldItem is Weapon weapon)
                                extraDamage = weapon.Damage;
                            enemy.TakeDamage(Player.Damage + extraDamage);
                            enemiesHit.Add(enemy);
                        }
                    }
                }
            }
        }

        private static void ImpaleAnimation(Graphics g, MeleeWeapon _weapon)
        {
            int weaponAnim = animFrame;
            if (weaponAnim > 45)
            {
                if (!impaleReturn)
                {
                    impaleReturn = true;
                    foreach (Enemy enemy in enemiesHit)
                    {
                        float extraDamage = 0;
                            extraDamage = _weapon.Damage;
                        enemy.TakeDamage(Player.Damage + extraDamage);
                    }
                }
                weaponAnim -= ((weaponAnim - 45) / 2) * 4;
            }

            g.TranslateTransform(Player.Location.X, Player.Location.Y);
            g.RotateTransform(-rotationLock + 45);
            g.TranslateTransform(2f * weaponAnim, -(2f * weaponAnim));
            g.DrawImage(WeaponSprite, new PointF(0, -65));
            g.ResetTransform();

            using (GraphicsPath path = new GraphicsPath())
            {
                using (Matrix matrix = new Matrix())
                {
                    matrix.Translate(Player.Location.X, Player.Location.Y);
                    matrix.Rotate(-rotationLock + 90);
                    path.AddRectangle(new RectangleF(new PointF(Player.Location.X - 4, Player.Location.Y - 105), new Size(50, 120)));
                    matrix.Translate(-(Player.Location.X), -(Player.Location.Y + (2.8f * weaponAnim)));
                    path.Transform(matrix);
                }

#if (DEBUG)
                g.DrawPath(Pens.Black, path);
#endif

                foreach (Enemy enemy in Entity.Enemies)
                {
                    if (!enemiesHit.Contains(enemy) && !enemy.dying && !enemy.isDead)
                    {
                        if (Collision.IsColliding(enemy.Hitbox, path))
                        {
                            float extraDamage = 0;
                            if (Inventory.HeldItem is Weapon weapon)
                                extraDamage = weapon.Damage;
                            enemy.TakeDamage(Player.Damage + extraDamage);
                            enemiesHit.Add(enemy);
                        }
                    }
                }
            }
        }


        private static void DrawProjectiles(Graphics g)
        {
            foreach (Projectile projectile in Projectiles)
            {
                g.TranslateTransform(projectile.Location.X - Player.ScrollX, projectile.Location.Y - Player.ScrollY);
                g.RotateTransform(-projectile.Rotation);
                g.DrawImage(projectile.Sprite, new PointF(0, -65));
                g.ResetTransform();

                using (GraphicsPath path = new GraphicsPath())
                {
                    using (Matrix matrix = new Matrix())
                    {
                        matrix.Translate(projectile.Location.X - Player.ScrollX, projectile.Location.Y - Player.ScrollY);
                        matrix.Rotate(-projectile.Rotation + 45);
                        path.AddRectangle(new RectangleF(new PointF(projectile.Location.X - Player.ScrollX - 8, projectile.Location.Y - Player.ScrollY - 60), new Size(15, 25)));
                        matrix.Translate(-(projectile.Location.X - Player.ScrollX), -(projectile.Location.Y - Player.ScrollY));
                        path.Transform(matrix);
                    }

#if (DEBUG)
                    g.DrawPath(Pens.Black, path);
#endif

                    foreach (Enemy enemy in Entity.Enemies)
                    {
                        if (!projectile.EnemiesHit.Contains(enemy) && !enemy.dying && !enemy.isDead)
                        {
                            if (Collision.IsColliding(enemy.Hitbox, path))
                            {
                                float extraDamage = 0;
                                if (Inventory.HeldItem is Weapon weapon)
                                    extraDamage = weapon.Damage;
                                enemy.TakeDamage(Player.Damage + extraDamage);
                                projectile.EnemiesHit.Add(enemy);
                            }
                        }
                    }
                }
            }
        }
    }
}
