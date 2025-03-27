using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    [System.Serializable]
    public class ChoicePanelData
    {
        public string currentQuestion;
        public string[] currentChoices;
        public bool isVisible;

        public static ChoicePanelData Capture()
        {
            
            ChoicePanelData data = new ChoicePanelData();

            if (ChoicePanel.instance != null)
            {
                data.currentQuestion = ChoicePanel.instance.lastDecision?.question;
                data.currentChoices = ChoicePanel.instance.lastDecision?.choices;
                data.isVisible = ChoicePanel.instance.isWaitingOnUserChoice;
            }

            return data;
        }

        public static void Apply(ChoicePanelData data)
        {
            if (data != null && data.isVisible && data.currentChoices != null)
            {
                ChoicePanel.instance.Show(data.currentQuestion, data.currentChoices);
            }
            else
            {
                ChoicePanel.instance.Hide();
                ChoicePanel.instance.SetWaitingOnUserChoice(false);
            }
        }
    }
}
