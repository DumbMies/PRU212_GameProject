using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string gameOverSceneName = "GameOver";
    public string winnerSceneName = "Winner";
    public string mainSceneName = "FightBoss";

    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void GameOver()
    {
        Debug.Log("Game Over - Player Died");
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void PlayerWins()
    {
        Debug.Log("Player Wins - Boss Defeated");
        SceneManager.LoadScene(winnerSceneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}