
using UnityEngine;
using System.Collections;

public class BombDropZone : MonoBehaviour
{
    [Header("Bomb Drop Settings")]
    public GameObject bombPrefab;
    public float bombDropInterval = 0.2f;
    public int numberOfBombs = 5;

    [Header("Zone Settings")]
    public float zoneWidth = 50f;
    public float zoneHeight = 100f; // Large vertical height to ensure boss interaction

    [Header("Debug Settings")]
    public bool enableDebugVisualization = false;
    public Color debugZoneColor = new Color(1f, 0f, 0f, 0.2f);

    private bool isDropping = false;
    private BoxCollider2D zoneCollider;
    private SpriteRenderer debugRenderer;

    void Start()
    {
        zoneCollider = GetComponent<BoxCollider2D>();
        if (zoneCollider == null)
        {
            zoneCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        zoneCollider.isTrigger = true;
        zoneCollider.size = new Vector2(zoneWidth, zoneHeight);

        SetupDebugVisualization();
    }

    void Update()
    {
        UpdateDebugVisualization();
    }

    private void SetupDebugVisualization()
    {
        if (!enableDebugVisualization) return;

        // Create a debug renderer if it doesn't exist
        debugRenderer = GetComponent<SpriteRenderer>();
        if (debugRenderer == null)
        {
            debugRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Create a white pixel texture for visualization
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        debugRenderer.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        // Initially hide the debug renderer
        debugRenderer.enabled = false;
    }

    private void UpdateDebugVisualization()
    {
        if (!enableDebugVisualization || debugRenderer == null) return;

        // Toggle debug zone visibility
        debugRenderer.enabled = true;

        // Set color with transparency
        debugRenderer.color = debugZoneColor;

        // Match the debug renderer's size to the collider
        transform.localScale = new Vector3(
            zoneCollider.size.x,
            zoneCollider.size.y,
            1
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Boss"
        if (other.CompareTag("Boss"))
        {
            // Retrieve the Boss2 component
            Boss2 boss2 = other.GetComponent<Boss2>();

            // Check if boss is in dash state
            if (boss2 != null && boss2.stateMachine.currentState == boss2.dashState)
            {
                // Start bomb drop sequence
                StartBombDrop();
            }
        }
    }

    public void StartBombDrop()
    {
        // Prevent multiple bomb drop sequences
        if (isDropping) return;

        // Start coroutine to drop bombs
        StartCoroutine(DropBombsSequence());
    }

    private IEnumerator DropBombsSequence()
    {
        isDropping = true;

        for (int i = 0; i < numberOfBombs; i++)
        {
            DropSingleBomb();
            yield return new WaitForSeconds(bombDropInterval);
        }

        isDropping = false;
    }

    private void DropSingleBomb()
    {
        // Calculate a random x position within the drop zone
        float randomX = Random.Range(
            transform.position.x - zoneWidth / 2,
            transform.position.x + zoneWidth / 2
        );

        // Instantiate the bomb
        GameObject bomb = Instantiate(bombPrefab,
            new Vector3(randomX, transform.position.y, 0),
            Quaternion.identity);

        // Get the Bomb component and trigger drop
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.Drop();
        }
    }

    // Optional method to manually position the zone relative to the player
    public void PositionZone(Vector2 playerPosition)
    {
        transform.position = new Vector3(
            playerPosition.x,
            playerPosition.y,
            0
        );
    }
}