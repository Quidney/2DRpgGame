using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Classes.Items;
using _2DRpgGame.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace _2DRpgGame.Classes
{
    internal class Player : Entity
    {
        static RectangleF _hitBox = new RectangleF(new PointF(), new SizeF(60, 78));

        internal static new PointF Location;

        internal static List<Keys> InputKeys = new List<Keys>();

        internal Bitmap[] DashAnimation;
        internal int DashAnimationSpeed;

        PointF mousePos;
        internal static float MouseRotation;

        internal static float ScrollX;
        internal static float ScrollY;

        internal static float MoveSpeed = 8f;

        bool spriteFlipped = false;

        bool isAttackHeld;
        bool isChargeHeld;

        bool attackCooldown = false;
        bool isAttacking = false;

        int attackDurationLeft;
        int attackCDLeft;

        const int T_DashCooldown = 25;
        const int T_DashDuration = 15;
        bool dashOnCooldown;
        int dashDurationLeft;
        int dashCooldownLeft;

        int horizontalDirection;
        int verticalDirection;

        const int T_InvincibilityFrames = 20;
        int invincibilityFramesLeft;
        bool isInvincible;

        #region Stats

        internal static float Damage = 5f;

        #endregion

        internal Player(string _name) : base(_name, 100f, _hitBox, new PointF(-(Resources.PlayerIdle.Width * Resources.PlayerIdle.PhysicalDimension.Width / 4.9f), -(Resources.PlayerIdle.Height * Resources.PlayerIdle.PhysicalDimension.Height / 4.95f)), Resources.PlayerIdle, Resources.PlayerWalk, Resources.PlayerDeath, Mathf.GetCenterOfRectangle(GameUpdate.GameWindow.ClientRectangle), true)
        {
            GameUpdate.GameWindow.KeyDown += InputDown;
            GameUpdate.GameWindow.KeyUp += InputUp;

            GameUpdate.GameWindow.MouseMove += Player_MouseMove;
            GameUpdate.GameWindow.MouseDown += Player_MouseDown;
            GameUpdate.GameWindow.MouseUp += Player_MouseUp;

            GameUpdate.GameWindow.MouseWheel += Player_MouseWheel;

            GameDraw.AddPaintEvent(Player_Paint, GameDraw.Layers.Entities);

            Bitmap _dashSprite = Resources.PlayerDash;
            DashAnimation = new Bitmap[_dashSprite.GetFrameCount(FrameDimension.Time)];
            for (int i = 0; i < DashAnimation.Length; i++)
            {
                _dashSprite.SelectActiveFrame(FrameDimension.Time, i);
                _dashSprite.SetResolution(16, 16);
                DashAnimation[i] = (Bitmap)_dashSprite.Clone();
            }
            DashAnimationSpeed = GameUpdate.FrameLimit / DashAnimation.Length / 4;

            Animations = new Bitmap[][] { IdleAnimation, WalkingAnimation, DashAnimation };

            Location = ((Entity)this).Location;
            ScrollX = Location.X;
            ScrollY = Location.Y;
        }

        private void Player_MouseWheel(object sender, MouseEventArgs e)
        {
            ChangeHotbar((int)(-e.Delta * 0.01f), false);
        }

        private static void ChangeHotbar(int _index, bool _set)
        {
            if (_set)
            {
                Inventory.SetSelectedHotbar(_index);
            }
            else
            {
                Inventory.ChangeSelectedHotbar(_index);
            }
        }

        internal void Update()
        {
            healthbar.Update(new Point((int)(Location.X), (int)(Location.Y)), HP);

            Location = ((Entity)this).Location;

            float x = mousePos.X - Location.X;
            float y = Location.Y - mousePos.Y;

            MouseRotation = (90 / (Math.Abs(x) + Math.Abs(y))) * y;
            MouseRotation = x < 0 ? -MouseRotation : MouseRotation;
            MouseRotation += x < 0 ? 180 : 0;
            if (MouseRotation < 0)
                MouseRotation += 360;
            if (MouseRotation >= 360)
                MouseRotation -= 360;


            if (isAttacking)
            {
                attackDurationLeft--;

                if (attackDurationLeft <= 0)
                {
                    isAttacking = false;
                    DrawWeapon.EndAttacking();
                }
            }
            if (attackCooldown)
            {
                attackCDLeft--;

                if (attackCDLeft <= 0)
                    attackCooldown = false;
            }


            if (Inventory.HeldItem is Weapon)
            {
                if (isAttackHeld)
                {
                    Attack();
                }
                if (isChargeHeld)
                {

                }
            }



            ProcessKeys();


            if (dashOnCooldown)
            {
                dashCooldownLeft--;

                if (dashCooldownLeft <= 0)
                    dashOnCooldown = false;
            }
            if (isDashing)
            {
                ScrollY += verticalDirection * MoveSpeed * 1.2f;
                ScrollX += horizontalDirection * MoveSpeed * 1.2f;

                dashDurationLeft--;

                Debug.WriteLine("Dashing!");

                if (dashDurationLeft <= 0)
                    isDashing = false;
            }
            if (isInvincible)
            {
                invincibilityFramesLeft--;

                if (invincibilityFramesLeft <= 0)
                    isInvincible = false;
            }

            verticalDirection = 0;
            horizontalDirection = 0;
        }

        private void Player_MouseDown(object sender, MouseEventArgs e)
        {
            if (!GUI.InventoryOpen && !GUI.Paused)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        isAttackHeld = true;
                        break;
                    case MouseButtons.Right:
                        isChargeHeld = true;
                        break;
                }
            }
        }
        private void Player_MouseUp(object sender, MouseEventArgs e)
        {
            if (!GUI.InventoryOpen && !GUI.Paused)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        isAttackHeld = false;
                        break;
                    case MouseButtons.Right:
                        isChargeHeld = false;
                        break;
                }
            }
        }

        private void Player_MouseMove(object sender, MouseEventArgs e)
        {
            if (!GUI.InventoryOpen)
            {
                if (e.Location.X < GameUpdate.GameWindow.ClientSize.Width / 2 && !spriteFlipped)
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

                if (e.Location.X > GameUpdate.GameWindow.ClientSize.Width / 2 && spriteFlipped)
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

                mousePos = e.Location;
            }

            
            //Debug.WriteLine(mousePos);
        }

        int walkAnim = 0;
        int idleAnim = 0;
        int dashAnim = 0;

        private void Player_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Font f = new Font("Poor Richard", 22f, FontStyle.Bold))
            {
                using (SolidBrush b = new SolidBrush(Color.DarkGreen))
                {
                    g.DrawString(Name + " | " + Level, f, b, new PointF(healthbar.HealthbarGFX.X, healthbar.HealthbarGFX.Y - 40));
                }
            }            
            g.DrawRectangle(Pens.Black, new Rectangle((int)healthbar.HealthbarGFX.X, (int)healthbar.HealthbarGFX.Y, (int)healthbar.HealthbarGFX.Width, (int)healthbar.HealthbarGFX.Height));
            using (SolidBrush b = new SolidBrush(healthbar.HealthbarColor))
            {
                g.FillRectangle(b, healthbar.HealthbarCurrentHealthGFX);
            }


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
            else if (isWalking && !isDashing)
            {
                Sprite = WalkingAnimation[walkAnim];

                if (GameUpdate.FrameCounter % WalkingAnimationSpeed == 0)
                {
                    walkAnim++;
                    if (walkAnim >= WalkingAnimation.Length)
                        walkAnim = 0;
                }
            }
            else if (isDashing)
            {
                Sprite = DashAnimation[dashAnim];

                if (GameUpdate.FrameCounter % DashAnimationSpeed == 0)
                {
                    dashAnim++;
                    if (dashAnim >= DashAnimation.Length)
                        dashAnim = 0;
                }
            }

            g.DrawImage(Sprite, new Point((int)(Location.X - (Sprite.Width * Sprite.PhysicalDimension.Width / 4)), (int)(Location.Y - (Sprite.Height * Sprite.PhysicalDimension.Height / 4))));

