using UnityEngine;
using UnityEngine.UI;

public class SceneUIController : MonoBehaviour
{
    public Button playAgainButton;

    void Start()
    {
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        }
    }

    void OnPlayAgainClicked()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.RestartGame();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("FightBoss");
        }
    }
}
