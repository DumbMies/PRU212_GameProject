using UnityEngine;

public class ShowTextTrigger : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private Marisa marisa;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            marisa.tutorial.gameObject.SetActive(false);
            marisa.ShowText(message);
        }
    }
}
