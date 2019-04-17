using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace FiveM_GT_Server
{
    public class Race : BaseScript
    {
        public Race()
        {
            EventHandlers["FiveM-GT:StartRaceForAll"] += new Action<Player, string, int>(StartRaceForAll);
        }

        public void StartRaceForAll([FromSource]Player player, string map, int laps)
        {
            if (MapManager.MapList.Contains(map))
            {
                Debug.WriteLine("[FiveM-GT] " + player.Name + " has requested to start a "+laps.ToString()+" lap race with the map '" + map + "'");
                List<string> spawns = MapManager.LoadMapSpawns(map);
                if (AssignRaceSpawnPositions(Players, spawns))
                    TriggerClientEvent("FiveM-GT:StartRace", MapManager.GetMapName(map), laps);

                MapManager.SendCheckpointsToPlayer(player, map);
            }
            else
            {
                Debug.WriteLine("[FiveM-GT] " + player.Name + " attempted to start a " + laps.ToString() + " lap race with a map that was not found '" + map + "'");
                player.TriggerEvent("chatMessage", "[FiveM-GT] The map you requested '"+ map +"' was not found");
            }
        }

        public bool AssignRaceSpawnPositions(PlayerList players, List<string> spawns)
        {
            Player[] playerList = players.ToArray();
            List<string> unusedSpawns = new List<string>(spawns);

            Random rand = new Random();

            if (playerList.Length <= unusedSpawns.Count)
            {
                for (int i = 0; i < playerList.Length; i++)
                {
                    int spawnIndex = rand.Next(unusedSpawns.Count);
                    string[] spawnInfo = unusedSpawns[spawnIndex].Split(',');

                    Vector3 position = new Vector3(float.Parse(spawnInfo[0]), float.Parse(spawnInfo[1]), float.Parse(spawnInfo[2]));
                    float heading = float.Parse(spawnInfo[3]);

                    Debug.WriteLine("[FiveM-GT] Assigning " + playerList[i].Name + " to spawn position " + (spawnIndex+1).ToString());
                    playerList[i].TriggerEvent("FiveM-GT:SpawnPlayerInMap", position, heading);

                    unusedSpawns.Remove(unusedSpawns[spawnIndex]);
                }
                return true;
            }
            else
            {
                Debug.WriteLine("[FiveM-GT] Chosen map does not have the capacity to support " + playerList.Length + " players!");
                return false;
            }
        }
    }
}
