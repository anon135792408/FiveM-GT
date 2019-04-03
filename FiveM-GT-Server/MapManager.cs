using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Server
{
    public class MapManager : BaseScript
    {
        public static List<string> MapList = new List<string>();

        public MapManager()
        {
            LoadMaps();
        }

        public static void LoadMaps()
        {
            MapList.Clear();
            foreach (string dir in Directory.GetDirectories($@"resources/{GetCurrentResourceName()}/maps/"))
            {
                if (DoDataFilesExist(dir))
                {
                    MapList.Add(dir.Replace($"resources/{GetCurrentResourceName()}/maps/", ""));
                    Debug.WriteLine("[FiveM-GT] Found GT Map '" + new DirectoryInfo(dir).Name + "'");
                }
            }

            TriggerClientEvent("FiveM-GT:LoadMapList", MapList);

            foreach (var item in MapList)
            {
                Debug.WriteLine(item);
            }
        }

        public static bool DoDataFilesExist(string dir)
        {
            if (File.Exists(dir + "/spawn.json"))
            {
                return true;
            }
            return false;
        }
    }
}
