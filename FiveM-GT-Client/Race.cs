using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private Vector3 CurrentCheckpoint;
        private int CheckpointIndex = 0;

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
                CurrentLap = 1;
                SendNuiMessage("{\"type\":\"SetLaps\",\"Laps\":" + Laps.ToString() + "}");
                StartRaceIntro();
            }
        }

        private async Task CheckpointTick()
        {
            if (CurrentLap == Laps && Checkpoints[Checkpoints.Count-1].Equals(CurrentCheckpoint))
            {
                World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(CurrentCheckpoint.X, CurrentCheckpoint.Y, CurrentCheckpoint.Z - 1), Vector3.Zero, Vector3.Zero, new Vector3(15f, 15f, 15f), System.Drawing.Color.FromArgb(155, 204, 102, 0), false, true, false);
                World.DrawMarker(MarkerType.CheckeredFlagCircle, new Vector3(CurrentCheckpoint.X, CurrentCheckpoint.Y, CurrentCheckpoint.Z + 5), Vector3.Zero, Vector3.Zero, new Vector3(15f, 15f, 15f), System.Drawing.Color.FromArgb(255, 255, 255), false, true, false);
                if (Game.PlayerPed.IsInRangeOf(CurrentCheckpoint, 15f))
                {
                    Debug.WriteLine("[FiveM-GT] You have finished the race!");
                    EndRaceLocally();
                    Tick -= CheckpointTick;
                }
            }
            else
            {
                World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(CurrentCheckpoint.X, CurrentCheckpoint.Y, CurrentCheckpoint.Z - 1), Vector3.Zero, Vector3.Zero, new Vector3(15f, 15f, 15f), System.Drawing.Color.FromArgb(155, 204, 102, 0), false, true, false);
                if (Game.PlayerPed.IsInRangeOf(CurrentCheckpoint, 15f))
                {
                    if (CheckpointIndex < Checkpoints.Count - 1)
                    {
                        Debug.WriteLine("[FiveM-GT] Passed checkpoint " + CheckpointIndex.ToString() + "!");
                        CheckpointIndex++;
                        CurrentCheckpoint = Checkpoints[CheckpointIndex];
                    }
                    else
                    {
                        Debug.WriteLine("[FiveM-GT] Passed final checkpoint " + CheckpointIndex.ToString() + "!");
                        CheckpointIndex = 0;
                        CurrentCheckpoint = Checkpoints[CheckpointIndex];
                        SendNuiMessage("{\"type\":\"SetRaceCurrentLap\",\"Lap\":" + CurrentLap.ToString() + "}");
                    }
                }
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

        private async void EndRaceLocally()
        {
            Debug.WriteLine("[FiveM-GT] Stopping Music...");
            SendNuiMessage("{\"type\":\"StopAllMusic\",\"enable\":true}");

            Debug.WriteLine("[FiveM-GT] Playing Race Finish Music...");
            SendNuiMessage("{\"type\":\"PlayFinishingSong\",\"enable\":true}");

            Player.FollowRaceCoordinates(Checkpoints);
        }

        private async void StartRaceIntro()
        {
            Debug.WriteLine("[FiveM-GT] Initiating race introduction set up...");
            if (IsPlayerPlaying)
            {
                await Delay(1000);

                Game.PlayerPed.CurrentVehicle.IsHandbrakeForcedOn = true;
                Debug.WriteLine("[FiveM-GT] Setting up introduction camera...");
                CamUtils.InitRaceStartCam();

                Debug.WriteLine("[FiveM-GT] Initiating race introduction...");
                InitRaceIntro();

                while (CamUtils.IsIntroCamRunning)
                {
                    await Delay(0);
                }

                InitCountdown();

                Debug.WriteLine("[FiveM-GT] Initiating Countdown...");

                await Delay(1000);
                CamUtils.InitCountdownCam();

                while(CamUtils.IsCountdownRunning)
                {
                    await Delay(0);
                }

                Game.PlayerPed.CurrentVehicle.IsHandbrakeForcedOn = false;

                Debug.WriteLine("[FiveM-GT] Playing Random Song...");
                SendNuiMessage("{\"type\":\"PlayRandomSong\",\"enable\":true}");
                Debug.WriteLine("[FiveM-GT] Resetting Current Checkpoint...");
                CurrentCheckpoint = Checkpoints[0];
                Debug.WriteLine("[FiveM-GT] Initialising checkpoint system...");
                Tick += CheckpointTick;
            }
        }

        private async void SpawnPlayerInMap(Vector3 position, float heading)
        {
            Vehicle v = await World.CreateVehicle(new Model((int)Player.ChosenVehicleHash), position, heading);
            SetFocusArea(v.Position.X, v.Position.Y, v.Position.Z, 0f, 0f, 0f);
            Game.PlayerPed.Position = v.Position;
            SetPedIntoVehicle(Game.PlayerPed.Handle, v.Handle, -1);
            ClearFocus();
        }
    }
}
