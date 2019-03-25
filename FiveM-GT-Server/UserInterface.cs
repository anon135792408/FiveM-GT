using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace FiveM_GT_Server
{
    public class UserInterface : BaseScript
    {
        public UserInterface()
        {
            EventHandlers["FiveM-GT:StartCountdownForAll"] += new Action(StartCountdownForAll);
        }

        public void StartCountdownForAll()
        {
            TriggerClientEvent("FiveM-GT:StartCountdown");
        }
    }
}
