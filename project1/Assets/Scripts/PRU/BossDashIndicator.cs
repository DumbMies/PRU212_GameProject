using UnityEngine;

public class BossDashIndicator : MonoBehaviour
{
    [Header("Indicator Settings")]
    public float indicatorWidth = 50f;
    private Camera mainCamera;
    private GameObject dashIndicator;
    private float bossHeight;
    private bool isRightSide;
    private float initialBossY;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void ShowDashIndicator(Vector2 dashOrigin, Vector2 dashDirection, float bossSpriteHeight)
    {
        // Remove any existing indicator
        if (dashIndicator != null)
        {
            Destroy(dashIndicator);
        }

        // Store boss height and initial Y position
        bossHeight = bossSpriteHeight;
        initialBossY = dashOrigin.y;

        // Determine which side of the screen the indicator should be on
        isRightSide = dashDirection.x < 0;

        // Create a new indicator GameObject
        dashIndicator = new GameObject("DashIndicator");

        // Add SpriteRenderer
        SpriteRenderer spriteRenderer = dashIndicator.AddComponent<SpriteRenderer>();

        // Create gradient texture
        Texture2D gradientTexture = CreateLinearGradientTexture(isRightSide);
        Sprite sprite = Sprite.Create(
            gradientTexture,
            new Rect(0, 0, gradientTexture.width, gradientTexture.height),
            new Vector2(0.5f, 0.5f)
        );
        spriteRenderer.sprite = sprite;

        // Set indicator scale
        dashIndicator.transform.localScale = new Vector3(
            indicatorWidth,
            bossHeight,
            1
        );

        // Adjust sorting
        spriteRenderer.sortingOrder = 100;
        spriteRenderer.sortingLayerName = "Foreground";
    }

    private Texture2D CreateLinearGradientTexture(bool isRightSideDash)
    {
        // Create a wider texture to show gradient effect
        int width = 128;
        int height = 1;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            // Calculate opacity based on position (linear decrease)
            float normalizedPosition = (float)x / width;

            // Flip the gradient if dashing from the right side
            float opacity = isRightSideDash
                ? normalizedPosition  // Gradient increases from left to right
                : 1f - normalizedPosition;  // Gradient decreases from left to right

            Color gradientColor = new Color(1f, 0f, 0f, opacity);
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, gradientColor);
            }
        }
        texture.Apply();
        return texture;
    }

    void Update()
    {
        if (dashIndicator == null) return;

        // Determine screen edge X based on which side the indicator should be
        float screenEdgeX = isRightSide
            ? mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x
            : mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        // Position the indicator at the camera's edge, maintaining initial Y
        dashIndicator.transform.position = new Vector3(
            screenEdgeX,
            initialBossY,
            0
        );
    }

    public void HideDashIndicator()
    {
        if (dashIndicator != null)
        {
            Destroy(dashIndicator);
        }
    }
}