using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigMenu : MenuPage
{
    public static ConfigMenu instance { get; private set; }

    [SerializeField] private GameObject[] panels;
    private GameObject activePanel;

    public UI_ITEMS ui;

    private VN_Configuration config => VN_Configuration.activeConfig;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }

        activePanel = panels[0];

        SetAvailableResolutions();

        LoadConfig();
    }

    private void LoadConfig()
    {
        if (File.Exists(VN_Configuration.filePath))
        {
            VN_Configuration.activeConfig = FileManager.Load<VN_Configuration>(VN_Configuration.filePath, encrypt: VN_Configuration.ENCRYPT);
        }
        else
        {
            VN_Configuration.activeConfig = new VN_Configuration();
        }
        VN_Configuration.activeConfig.Load();
    }

    private void OnApplicationQuit()
    {
        VN_Configuration.activeConfig.Save();
        VN_Configuration.activeConfig = null;
    }

    public void OpenPanel(string panelName)
    {
        GameObject panel = panels.First(p => p.name == panelName);

        if (panel == null)
        {
            Debug.LogWarning($"Did not find panel called '{panelName}' in config menu.");
            return;
        }

        if(activePanel != null && activePanel != panel)
        {
            activePanel.SetActive(false);
        }

        panel.SetActive(true);
        activePanel = panel;
    }

    private void SetAvailableResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<string> options = new List<string>();

        for(int i = resolutions.Length - 1; i >= 0; i--)
        {
            options.Add($"{resolutions[i].width}x{resolutions[i].height}");
        }

        ui.resolutions.ClearOptions();
        ui.resolutions.AddOptions(options);
    }

    [System.Serializable]
    public class UI_ITEMS
    {
        private static Color button_selectedColor = new Color(0.7f, 0.7f, 0.7f, 1);
        private static Color button_unselectedColor = new Color(1f, 1f, 1f, 1f);
        private static Color text_selectedColor = new Color(1, 1f, 0, 1);
        private static Color text_unselectedColor = new Color(255, 255, 255, 1);

        [Header("General")]
        public Button fullscreen;
        public Button windowed;
        public TMP_Dropdown resolutions;
        public Button skippingContinue, skippingStop;
        public Slider architectSpeed, autoReaderSpeed;


        [Header("Audio")]
        public Slider musicVolume;
        public Slider sfxVolume;
        public Slider voicesVolume;

        public void SetButtonColors(Button A, Button B, bool seledtedA)
        {
            A.GetComponent<Image>().color = seledtedA ? button_selectedColor : button_unselectedColor;
            B.GetComponent<Image>().color = !seledtedA ? button_selectedColor : button_unselectedColor;

            A.GetComponentInChildren<TextMeshProUGUI>().color = seledtedA ? text_selectedColor : text_unselectedColor;
            B.GetComponentInChildren<TextMeshProUGUI>().color = !seledtedA ? text_selectedColor : text_unselectedColor;
        }
    }

    //UI CALLABLE FUNCTIONS 
    public void SetDisplayToFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        ui.SetButtonColors(ui.fullscreen, ui.windowed, fullscreen);
    }

    public void SetDisplayResolution()
    {
        string resolution = ui.resolutions.captionText.text;
        string[] values = resolution.Split('x');

        if (int.TryParse(values[0], out int width) && int.TryParse(values[1], out int height))
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
            config.display_resolution = resolution;
        }
        else
        {
            Debug.LogError($"Parsing error for screen resolution! [{resolution}] could not be parsed into WIDTHxHEIGHT");
        }
    }

    public void SetContinueSkippingAfterChoice(bool continueSkipping)
    {
        config.continueSkippingAfterChoice = continueSkipping;
        ui.SetButtonColors(ui.skippingContinue, ui.skippingStop, continueSkipping);
    }

    public void SetTextArchitectSpeed()
    {
        config.dialogueTextSpeed = ui.architectSpeed.value;

        if (DialogueSystem.instance != null)
        {
            DialogueSystem.instance.conversationManager.architect.speed = config.dialogueTextSpeed;
        }
    }

    public void SetAutoReaderSpeed()
    {
        config.dialogueAutoReadSpeed = ui.autoReaderSpeed.value;
        if (DialogueSystem.instance == null)
        {
            return;
        }

        AutoReader autoReader = DialogueSystem.instance.autoReader;
        if (autoReader != null)
        {
            autoReader.speed = config.dialogueAutoReadSpeed;
        }
        
    }

    public void SetMusicVolume()
    {
        config.musicVolume = ui.musicVolume.value;
        AudioManager.instance.SetMusicVolume(config.musicVolume);
    }

    public void SetSFXVolume()
    {
        config.sfxVolume = ui.sfxVolume.value;
        AudioManager.instance.SetSFXVolume(config.sfxVolume);
    }

    public void SetVoicesVolume()
    {
        config.voicesVolume = ui.voicesVolume.value;
        AudioMixerGroup mixer = AudioManager.instance.voicesMixer;
        AnimationCurve audioFalloff = AudioManager.instance.audioFalloffCurve;

        mixer.audioMixer.SetFloat(AudioManager.VOICES_VOLUME_PARAMETER_NAME, audioFalloff.Evaluate(config.voicesVolume));
    }
}
