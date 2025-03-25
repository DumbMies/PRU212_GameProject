using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private Vector3 startPosition;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        startPosition = transform.position;
    }

    void Update()
    {
        float distanceToMoveX = (cam.transform.position.x - startPosition.x) * parallaxEffect;
        float distanceToMoveY = (cam.transform.position.y - startPosition.y) * parallaxEffect;

        transform.position = new Vector3(startPosition.x + distanceToMoveX, startPosition.y + distanceToMoveY, transform.position.z);
    }
}
