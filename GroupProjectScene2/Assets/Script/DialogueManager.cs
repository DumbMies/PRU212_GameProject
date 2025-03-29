using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Button continueButton;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private float bgmVolume = 0.3f;
    [SerializeField] private bool fadeOutMusicOnEnd = true;
    [SerializeField] private float fadeOutDuration = 2.0f;
    
    [Header("Dialogue Settings")]
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private string nextSceneName = "FightBoss";

    [System.Serializable]
    public class DialogueLine
    {
        public string characterName;
        [TextArea(3, 10)]
        public string dialogueText;
        public Sprite characterSprite;
    }

    [Header("Dialogue Sequence")]
    public List<DialogueLine> dialogueLines;

    private int currentLineIndex = 0;
    private bool isTextTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        continueButton.onClick.AddListener(ContinueDialogue);
        
        if (voiceAudioSource == null)
        {
            voiceAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        SetupBackgroundMusic();

        StartDialogue();
    }
    
    void SetupBackgroundMusic()
    {
        if (bgmAudioSource == null)
        {
            GameObject bgmObject = new GameObject("BackgroundMusic");
            bgmObject.transform.parent = transform;
            
            bgmAudioSource = bgmObject.AddComponent<AudioSource>();
            bgmAudioSource.playOnAwake = false;
            bgmAudioSource.loop = true;
            bgmAudioSource.volume = bgmVolume;
        }
        
        if (backgroundMusic != null)
        {
            bgmAudioSource.clip = backgroundMusic;
            bgmAudioSource.Play();
        }
    }

    void StartDialogue()
    {
        currentLineIndex = 0;
        DisplayDialogueLine();
    }

    void DisplayDialogueLine()
    {
        if (currentLineIndex < dialogueLines.Count)
        {
            DialogueLine currentLine = dialogueLines[currentLineIndex];

            characterNameText.text = currentLine.characterName;

            if (currentLine.characterSprite != null)
            {
                characterIcon.sprite = currentLine.characterSprite;
                characterIcon.gameObject.SetActive(true);
            }
            else
            {
                characterIcon.gameObject.SetActive(false);
            }

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeText(currentLine.dialogueText));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeText(string fullText)
    {
        isTextTyping = true;
        dialogueText.text = "";
        
        foreach (char letter in fullText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        
        isTextTyping = false;
    }

    public void ContinueDialogue()
    {
        if (isTextTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueLines[currentLineIndex].dialogueText;
            isTextTyping = false;
            return;
        }

        if (voiceAudioSource.isPlaying)
        {
            voiceAudioSource.Stop();
        }

        currentLineIndex++;
        DisplayDialogueLine();
    }

    void EndDialogue()
    {
        Debug.Log("Dialogue Ended, loading scene: " + nextSceneName);
        
        if (fadeOutMusicOnEnd && bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic());
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    
    IEnumerator FadeOutMusic()
    {
        float startVolume = bgmAudioSource.volume;
        float timer = 0;
        
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeOutDuration);
            yield return null;
        }
        
        bgmAudioSource.volume = 0;
        bgmAudioSource.Stop();
        
        SceneManager.LoadScene(nextSceneName);
    }
}