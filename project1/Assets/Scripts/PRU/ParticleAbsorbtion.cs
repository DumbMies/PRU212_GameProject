using UnityEngine;

public class ParticleAbsorptionEffect : MonoBehaviour
{
    [Header("Absorption Settings")]
    public Transform absorptionCenter;
    public float maxAbsorptionRadius = 5f;
    public float absorptionSpeed = 5f;
    public AnimationCurve absorptionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public GameObject particlePrefab;
    public int particleCount = 20;
    public float spawnRadius = 3f;

    void Start()
    {
        // Validate key references
        if (absorptionCenter == null)
        {
            Debug.LogError("Absorption Center is not set! Please assign a target in the inspector.");
            enabled = false;
            return;
        }

        if (particlePrefab == null)
        {
            Debug.LogError("Particle Prefab is not set! Please assign a prefab in the inspector.");
            enabled = false;
            return;
        }

        SpawnInitialParticles();
    }

    void SpawnInitialParticles()
    {
        for (int i = 0; i < particleCount; i++)
        {
            // Random position around the spawn area
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

            // Instantiate particle
            GameObject particle = Instantiate(particlePrefab, randomPos, Quaternion.identity);
            particle.tag = "Particle"; // Ensure tag is set
            particle.transform.SetParent(transform, true);

            Debug.Log($"Spawned particle at {randomPos}");
        }
    }

    void Update()
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");

        if (particles.Length == 0)
        {
            Debug.LogWarning("No particles found with 'Particle' tag!");
        }

        foreach (GameObject particleObj in particles)
        {
            // Calculate distance to absorption center
            float distance = Vector2.Distance(particleObj.transform.position, absorptionCenter.position);

            // Check if particle is within absorption radius
            if (distance <= maxAbsorptionRadius)
            {
                // Calculate absorption intensity based on distance and curve
                float t = 1f - (distance / maxAbsorptionRadius);
                float absorptionIntensity = absorptionCurve.Evaluate(t);

                // Move particle towards absorption center
                particleObj.transform.position = Vector2.MoveTowards(
                    particleObj.transform.position,
                    absorptionCenter.position,
                    absorptionSpeed * absorptionIntensity * Time.deltaTime
                );

                // Destroy particle when very close to center
                if (distance < 0.1f)
                {
                    Destroy(particleObj);
                }
            }
        }
    }

    // Visualize spawn and absorption areas
    void OnDrawGizmosSelected()
    {
        if (absorptionCenter != null)
        {
            // Spawn radius gizmo
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);

            // Absorption radius gizmo
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(absorptionCenter.position, maxAbsorptionRadius);
        }
    }
}