#if (DEBUG)
            g.DrawRectangle(Pens.Blue, Hitbox.X, Hitbox.Y, Hitbox.Width, Hitbox.Height);
            g.FillPie(new SolidBrush(Color.Blue), new Rectangle(new Point((int)Location.X - 5, (int)Location.Y - 5), new Size(10, 10)), 0, 360);
#endif
        }



        private void ProcessKeys()
        {
            isIdle = true;
            isWalking = false;

            if (!GUI.InventoryOpen)
            {
                foreach (Keys key in InputKeys)
                {
                    switch (key)
                    {
                        case Keys.W:
                            ScrollY -= MoveSpeed;
                            isIdle = false;
                            isWalking = true;
                            verticalDirection = -1;
                            break;
                        case Keys.A:
                            ScrollX -= MoveSpeed;
                            isIdle = false;
                            isWalking = true;
                            horizontalDirection = -1;
                            break;
                        case Keys.S:
                            ScrollY += MoveSpeed;
                            isIdle = false;
                            isWalking = true;
                            verticalDirection = 1;
                            break;
                        case Keys.D:
                            ScrollX += MoveSpeed;
                            isIdle = false;
                            isWalking = true;
                            horizontalDirection = 1;
                            break;
                        case Keys.Space:
                            if (!dashOnCooldown)
                                Dash();
                            break;
                    }
                }
            }
        }

        private void Dash()
        {
            dashAnim = 0;
            dashDurationLeft = T_DashDuration;
            dashOnCooldown = true;
            dashCooldownLeft = T_DashCooldown;
            isDashing = true;
        }

        private void MenuOpened()
        {
            InputKeys.Clear();
            isAttackHeld = false;
            isChargeHeld = false;
        }

        private void InputDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.A:
                case Keys.S:
                case Keys.D:
                case Keys.Space:
                    if (!InputKeys.Contains(e.KeyCode))
                    {
                        InputKeys.Add(e.KeyCode);
                    }
                    break;
                case Keys.E:
                    GUI.ToggleInventory();
                    MenuOpened();
                    break;
                case Keys.Escape:
                    GUI.TogglePause();
                    MenuOpened();
                    break;
                case Keys.R:
                    Application.Restart();
                    break;
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    ChangeHotbar(e.KeyValue - 49, true);
                    break;
            }
        }
        private void InputUp(object sender, KeyEventArgs e)
        {
            if (InputKeys.Contains(e.KeyCode))
            {
                InputKeys.Remove(e.KeyCode);
            }
        }

        const int T_Cooldown = 40;
        const int T_Duration = 30;
        private void Attack()
        {
            if (!attackCooldown && (!isAttacking || Inventory.HeldItem is RangedWeapon))
            {
                isAttacking = true;
                attackCooldown = true;
                DrawWeapon.StartAttacking();

                if (Inventory.HeldItem is MeleeWeapon mWeapon)
                {
                    attackCDLeft = (int)mWeapon.Cooldown;
                    attackDurationLeft = (int)mWeapon.AttackDuration;
                }
                else if (Inventory.HeldItem is RangedWeapon rWeapon)
                {
                    attackCDLeft = (int)rWeapon.Cooldown;
                    //attackDurationLeft = (int)rWeapon.ProjectileRange;
                }
                else
                {
                    attackCDLeft = T_Cooldown;
                    attackDurationLeft = T_Duration;
                }
            }
        }


        
        internal static int Level = 1;
        internal static float XP = 0f;
        internal static void GainXP(float _xp)
        {
            XP += _xp;

            float _requiredXP = (float)(Math.Round((Math.Sqrt(Level) * 60) * (Math.Floor(Level / 10d) + 1)) + 100);

            while (XP >= _requiredXP)
            {
                XP -= _requiredXP;
                Level++;

                _requiredXP = (float)(Math.Round((Math.Sqrt(Level) * 60) * (Math.Floor(Level / 10d) + 1)) + 100);
            }
        }

        internal static int Gold;
        internal static void AddGold(int _gold)
        {
            Gold += _gold;
        }

        internal new void TakeDamage(float _value)
        {
            if (!isInvincible && !isDashing)
            {
                HP -= _value;

                if (HP <= 0)
                {
                    dying = true;
                }

                isInvincible = true;
                invincibilityFramesLeft = T_InvincibilityFrames;
            }
        }
    }
}
