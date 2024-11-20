using _2DRpgGame.Classes.Items;
using System.Windows.Forms;

namespace _2DRpgGame.Classes.GameControllers
{
    internal static class GameLoad
    {
        internal static void LoadControllers(Form _gameWindow)
        {
            GameUpdate.SetupUpdate(_gameWindow);
            GameDraw.SetupDraw(_gameWindow);

            LoadOther();
        }

        internal static void LoadOther()
        {
            GUI.InitGUI();
            GUI.ToggleDrawGUI();

            ItemDatabase.InitItems();
        }
    }
}
