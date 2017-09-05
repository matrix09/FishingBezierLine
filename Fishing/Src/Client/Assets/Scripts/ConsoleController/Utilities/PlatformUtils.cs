

using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Utilities
{
    public static class PlatformUtils
    {
        public static bool IsMobileDevice()
        {

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return true;
            }
            else
                return false;
        }
    }

       
}

