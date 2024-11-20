using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _2DRpgGame.Classes.GameControllers
{
    internal static class GameUpdate
    {
        private const int FPS = 60;
        private static Timer UpdateTimer = new Timer() { Interval = 1000 / FPS };

        internal static int FrameLimit = FPS;
        internal static int FrameCounter = 0;

        internal static List<Entity> Entities = new List<Entity>();
        internal static List<Coin> Coins = new List<Coin>();

        internal static Form GameWindow;

        internal static Random Random = new Random();

        internal static void SetupUpdate(Form _gameWindow)
        {
            GameWindow = _gameWindow;
            UpdateTimer.Tick += UpdateTimer_Tick;
        }

        internal static void StartTimer()
        {
            UpdateTimer.Enabled = true;
            UpdateTimer.Start();
        }
        internal static void StopTimer()
        {
            UpdateTimer.Enabled = false;
            UpdateTimer.Stop();
            GameWindow.Invalidate();
        }

        private static void UpdateTimer_Tick(object sender, EventArgs e)
        {
            FrameCounter++;

            if (FrameCounter >= FrameLimit)
                FrameCounter = 0;

            foreach (Entity entity in Entities)
            {
                if (entity is Player player)
                {
                    player.Update();
                }
                else if (entity is Enemy enemy)
                {
                    enemy.Update();
                }
            }

            foreach (Coin coin in Coins)
            {
                coin.Update();
            }

            if (Random.Next(0, 200) == 0)
            {
                int spawnMin = 500;
                int spawnMax = 5700;
                Enemy enemy = new Enemy("Zombie", 100f, new PointF(Random.Next(spawnMin, spawnMax), Random.Next(spawnMin, spawnMax)), 2f);
                Entities.Add(enemy);
            }

            DrawWeapon.Update();

            if (!GameWindow.Focused)
            {
                Player.InputKeys.Clear();
                GUI.SetPause(true);
            }


            GameWindow.Invalidate();

            //Debug.WriteLine("X: " + Player.ScrollX, "Y: " + Player.ScrollY);
            //Debug.WriteLine("Location: " + Player.Location);
        }
    }
}
