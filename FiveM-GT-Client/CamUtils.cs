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

        public static bool IsIntroCamRunning { get { return isIntroCamRunning; } }

        public static async void InitRaceStartCam()
        {
            if (Game.PlayerPed != null && Game.PlayerPed.IsInVehicle())
            {
                Debug.WriteLine("[FiveM-GT] Initialising Race Start Cam...");

                isIntroCamRunning = true;

                Vector3 posInFront = Game.PlayerPed.Position + (Game.PlayerPed.ForwardVector * 20) + (Game.PlayerPed.UpVector * 2) - (Game.PlayerPed.RightVector + 1);
                Camera camStart = World.CreateCamera(posInFront, Vector3.Zero, 50f);
                camStart.PointAt(Game.PlayerPed.Position);

                Vector3 posBeside = Game.PlayerPed.Position + (Game.PlayerPed.ForwardVector * 5) + (Game.PlayerPed.UpVector * 2) + (Game.PlayerPed.RightVector * 2);
                Camera camEnd = World.CreateCamera(posBeside, Vector3.Zero, 50f);
                camEnd.PointAt(Game.PlayerPed.Position);

                World.RenderingCamera = camStart;

                camStart.InterpTo(camEnd, 8000, true, true);

                await Delay(8000);
                isIntroCamRunning = false;
            }
        }
    }
}
