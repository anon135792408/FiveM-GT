using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Client
{
    class Events : BaseScript
    {
        public Events()
        {
            EventHandlers["FiveM-GT:LoadMapList"] += new Action<dynamic>(LoadMapList);
        }

        public static void LoadMapList(dynamic mapList)
        {
            Debug.WriteLine("[FiveM-GT] Updating found maps...");
            Player.MapList = mapList;
        }
    }
}
