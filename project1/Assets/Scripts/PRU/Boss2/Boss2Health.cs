using UnityEngine;
using UnityEngine.UI;

public class Boss2Health : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private int maxHealth = 5;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHearts();
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    private void UpdateHearts()
    {
        //for (int i = 0; i < hearts.Length; i++)
        //{
        //    if (i < currentHealth)
        //    {
        //        hearts[i].sprite = fullHeart;
        //    }
        //    else
        //    {
        //        hearts[i].sprite = emptyHeart;
        //    }
        //}
    }
}
