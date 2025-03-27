using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VISUALNOVEL;

public class MainMenu : MonoBehaviour
{
    public const string MAIN_MENU_SCENE = "Main Menu";

    public static MainMenu instance { get; private set; }

    public AudioClip menuMusic;
    public CanvasGroup mainPanel;
    public CanvasGroup title;
    public CanvasGroup navBar;
    private CanvasGroupController mainCG;

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCG = new CanvasGroupController(this, mainPanel);

        mainPanel.alpha = 0;
        mainPanel.gameObject.SetActive(true);

        StartCoroutine(FadeInMainMenu());

        AudioManager.instance.StopAllSoundEffects();
        AudioManager.instance.StopAllTracks();

        AudioManager.instance.PlayTrack(menuMusic, channel: 0, startingVolume: 1);
    }

    private IEnumerator FadeInMainMenu()
    {
        float duration = 2f;
        float elapsedTime = 0;

        //Fade in main panel
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            mainPanel.alpha = alpha;
            yield return null;
        }
        mainPanel.alpha = 1;

        //yield return new WaitForSeconds(1f);

        //Fade in title       
        float titleDuration = 4f;
        elapsedTime = 0;
        while (elapsedTime < titleDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / titleDuration);
            title.alpha = alpha;
            yield return null;
        }
        title.alpha = 1;

        //Fade in navBar
        float navBarDuration = 1f;
        elapsedTime = 0;
        while (elapsedTime < navBarDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / navBarDuration);
            navBar.alpha = alpha;
            yield return null;
        }
        navBar.alpha = 1;
        navBar.interactable = true;
    }

    public void Click_StartNewGame()
    {
        uiChoiceMenu.Show("Bạn có chắc muốn chơi từ đầu không?", new UIConfirmationMenu.ConfirmationButton("Có", StartNewGame), new UIConfirmationMenu.ConfirmationButton("Không", null));
    }

    public void LoadGame(VNGameSave file)
    {
        VNGameSave.activeFile = file;
        VN_Configuration.activeConfig.Save();
        StartCoroutine(StartingGame());
    }

    private void StartNewGame()
    {
        VNGameSave.activeFile = new VNGameSave();
        StartCoroutine(StartingGame());
    }
    private IEnumerator StartingGame()
    {
        mainCG.Hide(speed: 0.3f);
        AudioManager.instance.StopTrack(0);

        while (mainCG.isVisible)
        {
            yield return null;
        }
        VN_Configuration.activeConfig.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("VisualNovel");
    }
}
