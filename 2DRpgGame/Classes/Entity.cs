using _2DRpgGame.Classes.GameControllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace _2DRpgGame.Classes
{
    internal abstract class Entity : IDisposable
    {
        internal static Player Player;

        internal static List<Enemy> Enemies = new List<Enemy>();

        internal string Name { get; private set; } = "Entity";
        internal float MaxHP { get; private set; }
        internal float HP { get; set; }

        internal Bitmap Sprite;

        internal Bitmap[][] Animations;

        internal Bitmap[] IdleAnimation;
        internal int IdleAnimationSpeed;
        internal Bitmap[] WalkingAnimation;
        internal int WalkingAnimationSpeed;
        internal Bitmap[] DeathAnimation;
        internal int DeathAnimationSpeed;

        internal PointF Location { get; private set; } = new PointF();
        internal float Rotation { get; private set; } = 0f;
        internal Rectangle Hitbox { get { return hitbox; } }
        private Rectangle hitbox;
        private PointF hitboxOffset;

        internal Healthbar healthbar;

        internal bool isIdle = true;
        internal bool isWalking = false;
        internal bool isDashing = false;
        internal bool dying = false;
        internal bool isDead = false;

        internal Entity(string _name, float _maxHP, RectangleF _hitbox, PointF _hitboxOffset, Bitmap _idleSprite, Bitmap _walkingSprite, Bitmap _deathSprite, PointF _location, bool _isPlayer)
        {
            Name = _name;
            MaxHP = _maxHP;
            HP = MaxHP;

            Location = _location;

            hitboxOffset = _hitboxOffset;
            hitbox = new Rectangle(new Point((int)(Location.X + _hitboxOffset.X), (int)(Location.Y + _hitboxOffset.Y)), new Size((int)_hitbox.Width, (int)_hitbox.Height));

            SetPosition(Location);

            IdleAnimation = new Bitmap[_idleSprite.GetFrameCount(FrameDimension.Time)];
            for (int i = 0; i < IdleAnimation.Length; i++)
            {
                _idleSprite.SelectActiveFrame(FrameDimension.Time, i);
                _idleSprite.SetResolution(16, 16);
                IdleAnimation[i] = (Bitmap)_idleSprite.Clone();
            }
            IdleAnimationSpeed = GameUpdate.FrameLimit / IdleAnimation.Length;

            WalkingAnimation = new Bitmap[_walkingSprite.GetFrameCount(FrameDimension.Time)];
            for (int i = 0; i < WalkingAnimation.Length; i++)
            {
                _walkingSprite.SelectActiveFrame(FrameDimension.Time, i);
                _walkingSprite.SetResolution(16, 16);
                WalkingAnimation[i] = (Bitmap)_walkingSprite.Clone();
            }
            WalkingAnimationSpeed = GameUpdate.FrameLimit / WalkingAnimation.Length / 5;

            DeathAnimation = new Bitmap[_deathSprite.GetFrameCount(FrameDimension.Time)];
            for (int i = 0; i < DeathAnimation.Length; i++)
            {
                _deathSprite.SelectActiveFrame(FrameDimension.Time, i);
                _deathSprite.SetResolution(16, 16);
                DeathAnimation[i] = (Bitmap)_deathSprite.Clone();
            }
            DeathAnimationSpeed = GameUpdate.FrameLimit / DeathAnimation.Length;


            Animations = new Bitmap[][] { IdleAnimation, WalkingAnimation };

            healthbar = new Healthbar(MaxHP);

            if (_isPlayer)
                Player = (Player)this;
            else
            {
                Enemies.Add(this as Enemy);
            }
        }

        internal void SetName(string _name)
        {
            Name = _name;
        }
        internal void SetRotation(float _rotation)
        {
            Rotation = _rotation;
        }
        internal void SetPosition(PointF _position)
        {
            Location = _position;

            if (this is Player)
            {
                hitbox.Location = new Point((int)(Location.X + hitboxOffset.X), (int)(Location.Y + hitboxOffset.Y));
            }
            if (this is Enemy)
            {
                hitbox.Location = new Point((int)(Location.X + hitboxOffset.X + -Player.ScrollX), (int)(Location.Y + hitboxOffset.Y + -Player.ScrollY));
            }
        }

        internal void TakeDamage(float _damage)
        {
            HP -= _damage;

            if (HP <= 0)
            {
                dying = true;
            }

            //((Image)Sprite).
        }

        public void Dispose()
        {
            Animations = null;
            IdleAnimation = null;
            WalkingAnimation = null;
            DeathAnimation = null;

            Rotation = 0;

            healthbar = null;
        }
    }
}
