using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VISUALNOVEL;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_VisualNovel : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            //Variable Assignment
            database.AddCommand("setplayername", new Action<string>(SetPlayerNameVariable));
            database.AddCommand("setbandname", new Action<string>(SetBandNameVariable));
            database.AddCommand("gohome", new Action<string>(GoHome));
            database.AddCommand("loadscene1", new Action<string>(LoadScene1));
            database.AddCommand("loadscene4", new Action<string>(LoadScene4));
            database.AddCommand("startsnow", new Action<string>(TurnOnSnowParticle));
            database.AddCommand("stopsnow", new Action<string>(TurnOffSnowParticle));
            database.AddCommand("startrain", new Action<string>(TurnOnRainParticle));
            database.AddCommand("stoprain", new Action<string>(TurnOffRainParticle));
        }

        private static void TurnOnSnowParticle(string data)
        {
            ParticlesControl.instance.TurnOnSnow();
        }

        private static void TurnOffSnowParticle(string data)
        {
            ParticlesControl.instance.TurnOffSnow();
        }

        private static void TurnOnRainParticle(string data)
        {
            ParticlesControl.instance.TurnOnRain();
        }

        private static void TurnOffRainParticle(string data)
        {
            ParticlesControl.instance.TurnOffRain();
        }

        private static void SetPlayerNameVariable(string data)
        {
            //VNGameSave.activeFile = VNGameSave.Load(VNGameSave.activeFile.filePath);
            VNGameSave.activeFile.SetPlayerName(data);
        }

        private static void SetBandNameVariable(string data)
        {
            //Debug.Log(data);
            //VariableStore.TrySetValue("VN.bandName", data);
            //VariableStore.PrintAllVariables();
            VNGameSave.activeFile.SetBandName(data);
        }

        private static void GoHome(string data)
        {

            VNMenuManager.instance.GoHome();
        }

        public static void LoadScene1(string data)
        {
            VN_Configuration.activeConfig.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
        }

        public static void LoadScene4(string data)
        {
            VN_Configuration.activeConfig.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene4");
        }

    }
}