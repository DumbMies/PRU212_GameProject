using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 0.7f;
    public float destroyAllEnemiesDelay = 3.5f;

    private float spawnTimer;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isSpawning = false;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!isSpawning) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(DestroyAllEnemiesAfterDelay(destroyAllEnemiesDelay));
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        // Choose a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Instantiate the enemy at the chosen spawn point
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemies.Add(enemy);
    }

    private IEnumerator DestroyAllEnemiesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = false;
        DestroyAllEnemies();
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();
    }
}
