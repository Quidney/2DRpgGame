using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Classes.Items;
using _2DRpgGame.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _2DRpgGame.Classes
{
    internal static class GUI
    {
        internal static bool DrawGUI { get; private set; } = false;
        internal static bool InventoryOpen { get; private set; } = false;
        internal static bool Paused { get; private set; } = false;

        static Form GameWindow;

        static HashSet<(Rectangle, int)> InventoryBoxes = new HashSet<(Rectangle, int)>();

        static bool isClicked = false;
        static bool isCarryingItem = false;
        static Item ItemCarried;
        static int ItemPrevIndex;

        const int InvHorizontal = 9;

        internal static void InitGUI()
        {
            GameWindow = GameUpdate.GameWindow;

            GameDraw.AddPaintEvent(GUI_Paint, GameDraw.Layers.GUI);

            GameWindow.MouseDown += GUI_MouseDown;
        }


        private static void GUI_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isClicked = true;
        }

        private static void GUI_Paint(object sender, PaintEventArgs e)
        {
            if (!DrawGUI)
                return;

            Graphics g = e.Graphics;

            Bitmap coin = Resources.CoinFlip;
            coin.SetResolution(64, 64);
            g.DrawImage(coin, 0, coin.PhysicalDimension.Height / 2);
            using (Font f = new Font("Arial", 22f))
            {
                using (SolidBrush b = new SolidBrush(Color.Black))
                {
                    g.DrawString(Player.Gold.ToString(), f, b, new PointF(Resources.CoinFlip.PhysicalDimension.Width + 8, 4));
                }
            }

            InventoryBoxes.Clear();

            if (InventoryOpen)
            {
                DrawInventory(g);
            }
            else
            {
                DrawHotbar(g);
            }

            if (Paused)
            {
                DrawPauseScreen(g);
            }

            isClicked = false;
        }

        private static void DrawHotbar(Graphics g)
        {
            if (isCarryingItem)
            {
                Inventory.Items[ItemPrevIndex] = ItemCarried;
                ItemCarried = null;
                isCarryingItem = false;
            }

            int InvBoxSize = 70;
            for (int i = 0; i < InvHorizontal; i++)
            {
                using (Pen pen = new Pen(Color.Black, i == Inventory.HotbarIndex ? 8f : 4f))
                {
                    int XPos = (GameWindow.ClientRectangle.Width - (InvHorizontal * InvBoxSize)) / 2 + (InvBoxSize * i);
                    int YPos = (int)(GameWindow.ClientRectangle.Height - InvBoxSize * 1.2f);
                    Point InvBoxPos = new Point(XPos, YPos);
                    Rectangle InvBox = new Rectangle(InvBoxPos, new Size(InvBoxSize, InvBoxSize));
                    g.DrawRectangle(pen, InvBox);

                    Item hotbarItem = Inventory.Items[Inventory.HotbarToInv(i)];

                    if (hotbarItem != null)
                    {
                        if (hotbarItem is Equipment equipment)
                        {
                            Color color = Equipment.RarityColor(equipment.Rarity);
                            using (SolidBrush b = new SolidBrush(Color.FromArgb(180, color)))
                            {
                                g.FillRectangle(b, InvBox);
                            }
                        }

                        Bitmap sprite = hotbarItem.Sprite;
                        sprite.SetResolution(100, 100);
                        g.DrawImage(sprite, new Point(InvBoxPos.X + 3, InvBoxPos.Y + 3));
                    }
                }
            }
        }

        private static void DrawInventory(Graphics g)
        {
            using (SolidBrush b = new SolidBrush(Color.FromArgb(160, Color.Gray)))
            {
                g.FillRectangle(b, GameWindow.ClientRectangle);
            }
            const int InvBoxSize = 90;
            using (Pen pen = new Pen(Color.Black, 4f))
            {
                for (int y = 0; y < Inventory.Items.Length / InvHorizontal; y++)
                {
                    int YPos = ((GameWindow.ClientRectangle.Height - ((Inventory.Items.Length / InvHorizontal) * InvBoxSize)) / 2 + (InvBoxSize * y));

                    for (int x = 0; x < InvHorizontal; x++)
                    {
                        int XPos = (GameWindow.ClientRectangle.Width - (InvHorizontal * InvBoxSize)) / 2 + (InvBoxSize * x);

                        Point InvBoxPos = new Point(XPos, YPos);
                        Rectangle InvBox = new Rectangle(InvBoxPos, new Size(InvBoxSize, InvBoxSize));
                        g.DrawRectangle(pen, InvBox);

                        int index = (y * InvHorizontal) + x;

                        InventoryBoxes.Add((InvBox, index));

                        Item item = Inventory.Items[index];

                        if (item != null)
                        {
                            if (item is Equipment equipment)
                            {
                                Color color = Equipment.RarityColor(equipment.Rarity);

                                using (SolidBrush b = new SolidBrush(Color.FromArgb(180, color)))
                                {
                                    g.FillRectangle(b, InvBox);
                                }
                            }

                            Bitmap sprite = item.Sprite;
                            sprite.SetResolution(75, 75);
                            g.DrawImage(sprite, new Point(InvBoxPos.X + 5, InvBoxPos.Y + 5));
                        }
                    }
                }
            }


            using (GraphicsPath path = new GraphicsPath())
            {
                foreach (var pair in InventoryBoxes)
                {
                    int index = pair.Item2;
                    Item item = Inventory.Items[index];
                    if (item == null && !isCarryingItem)
                        continue;

                    Rectangle rect = pair.Item1;

                    Point mousePos = GameWindow.PointToClient(Cursor.Position);


                    path.Reset();
                    path.AddRectangle(rect);

                    if (Collision.IsColliding(new Rectangle(GameWindow.PointToClient(Cursor.Position), Cursor.Current.Size), path))
                    {

                        if (item != null)
                        {
                            string textToDisplay = string.Empty;
                            if (item is Equipment equipment)
                                textToDisplay += $"{equipment.Rarity} ";

                            textToDisplay += $"{item.Name} \n*{item.Tooltip}\n";

                            if (item is Weapon weapon)
                                textToDisplay += $"Damage: {weapon.Damage}\nSpeed: {Math.Round(60 / weapon.Cooldown, 2)}\nRarity Damage Modifier: {(-1 + Equipment.RarityDamageModifier[weapon.Rarity]) * 100}%";

                            Size boxSize = new Size(textToDisplay.Length * 5, 175);

                            int offSet = (boxSize.Width - (GameWindow.ClientSize.Width - mousePos.X));
                            Point textPos = new Point(mousePos.X - (offSet > 0 ? offSet : 0), mousePos.Y - 200);

                            Debug.Write("MouseX: " + mousePos.X + " ");
                            Debug.Write("Width: " + GameWindow.ClientSize.Width + " ");
                            Debug.WriteLine("BoxWidth: " + boxSize.Width);

                            Rectangle itemInfoRect = new Rectangle(textPos, boxSize);

                            g.DrawRectangle(Pens.Blue, rect);
                            using (SolidBrush b = new SolidBrush(Color.LightBlue))
                            {
                                g.FillRectangle(b, itemInfoRect);
                            }
                            using (SolidBrush b = new SolidBrush(Color.Black))
                            {
                                using (Font f = new Font("Arial", 18))
                                {
                                    g.DrawString(textToDisplay, f, b, textPos);
                                }
                            }
                        }


                        if (isClicked)
                        {
                            if (isCarryingItem)
                            {
                                Item replacedItem = Inventory.Items[index];
                                Inventory.Items[index] = ItemCarried;

                                if (replacedItem != null)
                                {
                                    ItemCarried = replacedItem;
                                    isCarryingItem = true;
                                    ItemPrevIndex = index;
                                }
                                else
                                {
                                    isCarryingItem = false;
                                    ItemCarried = null;
                                }
                            }
                            else
                            {
                                ItemCarried = item;
                                isCarryingItem = true;
                                Inventory.Items[index] = null;
                                ItemPrevIndex = index;
                            }
                        }

                        break;
                    }
                }
            }

            if (isCarryingItem)
            {
                Point mousePos = GameWindow.PointToClient(Cursor.Position);
                g.DrawImage(ItemCarried.Sprite, mousePos);
            }
        }
        private static void DrawPauseScreen(Graphics g)
        {
            using (SolidBrush b = new SolidBrush(Color.FromArgb(160, Color.Gray)))
            {
                g.FillRectangle(b, GameWindow.ClientRectangle);
            }
            string paused = "Paused!\n(ESC To Continue)";
            using (SolidBrush b = new SolidBrush(Color.Black))
            {
                using (Font f = new Font("Arial", 32))
                {
                    g.DrawString(paused, f, b, new PointF(GameWindow.ClientSize.Width - 370, 0));
                }
            }
        }

        internal static void ToggleInventory()
        {
            InventoryOpen = !InventoryOpen;
        }
        internal static void SetInventory(bool _value)
        {
            InventoryOpen = _value;
        }

        internal static void ToggleDrawGUI()
        {
            DrawGUI = !DrawGUI;
        }
        internal static void SetDrawGUI(bool _value)
        {
            DrawGUI = _value;
        }

        internal static void TogglePause()
        {
            Paused = !Paused;
            PauseUnpause();
        }
        internal static void SetPause(bool _value)
        {
            Paused = _value;
            PauseUnpause();
        }

        private static void PauseUnpause()
        {
            if (Paused)
                GameUpdate.StopTimer();
            if (!Paused)
                GameUpdate.StartTimer();
        }
    }
}
