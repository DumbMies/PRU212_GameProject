using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    [System.Serializable]
    public class InputPanelData
    {
        public string currentTitle;
        public string lastInput;
        public bool isVisible;

        public static InputPanelData Capture()
        {
            InputPanelData data = new InputPanelData();

            if (InputPanel.instance != null)
            {
                data.currentTitle = InputPanel.instance.titleText.text;
                data.lastInput = InputPanel.instance.lastInput;
                data.isVisible = InputPanel.instance.isWaitingOnUserInput;
            }

            return data;
        }

        public static void Apply(InputPanelData data)
        {
            if (data != null && data.isVisible)
            {
                InputPanel.instance.Show(data.currentTitle);
                InputPanel.instance.inputField.text = data.lastInput;
            }
            else
            {
                InputPanel.instance.Hide();
            }
        }
    }
}
