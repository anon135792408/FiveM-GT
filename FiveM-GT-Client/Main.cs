﻿using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using static FiveM_GT_Client.UserInterface;

namespace FiveM_GT_Client
{
    public class Main : BaseScript
    {
        private bool IsPlayerPlaying = true;

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
            if (IsPlayerPlaying)
            {
                Game.PlayerPed.CurrentVehicle.IsHandbrakeForcedOn = true;
                CamUtils.InitRaceStartCam();
                InitRaceIntro();

                while (CamUtils.IsIntroCamRunning)
                {
                    await Delay(0);
                }

                InitCountdown();
                await Delay(1000);
                CamUtils.InitCountdownCam();

                while(CamUtils.IsCountdownRunning)
                {
                    await Delay(0);
                }

                Game.PlayerPed.CurrentVehicle.IsHandbrakeForcedOn = false;
            }
        }
    }
}
