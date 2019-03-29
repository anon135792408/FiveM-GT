using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Client
{
    class CamUtils : BaseScript
    {

        private static bool isIntroCamRunning = false;
        private static bool isCountdownRunning = false;

        public static bool IsIntroCamRunning { get { return isIntroCamRunning; } }
        public static bool IsCountdownRunning { get { return isCountdownRunning; } }

        public CamUtils()
        {
            //Tick += PointAtPlayer;
        }

        public static async void InitRaceStartCam()
        {
            if (Game.PlayerPed != null && Game.PlayerPed.IsInVehicle())
            {
                var target = Game.PlayerPed.CurrentVehicle;

                Debug.WriteLine("[FiveM-GT] Initialising Race Start Cam...");

                isIntroCamRunning = true;

                Vector3 posInFront = Game.PlayerPed.Position + (Game.PlayerPed.ForwardVector * 20) + (Game.PlayerPed.UpVector * 2) - (Game.PlayerPed.RightVector + 1);
                Camera camStart = World.CreateCamera(posInFront, Vector3.Zero, 50f);
                camStart.PointAt(target);

                Vector3 posBeside = Game.PlayerPed.Position + (Game.PlayerPed.ForwardVector * 5) + (Game.PlayerPed.UpVector * 2) + (Game.PlayerPed.RightVector * 2);
                Camera camEnd = World.CreateCamera(posBeside, Vector3.Zero, 50f);
                camEnd.PointAt(target);

                RenderScriptCams(true, false, 0, true, false);

                camStart.InterpTo(camEnd, 8000, false, false);

                await Delay(7000);
                isIntroCamRunning = false;
            }
        }

        public static async void InitCountdownCam()
        {
            if (Game.PlayerPed != null && Game.PlayerPed.IsInVehicle())
            {
                Vector3[] camCheckpoints = new Vector3[] {
                    ((Game.PlayerPed.Position - Game.PlayerPed.ForwardVector * 4) - Game.PlayerPed.RightVector * 2),    //Back left
                    ((Game.PlayerPed.Position - Game.PlayerPed.ForwardVector * 4) + Game.PlayerPed.RightVector * 2),    //Back right
                    ((Game.PlayerPed.Position + Game.PlayerPed.ForwardVector * 4) - Game.PlayerPed.RightVector * 2),    //Front right 
                    ((Game.PlayerPed.Position + Game.PlayerPed.ForwardVector * 4) + Game.PlayerPed.RightVector * 2)     //Front left
                };

                var target = Game.PlayerPed.CurrentVehicle;

                Debug.WriteLine("[FiveM-GT] Initialising Countdown Cam...");

                isCountdownRunning = true;

                int ind = 0;

                while (ind < camCheckpoints.Length - 1)
                {
                    Vector3 camPosStart = camCheckpoints[ind];
                    Camera camStart = World.CreateCamera(camPosStart, Vector3.Zero, 50f);
                    camStart.PointAt(target);

                    RenderScriptCams(true, false, 0, true, false);

                    Vector3 camPosEnd = camCheckpoints[ind+1];
                    Camera camEnd = World.CreateCamera(camPosEnd, Vector3.Zero, 50f);
                    camEnd.PointAt(target);

                    camStart.InterpTo(camEnd, 1000, true, false);

                    await Delay(1000);

                    ind+=2;
                }

                World.DestroyAllCameras();

                RenderScriptCams(false, false, 0, true, false);

                SetGameplayCamRelativeHeading(0f);

                await Delay(1000);

                isCountdownRunning = false;
            }
        }

        private async Task PointAtPlayer()
        {
            await Delay(0);

            if (isIntroCamRunning)
            {
                World.RenderingCamera.PointAt(Game.PlayerPed);
            }
        }
    }
}
