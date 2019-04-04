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
            EventHandlers["FiveM-GT:StartRaceForAll"] += new Action<Player, string>(StartRaceForAll);
        }

        public void StartRaceForAll([FromSource]Player player, string map)
        {
            if (MapManager.MapList.Contains(map))
            {
                Debug.WriteLine("[FiveM-GT] " + player.Name + " has requested to start a race with the map '" + map + "'");
                TriggerClientEvent("FiveM-GT:StartRace", map);
                List<string> spawns = MapManager.LoadMapSpawns(map);
            }
            else
            {
                Debug.WriteLine("[FiveM-GT] " + player.Name + " attempted to start a race with a map that was not found '" + map + "'");
                player.TriggerEvent("chatMessage", "[FiveM-GT] The map you requested '"+ map +"' was not found");
            }
        }
    }
}
