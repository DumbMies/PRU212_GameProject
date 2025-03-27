using COMMANDS;
using DIALOGUE;
using History;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VISUALNOVEL
{
    [System.Serializable]
    public class VNGameSave
    {
        public static VNGameSave activeFile = null;

        public const string FILE_TYPE = ".absgaming";
        public const string SCREENSHOT_FILE_TYPE = ".jpg";
        public const bool ENCRYPT = false;
        public const float SCREENSHOT_DOWNSCALE_AMOUNT = 0.25f;

        public string filePath => $"{FilePaths.gameSaves}{slotNumber}{FILE_TYPE}";
        public string screenshotPath => $"{FilePaths.gameSaves}{slotNumber}{SCREENSHOT_FILE_TYPE}";

        public string playerName;
        public string bandName;
        public int slotNumber = 1;

        public bool newGame = true;
        public string[] activeConversation;
        public HistoryState activeState;
        public HistoryState[] historyLogs;
        public VN_VariableData[] variables;

        public string timestamp;

        public static VNGameSave Load(string filePath, bool activateOnLoad = false)
        {
            VNGameSave save = FileManager.Load<VNGameSave>(filePath, ENCRYPT);

            activeFile = save;

            VariableStore.PrintAllVariables();

            if (activateOnLoad)
            {
                save.Activate();
                activeFile = save;
            }

            return save;
        }

        public void SetPlayerName(string data)
        {
            playerName = data;
            
            //VariableStore.TrySetValue<string>("VN.mainCharName", data);
            //Save();
        }

        public void SetBandName(string data)
        {
            bandName = data;
            if (bandName != null)
            {
                bandName = VariableStore.GetValue<string>("Default.bandName");
            }
            //Debug.Log(data);
            //Debug.Log(bandName);
            //Save();
        }

        public void Save()
        {
            Debug.Log(VariableStore.GetValue<string>("Default.bandName"));
            if (VariableStore.TryGetValue("Default.bandName", out object variable))
            {
                //Debug.Log("IT IS NULLLLLLLL");
                bandName = VariableStore.GetValue<string>("Default.bandName");
            }
            else if(VariableStore.TryGetValue("Default.bandName", out object v))
            {
                //Debug.Log("IT IS NULLLLLLLL 2");
                bandName = VariableStore.GetValue<string>("VN.bandName");
            }
            else
            {
                //Debug.Log("IT IS NOT NULLLLLLLL");
            }


            //if (InputPanel.instance.lastInput != "")
            //{
            //    bandName = InputPanel.instance.lastInput;
            //    Debug.Log("The Band Name: " + InputPanel.instance.lastInput);
            //}
            //else
            //{
            //    Debug.Log("Last input is null: " + InputPanel.instance.lastInput);
            //}


            //Debug.Log("Band name BEFORE: " + bandName);
            newGame = false;

            VariableStore.PrintAllVariables();

            activeState = HistoryState.Capture();
            historyLogs = HistoryManager.instance.history.ToArray();
            activeConversation = GetConversationData();
            variables = GetVariableData();

            timestamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

            ScreenshotMaster.CaptureScreenshot(VNManager.instance.mainCamera, Screen.width, Screen.height, SCREENSHOT_DOWNSCALE_AMOUNT, screenshotPath);

            //Debug.Log("Band name AFTER: " + bandName);

            string saveJSON = JsonUtility.ToJson(this);
            FileManager.Save(filePath, saveJSON, ENCRYPT);

        }

        public void Activate()
        {
            if (activeState != null)
            {
                activeState.Load();
            }
            VNMenuManager.instance.EnablePanelRoot();
            HistoryManager.instance.history = historyLogs.ToList();
            HistoryManager.instance.logManager.Clear();
            HistoryManager.instance.logManager.Rebuild();

            SetVariableData();

            SetConversationData();

            DialogueSystem.instance.prompt.Hide();
        }

        private string[] GetConversationData()
        {
            List<string> retData = new List<string>();

            var conversations = DialogueSystem.instance.conversationManager.GetConversationQueue();

            for (int i = 0; i < conversations.Length; i++)
            {
                var conversation = conversations[i];
                string data = "";

                if (conversation.file != string.Empty)
                {
                    var compressedData = new VN_ConversationDataCompressed();
                    compressedData.fileName = conversation.file;
                    compressedData.progress = conversation.GetProgress();
                    compressedData.startIndex = conversation.fileStartIndex;
                    compressedData.endIndex = conversation.fileEndIndex;
                    data = JsonUtility.ToJson(compressedData);
                }
                else
                {
                    var fullData = new VN_ConversationData();
                    fullData.conversation = conversation.GetLines();
                    fullData.progress = conversation.GetProgress();
                    data = JsonUtility.ToJson(fullData);
                }

                retData.Add(data);
            }

            return retData.ToArray();
        }

        private void SetConversationData()
        {
            for (int i = 0; i < activeConversation.Length; i++)
            {
                try
                {
                    string data = activeConversation[i];
                    Conversation conversation = null;

                    var fullData = JsonUtility.FromJson<VN_ConversationData>(data);
                    if (fullData != null && fullData.conversation != null && fullData.conversation.Count > 0)
                    {
                        conversation = new Conversation(fullData.conversation, fullData.progress);
                    }
                    else
                    {
                        var compressedData = JsonUtility.FromJson<VN_ConversationDataCompressed>(data);
                        if (compressedData != null && compressedData.fileName != string.Empty)
                        {
                            TextAsset file = Resources.Load<TextAsset>(compressedData.fileName);

                            int count = compressedData.endIndex - compressedData.startIndex;

                            List<string> lines = FileManager.ReadTextAsset(file).Skip(compressedData.startIndex).Take(count + 1).ToList();

                            conversation = new Conversation(lines, compressedData.progress, compressedData.fileName, compressedData.startIndex, compressedData.endIndex);
                        }
                        else
                        {
                            Debug.LogError($"Unknown conversation format! Unable to reload conversation from VNGameSave using data '{data}'");
                        }
                    }

                    if (conversation != null && conversation.GetLines().Count > 0)
                    {
                        if (i == 0)
                        {
                            DialogueSystem.instance.conversationManager.StartConversation(conversation);
                        }
                        else
                        {
                            DialogueSystem.instance.conversationManager.Enqueue(conversation);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Encountered error while extracting saved conversation data! {e}");
                    continue;
                }

            }
        }

        private VN_VariableData[] GetVariableData()
        {
            List<VN_VariableData> retData = new List<VN_VariableData>();
            foreach (var database in VariableStore.databases.Values)
            {
                foreach (var variable in database.variables)
                {    
                    VN_VariableData variableData = new VN_VariableData();
                    variableData.name = $"{database.name}.{variable.Key}";
                    string val = $"{variable.Value.Get()}";
                    variableData.value = val;
                    //variableData.type = val;
                    variableData.type = val == string.Empty ? "System.String" : variable.Value.Get().GetType().ToString();
                    retData.Add(variableData);
                }
            }

            return retData.ToArray();
        }

        private void SetVariableData()
        {
            foreach (var variable in variables)
            {
                string val = variable.value;

                switch (variable.type)
                {
                    case "System.Boolean":
                        if (bool.TryParse(val, out bool b_val))
                        {
                            VariableStore.TrySetValue(variable.name, b_val);
                            continue;
                        }
                        break;
                    case "System.Int32":
                        if (int.TryParse(val, out int i_val))
                        {
                            VariableStore.TrySetValue(variable.name, i_val);
                            continue;
                        }
                        break;
                    case "System.Single":
                        if (float.TryParse(val, out float f_val))
                        {
                            VariableStore.TrySetValue(variable.name, f_val);
                            continue;
                        }
                        break;
                    case "System.String":
                        VariableStore.TrySetValue(variable.name, val);
                        continue;
                }

                Debug.LogError($"Could not interpret variable type. {variable.name} = {variable.type}");
            }
        }

    }
}