using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ConsoleController.Console
{
    public static class SystemController
    {

        public static void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public static void ResumeGame()
        {
            Time.timeScale = 1f;
        }

    }
}

