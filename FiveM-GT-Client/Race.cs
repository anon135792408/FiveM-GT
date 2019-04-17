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
        private int Laps = 1;
        private int CurrentLap = 1;
        private List<Vector3> Checkpoints;

        public Race()
        {
            EventHandlers["FiveM-GT:StartRace"] += new Action<string, int>(StartRace);
            EventHandlers["FiveM-GT:SpawnPlayerInMap"] += new Action<Vector3, float>(SpawnPlayerInMap);
            EventHandlers["FiveM-GT:DownloadRaceCheckpoints"] += new Action<List<dynamic>>(DownloadRaceCheckpoints);
        }

        public void StartRace(string map, int laps)
        {
            if (IsPlayerPlaying && Player.ChosenVehicleHash != 0)
            {
                Debug.WriteLine("[FiveM-GT] Starting "+laps.ToString()+" lap race on map " + map);
                Laps = laps;
                SendNuiMessage("{\"type\":\"SetLaps\",\"Laps\":" + Laps.ToString() + "}");
                StartRaceIntro();
            }
        }

        private void DownloadRaceCheckpoints(List<dynamic> checkpoints)
        {
            Checkpoints = new List<Vector3>();

            foreach (object o in checkpoints)
            {
                Checkpoints.Add((Vector3)o);
                Debug.WriteLine("[FiveM-GT] Parsing checkpoint " + ((Vector3)o).ToString());
            }

            Debug.WriteLine("[FiveM-GT] Parsed " + Checkpoints.Count + " race checkpoints...");
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
