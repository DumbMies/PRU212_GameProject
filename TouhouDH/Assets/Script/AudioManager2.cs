using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip bgm;
    public AudioClip dashSound;
    public AudioClip meleeAttackSound;
    public AudioClip longAttackSound;
    public AudioClip ultimateSound;
    public AudioClip hurtSound;
    //public AudioClip deathSound; //this may not be needed

    private void Start()
    {
        BGMSource.clip = bgm;
        BGMSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        SFXSource.volume = volume;
        SFXSource.PlayOneShot(clip);
    }
}
