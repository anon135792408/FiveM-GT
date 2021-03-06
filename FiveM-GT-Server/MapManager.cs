﻿using System;
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
        }

        public static IDictionary<string, object> LoadMapJson(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                return JsonConvert.DeserializeObject<IDictionary<String, Object>>(r.ReadToEnd());
            }
        }

        public static void SendCheckpointsToPlayer(Player player, string map)
        {
            if (MapList.Contains(map))
            {
                Debug.WriteLine("[FiveM-GT] " + player.Name + " is downloading checkpoints for map " + GetMapName(map) + "...");
                List<Vector3> checkpoints = RetrieveCheckpoints(map);
                player.TriggerEvent("FiveM-GT:DownloadRaceCheckpoints", checkpoints);
            }
        }

        public static List<Vector3> RetrieveCheckpoints(string map)
        {
            Debug.WriteLine("[FiveM-GT] Retrieving checkpoints for map " + GetMapName(map));

            List<Vector3> checkpoints = new List<Vector3>();
            string mapDir = $"resources/{GetCurrentResourceName()}/maps/" + map;

            if (!DoDataFilesExist(mapDir))
                return null;

            string file = mapDir + "/checkpoints.json";

            foreach (var item in LoadMapJson(file))
            {
                string[] coords = item.ToString().Split(',');
                Vector3 coord = new Vector3(float.Parse(coords[1]), float.Parse(coords[2]), float.Parse(coords[3]));
                Debug.WriteLine("[FiveM-GT] Parsing map checkpoint " + coord.ToString());
                checkpoints.Add(coord);
            }

            return checkpoints;
        }

        public static string GetMapName(string map)
        {
            string mapDir = $"resources/{GetCurrentResourceName()}/maps/" + map;

            if (!DoDataFilesExist(mapDir))
                return null;

            string file = mapDir + "/mapinfo.json";

            foreach (var item in LoadMapJson(file))
            {
                if (item.Key.ToString().Equals("name"))
                {
                    return item.Value.ToString();
                }
            }

            return "NO-NAME";
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
