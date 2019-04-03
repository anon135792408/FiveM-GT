using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Client
{
    class Main : BaseScript
    {
        public static List<string> MapList = new List<string>();

        public Main()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName)
                return;

            Debug.WriteLine("[FiveM-GT] Initiating FiveM Gran Turismo...");

            RegisterCommand("countdown", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("FiveM-GT:StartRaceIntroForAll");
            }), false);

            RegisterCommand("camtest", new Action<int, List<object>, string>((source, args, raw) =>
            {
                CamUtils.InitRaceStartCam();
            }), false);

            SetUpPlayerConfig();
        }

        private void SetUpPlayerConfig()
        {
            Debug.WriteLine("[FiveM-GT] Updating user music volume variable to "+ UserConfig.MusicVolume + "...");
            SendNuiMessage("{\"type\":\"SetMusicVolume\",\"MusicVolume\":" + UserConfig.MusicVolume + "}");
        }
    }
}
