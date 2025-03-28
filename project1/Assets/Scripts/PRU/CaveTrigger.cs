using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaveTrigger : MonoBehaviour
{
    private bool hasExited = false;
    private GameObject boss2;
    private Light2D globalLight;
    private Color targetColor;
    private float transitionDuration = 2f; // Time in seconds for the transition

    private void Start()
    {
        boss2 = GameObject.Find("Boss");
        globalLight = GameObject.Find("Global Light 2D")?.GetComponent<Light2D>();
        targetColor = HexToColor("#000000");
    }

    private Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        return Color.white;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (hasExited || globalLight == null) return;

        if (other.CompareTag("Player"))
        {
            hasExited = true;
            StartCoroutine(FadeToColor(targetColor, transitionDuration));
        }
    }

    private IEnumerator FadeToColor(Color target, float duration)
    {
        Color startColor = globalLight.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            globalLight.color = Color.Lerp(startColor, target, elapsedTime / duration);
            yield return null; // Wait for the next frame
        }

        globalLight.color = target; // Ensure final color is set
    }
}
