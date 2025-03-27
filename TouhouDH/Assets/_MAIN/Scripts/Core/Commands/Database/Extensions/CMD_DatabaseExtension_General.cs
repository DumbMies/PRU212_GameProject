using DIALOGUE;
using History;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate" };
        private static string[] PARAM_SPEED => new string[] { "-s", "-spd" };
        private static string[] PARAM_FILEPATH => new string[] { "-f", "-file", "-filepath" };
        private static string[] PARAM_ENQUEUE => new string[] { "-e", "-enqueue" };
        

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));

            //Dialogue System Control
            database.AddCommand("showui", new Action<string>(ShowUI));
            database.AddCommand("hideui", new Action<string>(HideUI));

            //Dialogue Controls
            database.AddCommand("showdb", new Func<string[], IEnumerator>(ShowDialogueBox)); 
            database.AddCommand("hidedb", new Func<string[], IEnumerator>(HideDialogueBox));

            database.AddCommand("load", new Action<string[]>(LoadNewDialogueFile));

            database.AddCommand("resetforscene", new Action<string>(ResetForScene));
            database.AddCommand("stopskip", new Action<string>(StopSkip));

        }

        private static void LoadNewDialogueFile(string[] data)
        {
            string fileName = string.Empty;
            bool enqueue = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_FILEPATH, out fileName);
            parameters.TryGetValue(PARAM_ENQUEUE, out enqueue, defaultValue: false);

            string filePath = FilePaths.GetPathToResource(FilePaths.resources_dialogueFiles, fileName);
            TextAsset file = Resources.Load<TextAsset>(filePath);

            if (file == null)
            {
                Debug.LogWarning($"File '{filePath}' could not be loaded from the dialogues files. Please ensure it exists within the '{FilePaths.resources_dialogueFiles}' resources folder.");
                return;
            }

            List<string> lines = FileManager.ReadTextAsset(file, includeBlankLines: true);
            Conversation newConversation = new Conversation(lines);

            if (enqueue)
            {
                DialogueSystem.instance.conversationManager.Enqueue(newConversation);
            }
            else
            {
                DialogueSystem.instance.conversationManager.StartConversation(newConversation);
            }
        }

        private static IEnumerator Wait(string data)
        {

            if (float.TryParse(data, out float time))
            {
                VNMenuManager.instance.DisablePanelRoot();
                ConversationManager conversationManager = DialogueSystem.instance.conversationManager;
                HistoryNavigation historyNavigation = HistoryManager.instance.navigation;
                conversationManager.allowUserPrompts = false;
                historyNavigation.stopNavigate = true;
                yield return new WaitForSeconds(time);
                //VNMenuManager.instance.EnablePanelRoot();
                conversationManager.allowUserPrompts = true;
                historyNavigation.stopNavigate = false;
            }
        }

        private static IEnumerator ShowDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Show(speed, immediate);
        }
        private static IEnumerator HideDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Hide(speed, immediate);
        }

        private static void ShowUI(string data)
        {
            VNMenuManager.instance.EnablePanelRoot();
        }

        private static void HideUI(string data)
        {
            VNMenuManager.instance.DisablePanelRoot();
        }

        private static IEnumerator ShowDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Show(speed, immediate);
        }
        private static IEnumerator HideDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Hide(speed, immediate);
        }

        private static void ResetForScene(string data)
        {
            AutoReader autoReader = DialogueSystem.instance?.autoReader;
            if (autoReader != null)
            {
                autoReader.Disable();
            }
            else
            {
                Debug.LogError("AutoReader is null!");
            }

            if (DialogueSystem.instance != null)
            {
                if (DialogueSystem.instance.conversationManager != null)
                {
                    DialogueSystem.instance.conversationManager.architect.speed = 1f;
                }
                else
                {
                    Debug.LogError("ConversationManager is null!");
                }
            }
            else
            {
                Debug.LogError("DialogueSystem.instance is null!");
            }
        }

        private static void StopSkip(string data)
        {
            AutoReader autoReader = DialogueSystem.instance.autoReader;
            autoReader.Disable();
        }
    }
}