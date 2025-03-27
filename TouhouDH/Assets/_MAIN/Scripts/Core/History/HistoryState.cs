using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    [System.Serializable]
    public class HistoryState 
    {
        public DialogueData dialogue;
        public List<CharacterData> characters;
        public List<AudioTrackData> audio;
        public List<AudioSFXData> sfx;
        public List<GraphicData> graphics;
        public ChoicePanelData choicePanel;
        public InputPanelData inputPanel;
        public bool isParticleActive;

        public static HistoryState Capture()
        {
            HistoryState state = new HistoryState();
            state.dialogue = DialogueData.Capture();
            state.characters = CharacterData.Capture();
            state.audio = AudioTrackData.Capture();
            state.sfx = AudioSFXData.Capture();
            state.graphics = GraphicData.Capture();
            state.choicePanel = ChoicePanelData.Capture();
            state.inputPanel = InputPanelData.Capture();
            state.isParticleActive = ParticlesControl.instance.IsParticleActive();

            return state;
        }
        public void Load()
        {
            DialogueData.Apply(dialogue);
            CharacterData.Apply(characters);
            AudioTrackData.Apply(audio);
            AudioSFXData.Apply(sfx);
            GraphicData.Apply(graphics);
            ChoicePanelData.Apply(choicePanel);
            InputPanelData.Apply(inputPanel);

            if (isParticleActive)
            {
                //ParticlesControl.instance.TurnOnSnow();
                ParticlesControl.instance.TurnOnRain();
            }
            else
            {
                //ParticlesControl.instance.TurnOffSnow();
                ParticlesControl.instance.TurnOffRain();
            }
        }
    }
}