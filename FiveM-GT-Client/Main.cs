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
            SetNuiFocus(false, false);

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

            RegisterNUICallback("updateMusicVolume", NUI_UpdateMusicVolume);
            RegisterNUICallback("updateSfxVolume", NUI_UpdateSfxVolume);
            RegisterNUICallback("endNuiFocus", NUI_EndNuiFocus);

            SetUpPlayerConfig();
        }

        private void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            RegisterNuiCallbackType(msg);

            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                CallbackDelegate err = callback.Invoke(body, resultCallback);
            });
        }

        private CallbackDelegate NUI_UpdateMusicVolume(IDictionary<string, object> data, CallbackDelegate result)
        {
            object volume = "";

            if (data.TryGetValue("musicVolume", out volume)) {
                result("ok");
                UserConfig.MusicVolume = int.Parse(volume.ToString());
                Debug.WriteLine("[FiveM-GT] Updating user music volume variable to " + UserConfig.MusicVolume + "...");
                SendNuiMessage("{\"type\":\"SetMusicVolume\",\"MusicVolume\":" + UserConfig.MusicVolume + "}");
            }
            return result;
        }

        private CallbackDelegate NUI_EndNuiFocus(IDictionary<string, object> data, CallbackDelegate result)
        {
            result("ok");
            SetNuiFocus(false, false);
            return result;
        }

        private CallbackDelegate NUI_UpdateSfxVolume(IDictionary<string, object> data, CallbackDelegate result)
        {
            object volume = "";

            if (data.TryGetValue("sfxVolume", out volume))
            {
                result("ok");
                UserConfig.SfxVolume = int.Parse(volume.ToString());
                Debug.WriteLine("[FiveM-GT] Updating user music volume variable to " + UserConfig.SfxVolume + "...");
                SendNuiMessage("{\"type\":\"SetSfxVolume\",\"SfxVolume\":" + UserConfig.SfxVolume + "}");
            }
            return result;
        }

        private void SetUpPlayerConfig()
        {
            Debug.WriteLine("[FiveM-GT] Updating user music volume variable to "+ UserConfig.MusicVolume + "...");
            SendNuiMessage("{\"type\":\"SetMusicVolume\",\"MusicVolume\":" + UserConfig.MusicVolume + "}");
        }
    }
}
