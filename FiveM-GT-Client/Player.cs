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
    }
}
