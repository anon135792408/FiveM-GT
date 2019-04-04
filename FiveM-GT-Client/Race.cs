using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using static FiveM_GT_Client.UserInterface;

namespace FiveM_GT_Client
{
    public class Race : BaseScript
    {
        private bool IsPlayerPlaying = true;

        public Race()
        {
            EventHandlers["FiveM-GT:StartRace"] += new Action<string>(StartRace);
            EventHandlers["FiveM-GT:SpawnPlayerInMap"] += new Action<Vector3, float>(SpawnPlayerInMap);
        }

        public void StartRace(string map)
        {
            if (IsPlayerPlaying && Player.ChosenVehicleHash != 0)
            {
                Debug.WriteLine("[FiveM-GT] Starting race on map " + map);
                StartRaceIntro();
            }
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

                Debug.WriteLine("[FiveM-GT] Playing Random Song...");
                SendNuiMessage("{\"type\":\"PlayRandomSong\",\"enable\":true}");
            }
        }

        private async void SpawnPlayerInMap(Vector3 position, float heading)
        {
            Vehicle v = await World.CreateVehicle(new Model((int)Player.ChosenVehicleHash), position, heading);
            SetPedIntoVehicle(Game.PlayerPed.Handle, v.Handle, -1);
        }
    }
}
