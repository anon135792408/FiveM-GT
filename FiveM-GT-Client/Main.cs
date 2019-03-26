using System;
using System.Collections.Generic;
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
            EventHandlers["FiveM-GT:StartRaceIntro"] += new Action(StartRaceIntro);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Debug.WriteLine("[FiveM-GT] Initiating FiveM Gran Turismo...");

            RegisterCommand("countdown", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("FiveM-GT:StartRaceIntroForAll");
            }), false);

            RegisterCommand("camtest", new Action<int, List<object>, string>((source, args, raw) =>
            {
                CamUtils.InitRaceStartCam();
            }), false);
        }

        private async void StartRaceIntro()
        {
            CamUtils.InitRaceStartCam();
            InitRaceIntro();

            await Delay(7000);
            InitCountdown();
        }
    }
}
