using System.Text;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace ThirdPersonShooter
{

    public enum InputNames
    {
        moveForward = 0, moveBackward, moveLeft, moveRight, run, crouch, cover, fire,
        aim, use,reload, openInventory, pause ,equipRifle,equipPistol
    }
    public static class InputManager{
        static string playerPrefsSaveName = "InputManager";
        const char KEY_VALUE_SEPARATOR = ':';
        const char INPUT_SETTINGS_SEPARATOR = ',';

        static Dictionary<InputNames, KeyCode> keyMaps;

        static InputManager()
        {
            keyMaps = new Dictionary<InputNames, KeyCode>();
            SetDefault();
            //if (PlayerPrefs.GetString(playerPrefsSaveName).Length == 0)
            //{
            //    SetDefault();
            //}
            //else
            //{
            //    LoadKeySettings();
            //}
            PrintKeyMapSets();
        }

        public static void SetDefault()
        {

            //hard-coded!
            keyMaps.Add(InputNames.moveForward, KeyCode.W);
            keyMaps.Add(InputNames.moveBackward, KeyCode.S);
            keyMaps.Add(InputNames.moveLeft, KeyCode.A);
            keyMaps.Add(InputNames.moveRight, KeyCode.D);
            keyMaps.Add(InputNames.run, KeyCode.LeftShift);
            keyMaps.Add(InputNames.crouch, KeyCode.LeftControl);
            keyMaps.Add(InputNames.cover, KeyCode.Space);
            keyMaps.Add(InputNames.fire, KeyCode.Mouse0);
            keyMaps.Add(InputNames.aim, KeyCode.Mouse1);
            keyMaps.Add(InputNames.openInventory, KeyCode.Tab);
            keyMaps.Add(InputNames.pause, KeyCode.Escape);
            keyMaps.Add(InputNames.use, KeyCode.E);
            keyMaps.Add(InputNames.reload, KeyCode.R);
            keyMaps.Add(InputNames.equipRifle, KeyCode.Alpha1);
            keyMaps.Add(InputNames.equipPistol, KeyCode.Alpha2);

            //save set.
            SaveKeySettings();

        }
        public static void EditKeySettings()
        {

        }

        private static void SaveKeySettings()
        {

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<InputNames, KeyCode> keyPair in keyMaps)
            {
                builder.Append(keyPair.Key.ToString());
                builder.Append(KEY_VALUE_SEPARATOR);
                builder.Append(keyPair.Value);
                builder.Append(INPUT_SETTINGS_SEPARATOR);
            }
            string builtString = builder.ToString().TrimEnd(INPUT_SETTINGS_SEPARATOR);
            PlayerPrefs.SetString(playerPrefsSaveName, builtString);
            PlayerPrefs.Save();
        }
        private static void LoadKeySettings()
        {
            string loadedSetting = PlayerPrefs.GetString(playerPrefsSaveName);
            string[] keySets = loadedSetting.Split(INPUT_SETTINGS_SEPARATOR);

            for (int i = 0; i < keySets.Length; i++)
            {
                string[] keySet = keySets[i].Split(KEY_VALUE_SEPARATOR);
                keyMaps.Add((InputNames)Enum.Parse(typeof(InputNames), keySet[0]), (KeyCode)Enum.Parse(typeof(KeyCode), keySet[1]));
            }

        }

        //for debug.
        private static void PrintKeyMapSets()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<InputNames, KeyCode> keyPair in keyMaps)
            {
                builder.Append(keyPair.Key.ToString());
                builder.Append(KEY_VALUE_SEPARATOR);
                builder.Append(keyPair.Value);
                builder.Append(INPUT_SETTINGS_SEPARATOR);
            }
            string builtString = builder.ToString().TrimEnd(INPUT_SETTINGS_SEPARATOR);
            Debug.Log(builtString);
        }

        public static bool GetKey(InputNames name)
        {
            return Input.GetKey(keyMaps[name]);
        }

        public static bool GetKeyDown(InputNames name)
        {
            return Input.GetKeyDown(keyMaps[name]);
        }

        public static bool GetKeyUp(InputNames name)
        {
            return Input.GetKeyUp(keyMaps[name]);
        }
    }
}
