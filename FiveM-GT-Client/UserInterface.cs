using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM_GT_Client
{
    static class UserInterface
    {
        private static bool isOpeningMoviePlaying = false;

        public static bool IsOpeningMoviePlaying
        {
            get { return isOpeningMoviePlaying; }
        }

        public static void InitOpeningMovieAsync()
        {
            if (!IsOpeningMoviePlaying)
            {

            }
        }
    }
}
