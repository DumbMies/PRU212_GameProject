using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class EyeWakeEffect : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private RectTransform upperEyelid;
    [SerializeField] private RectTransform lowerEyelid;
    [SerializeField] private Image bedroomBackground;

    [Header("Animation Settings")]
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private float blinkPauseDuration = 0.3f;
    [SerializeField] private int numberOfBlinks = 3;
    [SerializeField] private float finalOpenDuration = 1.0f;
    [SerializeField] private float backgroundBlurTransitionDuration = 2.0f;
    [SerializeField] private float delayBeforeSceneChange = 1.0f; 

    [Header("Eyelid Settings")]
    [SerializeField] private float eyelidOpenDistance = 540f;

    [Header("Scene Change")]
    [SerializeField] private string nextSceneName = "Dialogues"; 

    private Material bedroomMaterial;
    private float currentBlur = 3f;
    private bool isAnimating = false;

    private Vector2 upperEyelidClosedPosition;
    private Vector2 lowerEyelidClosedPosition;

    private void Awake()
    {
        upperEyelidClosedPosition = new Vector2(0f, -67.098f);
        lowerEyelidClosedPosition = new Vector2(0f, 32.90201f);

        upperEyelid.anchoredPosition = upperEyelidClosedPosition;
        lowerEyelid.anchoredPosition = lowerEyelidClosedPosition;
    }

    private void Start()
    {
        bedroomMaterial = bedroomBackground.material;
        bedroomMaterial.SetFloat("_BlurAmount", 1f);

        StartCoroutine(WakeUpSequence());
    }

    private IEnumerator WakeUpSequence()
    {
        if (isAnimating) yield break;
        isAnimating = true;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < numberOfBlinks; i++)
        {
            yield return StartCoroutine(PerformBlink());
            yield return new WaitForSeconds(blinkPauseDuration);
        }

        StartCoroutine(TransitionBlur());
        yield return StartCoroutine(OpenEyesFully());

        yield return new WaitForSeconds(delayBeforeSceneChange);

        LoadNextScene();

        isAnimating = false;
    }

    private IEnumerator PerformBlink()
    {
        float elapsedTime = 0f;
        Vector2 upperStart = upperEyelid.anchoredPosition;
        Vector2 lowerStart = lowerEyelid.anchoredPosition;

        while (elapsedTime < blinkDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (blinkDuration / 2);
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            Vector2 upperPartialOpen = new Vector2(
                upperEyelidClosedPosition.x,
                upperEyelidClosedPosition.y + (eyelidOpenDistance * 0.3f)
            );
            Vector2 lowerPartialOpen = new Vector2(
                lowerEyelidClosedPosition.x,
                lowerEyelidClosedPosition.y - (eyelidOpenDistance * 0.3f)
            );

            upperEyelid.anchoredPosition = Vector2.Lerp(upperStart, upperPartialOpen, t);
            lowerEyelid.anchoredPosition = Vector2.Lerp(lowerStart, lowerPartialOpen, t);

            yield return null;
        }

        elapsedTime = 0f;
        upperStart = upperEyelid.anchoredPosition;
        lowerStart = lowerEyelid.anchoredPosition;

        while (elapsedTime < blinkDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (blinkDuration / 2);
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            upperEyelid.anchoredPosition = Vector2.Lerp(upperStart, upperEyelidClosedPosition, t);
            lowerEyelid.anchoredPosition = Vector2.Lerp(lowerStart, lowerEyelidClosedPosition, t);

            yield return null;
        }
    }

    private IEnumerator OpenEyesFully()
    {
        float elapsedTime = 0f;
        Vector2 upperStart = upperEyelid.anchoredPosition;
        Vector2 lowerStart = lowerEyelid.anchoredPosition;

        Vector2 upperFullyOpen = new Vector2(
            upperEyelidClosedPosition.x,
            upperEyelidClosedPosition.y + eyelidOpenDistance
        );
        Vector2 lowerFullyOpen = new Vector2(
            lowerEyelidClosedPosition.x,
            lowerEyelidClosedPosition.y - eyelidOpenDistance
        );

        while (elapsedTime < finalOpenDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / finalOpenDuration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            upperEyelid.anchoredPosition = Vector2.Lerp(upperStart, upperFullyOpen, t);
            lowerEyelid.anchoredPosition = Vector2.Lerp(lowerStart, lowerFullyOpen, t);

            yield return null;
        }
    }

    private IEnumerator TransitionBlur()
    {
        float elapsedTime = 0f;

        while (elapsedTime < backgroundBlurTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / backgroundBlurTransitionDuration;
            currentBlur = Mathf.Lerp(1f, 0f, t);
            bedroomMaterial.SetFloat("_BlurAmount", currentBlur);
            yield return null;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}