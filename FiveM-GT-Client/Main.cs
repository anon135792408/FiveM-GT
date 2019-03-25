using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using static FiveM_GT_Client.UserInterface; 

namespace FiveM_GT_Client
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Debug.WriteLine("[FiveM-GT] Initiating FiveM Gran Turismo...");

            InitOpeningMovieAsync();
        }
    }
}
