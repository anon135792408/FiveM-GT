using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Client
{
    class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName)
                return;

            Debug.WriteLine("[FiveM-GT] Initiating FiveM Gran Turismo...");

            RegisterCommand("startrace", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("FiveM-GT:StartRaceForAll", args[0].ToString().Replace("\"", ""), int.Parse(args[1].ToString()));
            }), false);

            RegisterCommand("choosecar", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Player.UpdateChosenVehicle(args[0].ToString());

            }), false);

            RegisterCommand("camtest", new Action<int, List<object>, string>((source, args, raw) =>
            {
                CamUtils.InitRaceStartCam();
            }), false);

            RegisterCommand("soundoptions", new Action<int, List<object>, string>((source, args, raw) =>
            {
                SendNuiMessage("{\"type\":\"ShowSoundOptions\"}");
                SetNuiFocus(true, true);
            }), false);

            RegisterNUICallback("updateMusicVolume");
            EventHandlers["updateMusicVolume"] += new Action(UserConfig.SetMusicVolume);

            SetUpPlayerConfig();
        }

        private void RegisterNUICallback(string name, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            RegisterNuiCallbackType(name);

            EventHandlers[$"__cfx_nui:{name}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                callback.Invoke(body, resultCallback);
            });
        }

        private void SetUpPlayerConfig()
        {
            Debug.WriteLine("[FiveM-GT] Updating user music volume variable to "+ UserConfig.MusicVolume + "...");
            SendNuiMessage("{\"type\":\"SetMusicVolume\",\"MusicVolume\":" + UserConfig.MusicVolume + "}");
        }
    }
}
