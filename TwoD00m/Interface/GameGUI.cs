using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoD00m.Interface
{
    public class GameGUI
    {
        private
            List<GUI> playerGUI = new List<GUI>();

        public GameGUI(Hero hero) {

            playerGUI.Add( new PlayerShiftGUI( true , hero) );

            update();
            
        }

        public void DrawGUI()
        {
            foreach(var pGui in playerGUI)
            {
                pGui.draw();
            }
        }

        public void update()
        {
            foreach (var pGui in playerGUI)
            {
                pGui.update();
            }
        }
        
        public PlayerShiftGUI GetPlayerHUD()
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
