using _2DRpgGame.Classes;
using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Classes.Items;
using _2DRpgGame.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRpgGame
{
    public partial class Form1 : Form
    {
        private static string ResourcesFolder = "Resources";
        private static string CursorIcon = Path.Combine(ResourcesFolder, "Cursor.ico");

        private Bitmap background;

        public Form1()
        {
            InitializeComponent();

            SaveNecessaryFiles().Wait();

            ClientSize = new Size(1024, 1024);
            StartPosition = FormStartPosition.CenterScreen;

            Cursor.Position = PointToScreen(new Point(ClientSize.Width / 2, ClientSize.Height / 2));
            Cursor = new Cursor(CursorIcon);

#if (DEBUG)
            Text += Resources.Debug;
#endif

            Load += LoadNecessities;
            Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            btnStart_Click(btnStart, EventArgs.Empty);
        }

        private void LoadNecessities(object sender, EventArgs e)
        {
            GenerateBackground.GenerateTilesFromFile();
            background = GenerateBackground.GenerateMap();

            GameLoad.LoadControllers(this);
            GameDraw.AddPaintEvent(BackgroundPaint, GameDraw.Layers.Background);
        }

        private static Task SaveNecessaryFiles()
        {
            if (!Directory.Exists(ResourcesFolder))
            {
                Directory.CreateDirectory(ResourcesFolder);
            }

            using (FileStream stream = new FileStream(CursorIcon, FileMode.Create))
            {
                Resources.Cursor.Save(stream);
            }

            return Task.CompletedTask;
        }

        private void BackgroundPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingMode = CompositingMode.SourceCopy;

            Player.ScrollX = Mathf.Clamp(Player.ScrollX, -22, (background.PhysicalDimension.Width * background.HorizontalResolution) + ClientSize.Width - 106);
            Player.ScrollY = Mathf.Clamp(Player.ScrollY, -60, (background.PhysicalDimension.Height * background.VerticalResolution) + ClientSize.Height - 102);
            g.DrawImage(background, -Player.ScrollX - GenerateBackground.tilePixels * GenerateBackground.tileResolution, -Player.ScrollY - GenerateBackground.tilePixels * GenerateBackground.tileResolution);

            g.CompositingMode = CompositingMode.SourceOver;
        }

        Player player;
        private void btnStart_Click(object sender, EventArgs e)
        {
            player = new Player("Quidney");

            for (int i = 0; i < 9; i++)
            {
                Weapon randomWeapon = ItemDatabase.GetRandomWeapon();
                Inventory.AddItem(randomWeapon);
            }
            for (int i = 0; i < 9; i++)
            {
                Armor randomArmor = ItemDatabase.GetRandomArmor();
                Inventory.AddItem(randomArmor);
            }

            GameUpdate.Entities.Add(player);

            for (int i = 0; i < 0; i++)
            {
                Enemy enemy = new Enemy("Zombie", 100f, new PointF(GameUpdate.Random.Next(200, 2000), GameUpdate.Random.Next(200, 2000)), 2f);
                GameUpdate.Entities.Add(enemy);
            }

            GameUpdate.StartTimer();

            pnlMainMenu.Dispose();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
