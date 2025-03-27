using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndSceneTrigger : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private Light2D sceneLight1;
    private Light2D sceneLight2;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        GameObject lightObject = GameObject.Find("UltLightChange");
        GameObject lightObject2 = GameObject.Find("Global Light 2D");
        if (lightObject != null)
        {
            sceneLight1 = lightObject.GetComponent<Light2D>();
        }
        if (lightObject2 != null)
        {
            sceneLight2 = lightObject2.GetComponent<Light2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemySpawner.StartSpawning();
            StartCoroutine(ChangeSceneAfterDelay(10f));
        }
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (sceneLight1 != null && sceneLight2 != null)
        {
            sceneLight1.color = Color.black;
            sceneLight2.color = Color.black;
        }
        SceneManager.LoadScene("VisualNovel2");
    }
}
