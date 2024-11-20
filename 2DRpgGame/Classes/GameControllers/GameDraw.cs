using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _2DRpgGame.Classes.GameControllers
{
    internal static class GameDraw
    {
        static Form GameWindow;
        internal static void SetupDraw(Form _gameWindow)
        {
            GameWindow = _gameWindow;
        }

        internal enum Layers
        {
            TopPrio,
            GUI,
            Entities,
            Background
        }

        static Dictionary<PaintEventHandler, Layers> DrawOrder = new Dictionary<PaintEventHandler, Layers>();

        internal static void AddPaintEvent(PaintEventHandler _paintEventHandler, Layers _layer)
        {
            DrawOrder.Add(_paintEventHandler, _layer);

            RefreshEvents();
        }

        internal static void RemovePaintEvent(PaintEventHandler _paintEventHandler)
        {
            DrawOrder.Remove(_paintEventHandler);

            RefreshEvents();
        }

        internal static void RefreshEvents()
        {
            foreach (var pair in DrawOrder)
            {
                GameWindow.Paint -= pair.Key;
            }


            foreach (var pair in DrawOrder.Where(p => p.Value == Layers.Background))
            {
                GameWindow.Paint += pair.Key;
            }
            foreach (var pair in DrawOrder.Where(p => p.Value == Layers.Entities))
            {
                GameWindow.Paint += pair.Key;
            }
            foreach (var pair in DrawOrder.Where(p => p.Value == Layers.GUI))
            {
                GameWindow.Paint += pair.Key;
            }
            foreach (var pair in DrawOrder.Where(p => p.Value == Layers.TopPrio))
            {
                GameWindow.Paint += pair.Key;
            }
        }
    }
}
