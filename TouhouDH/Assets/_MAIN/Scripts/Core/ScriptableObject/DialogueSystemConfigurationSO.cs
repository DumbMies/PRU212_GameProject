using CHARACTERS;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue Sytem/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public const float DEFAULT_FONTSIZE_DIALOGUE = 21;
        public const float DEFAULT_FONTSIZE_NAME = 31;

        public CharacterConfigSO characterConfigurationAsset;

        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;

        public float dialogueFontScale = 1f;
        public float defaultDialogueFontSize = DEFAULT_FONTSIZE_DIALOGUE;
        public float defaultNameFontSize = DEFAULT_FONTSIZE_NAME;

    }
}