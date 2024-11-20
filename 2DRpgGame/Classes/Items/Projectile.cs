using _2DRpgGame.Classes.GameControllers;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal class Projectile : IDisposable
    {
        internal float Speed { get; private set; }
        internal int Accuracy { get; private set; }
        internal Bitmap Sprite { get; private set; }
        internal Rectangle Hitbox { get; private set; }

        internal PointF Location { get; private set; }
        internal float Rotation { get; private set; }

        internal float LifetimeLeft;
        internal List<Enemy> EnemiesHit;

        internal Projectile(float _speed, int _accuracy, Bitmap _sprite, Rectangle _hitbox)
        {
            Speed = _speed;
            Accuracy = _accuracy;
            Sprite = _sprite;
            Hitbox = _hitbox;
        }

        internal Projectile Clone(PointF _location, float _rotation, RangedWeapon _mainWeapon)
        {
            int rarityModifier = (int)_mainWeapon.Rarity;
            int modifiedInaccuracy = rarityModifier * 3;
            int inaccuracy = 115 - (Accuracy + modifiedInaccuracy);
            int randomInaccucuracy = GameUpdate.Random.Next(-inaccuracy, inaccuracy + 1);

            return
                new Projectile(Speed, Accuracy, Sprite, Hitbox)
                {
                    Location = _location,
                    Rotation = _rotation + randomInaccucuracy,
                    LifetimeLeft = _mainWeapon.ProjectileRange,
                    EnemiesHit = new List<Enemy>()
                };
        }

        internal void Move()
        {
            double radians = (Rotation + 45) * Math.PI / 180;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            float deltaX = cos * Speed;
            float deltaY = sin * Speed;
            Location = new PointF(Location.X + deltaX, Location.Y - deltaY);
        }

        public void Dispose()
        {
            Sprite = null;
            EnemiesHit.Clear();
            EnemiesHit = null;
            Hitbox = new Rectangle();
            Speed = 0f;
            Accuracy = 0;
            Location = new PointF();
            Rotation = 0f;
        }
    }
}
