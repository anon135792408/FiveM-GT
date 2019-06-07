using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Client
{
    public class Player : BaseScript
    {
        public static dynamic MapList = null;

        public static uint ChosenVehicleHash = 0;

        public Player()
        {
        }

        public static void UpdateChosenVehicle(string vehName)
        {
            var model = (uint)GetHashKey(vehName);
            if (!IsModelInCdimage(model) || !IsModelValid(model))
                return;

            ChosenVehicleHash = model;

            Debug.WriteLine("[FiveM-GT] Chosen vehicle set to " + GetDisplayNameFromVehicleModel(ChosenVehicleHash));
        }

        public async static void FollowRaceCoordinates(List<Vector3> coords)
        {
            IEnumerator<Vector3> currCoord = coords.GetEnumerator();

            while (true)
            {
                await Delay(250);

                if (!currCoord.MoveNext())
                {
                    Debug.WriteLine("[FiveM-GT] FollowRaceCoordinates Attempted to find first coordinate, none found!");
                    break;
                }

                Game.PlayerPed.Task.DriveTo(Game.PlayerPed.CurrentVehicle, currCoord.Current, 5f, 30f);
                while (!Game.PlayerPed.IsInRangeOf(currCoord.Current, 5.0f))
                {
                    await Delay(250);
                    Debug.WriteLine("Attempting to drive to: " + currCoord.Current.ToString());
                    Debug.WriteLine("Or " + coords[0].ToString());
                }

                if (!currCoord.MoveNext())
                    break;
            }
        }
    }
}
