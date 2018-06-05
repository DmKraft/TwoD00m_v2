using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.Interface
{
    public static class GameGUI
    {
        private static List<GUI> playerGUI = new List<GUI>();

        public static void SetUp(Hero hero)
        {
            playerGUI.Add(new PlayerShiftGUI(true, hero));
            Update();
        }

        public static void Draw()
        {
            foreach (var pGui in playerGUI)
            {
                pGui.draw();
            }
        }

        public static void Update()
        {
            foreach (var pGui in playerGUI)
            {
                pGui.update();
            }
        }

        public static PlayerShiftGUI GetPlayerHUD()
        {
            foreach (var pGui in playerGUI)
            {
                if (pGui is PlayerShiftGUI)
                    return (PlayerShiftGUI)pGui;
            }
            return null;
        }
    }
}
