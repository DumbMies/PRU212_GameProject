using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Marisa player { get; private set; }

    public int playerHealth;
    public bool isGamePaused;

    private void Awake()
    {
        // Ensure that there is only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize game state
        playerHealth = 3;
        isGamePaused = false;
        player = FindFirstObjectByType<Marisa>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveSystem.Save();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveSystem.Load();
        }
    }

    public void TakeDamage(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Pause the game
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume the game
    }

    public void GameOver()
    {
        // Handle game over logic
        Debug.Log("Game Over");
        // Load Game Over scene or show Game Over UI
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
