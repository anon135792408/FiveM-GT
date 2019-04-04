using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                    string MapName = dir.Replace($"resources/{GetCurrentResourceName()}/maps/", "");
                    MapList.Add(MapName);
                    Debug.WriteLine("[FiveM-GT] Found GT Map '" + MapName + "'");
                }
            }

            TriggerClientEvent("FiveM-GT:LoadMapList", MapList);

            foreach (var item in MapList)
            {
                Debug.WriteLine(item);
            }
        }

        public static IDictionary<string, object> LoadMapJson(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                return JsonConvert.DeserializeObject<IDictionary<String, Object>>(r.ReadToEnd());
            }
        }

        public static List<string> LoadMapSpawns(string map)
        {
            List<string> spawns = new List<string>();
            string mapDir = $"resources/{GetCurrentResourceName()}/maps/"+map;

            if (!DoDataFilesExist(mapDir))
                return null;

            string file = mapDir + "/spawn.json";

            foreach(var item in LoadMapJson(file))
            {
                spawns.Add(item.Value.ToString());
            }

            foreach (string s in spawns)
            {
                Debug.WriteLine(s);
            }

            return spawns;
        }

        public static bool DoDataFilesExist(string dir)
        {
            string[] FilesToFind = {
                "spawn.json",
                "checkpoints.json",
                "mapinfo.json"
            };

            foreach (string s in FilesToFind) {
                if (!File.Exists(dir + "/" + s))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
