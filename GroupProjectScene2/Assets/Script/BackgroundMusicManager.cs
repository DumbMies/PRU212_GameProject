using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }

    private AudioSource mainAudioSource;
    private AudioSource secondaryAudioSource;

    public AudioClip battleMusic;
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;

    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0.1f, 3f)]
    public float fadeTime = 1.5f;

    private bool isTransitioning = false;
    private AudioClip currentMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupAudioSources()
    {
        mainAudioSource = gameObject.AddComponent<AudioSource>();
        mainAudioSource.loop = true;
        mainAudioSource.volume = musicVolume;
        mainAudioSource.playOnAwake = false;

        secondaryAudioSource = gameObject.AddComponent<AudioSource>();
        secondaryAudioSource.loop = true;
        secondaryAudioSource.volume = 0f;
        secondaryAudioSource.playOnAwake = false;
    }

    void Start()
    {
        if (battleMusic != null)
        {
            PlayMusic(battleMusic);
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;

        if (isTransitioning || currentMusic == musicClip) return;

        if (mainAudioSource.isPlaying)
        {
            StartCoroutine(CrossFadeMusic(musicClip));
        }
        else
        {
            mainAudioSource.clip = musicClip;
            mainAudioSource.volume = musicVolume;
            mainAudioSource.Play();
            currentMusic = musicClip;
        }
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic != null)
        {
            PlayMusic(victoryMusic);
        }
    }

    public void PlayDefeatMusic()
    {
        if (defeatMusic != null)
        {
            PlayMusic(defeatMusic);
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        mainAudioSource.volume = musicVolume;
    }

    private IEnumerator CrossFadeMusic(AudioClip newClip)
    {
        isTransitioning = true;

        secondaryAudioSource.clip = newClip;
        secondaryAudioSource.Play();

        float timeElapsed = 0f;

        while (timeElapsed < fadeTime)
        {
            mainAudioSource.volume = Mathf.Lerp(musicVolume, 0f, timeElapsed / fadeTime);
            secondaryAudioSource.volume = Mathf.Lerp(0f, musicVolume, timeElapsed / fadeTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainAudioSource.volume = 0f;
        secondaryAudioSource.volume = musicVolume;

        AudioSource temp = mainAudioSource;
        mainAudioSource = secondaryAudioSource;
        secondaryAudioSource = temp;

        secondaryAudioSource.Stop();

        currentMusic = newClip;
        isTransitioning = false;
    }
}
