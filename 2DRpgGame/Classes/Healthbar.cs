using System.Drawing;

namespace _2DRpgGame.Classes
{
    internal class Healthbar
    {
        internal RectangleF HealthbarGFX;
        internal RectangleF HealthbarCurrentHealthGFX;
        internal Color HealthbarColor;

        private const int healthbarWidth = 175;
        private const int healthbarHeight = 30;

        private float MaxHealth;
        private float Health;
        private Point Location;

        internal Healthbar(float _maxHealth)
        {
            MaxHealth = _maxHealth;
        }
        internal void SetMaxHealth(float _maxHealth)
        {
            MaxHealth = _maxHealth;
        }

        /// <summary>
        /// Update the healthbar per tick
        /// </summary>
        /// <param name="_location"></param>
        /// <param name="_health"></param>
        internal void Update(Point _location, float _health)
        {
            Location = _location;
            Health = _health;

            if (Health <= 15)
                HealthbarColor = Color.DarkRed;
            else
                HealthbarColor = Color.Red;

            HealthbarGFX = new RectangleF(new PointF(Location.X - healthbarWidth / 2, Location.Y - healthbarHeight * 3.2f), new Size(healthbarWidth, healthbarHeight));
            HealthbarCurrentHealthGFX = new RectangleF(new PointF(HealthbarGFX.X + 1, HealthbarGFX.Y + 1), new SizeF(Health / MaxHealth * healthbarWidth - 2, healthbarHeight - 2));
        }
    }
}
