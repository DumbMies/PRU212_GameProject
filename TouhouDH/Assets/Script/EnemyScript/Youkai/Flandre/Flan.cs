using Pathfinding;
using System.Collections;
using UnityEngine;

public class Flan : MonoBehaviour
{

    public GameObject lightningWarn;
    public GameObject lightning;
    public GameObject lightningExplosion;
    public Marisa player;

    public float speed = 3f;
    public float waitTime = 1f;
    public Transform pointA;
    public Transform pointB;

    private Vector3 target;
    private Animator animator;
    private bool isMoving = true;

    private float lightningCooldown = 0.5f;
    private float lastLightningTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = pointB.position;
        animator.SetBool("Move", true);
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
            if(Time.time - lastLightningTime >= lightningCooldown)
            {
                TriggerLightningStrike();
                lastLightningTime = Time.time;
            }
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(WaitAndSwitch());
        }
    }

    IEnumerator WaitAndSwitch()
    {
        isMoving = false;
        animator.SetBool("Move", false);
        animator.SetBool("Idle", true);

        yield return new WaitForSeconds(waitTime);

        target = target == pointA.position ? pointB.position : pointA.position;
        Flip();

        animator.SetBool("Idle", false);
        animator.SetBool("Move", true);
        isMoving = true;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TriggerLightningStrike()
    {
        if (player != null)
        {
            Vector3 groundPosition = new Vector3(player.transform.position.x, GetGroundYPosition(player.transform.position), player.transform.position.z);
            GameObject warning = Instantiate(lightningWarn, groundPosition, Quaternion.identity);
            StartCoroutine(StrikeLightningAfterDelay(groundPosition, warning));
        }
    }

    private float GetGroundYPosition(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            return hit.point.y;
        }
        return position.y;
    }

    private IEnumerator StrikeLightningAfterDelay(Vector3 position, GameObject warning)
    {
        yield return new WaitForSeconds(1f); // Delay before the lightning strike
        Destroy(warning);
        GameObject lightningInstance = Instantiate(lightning, position, Quaternion.identity);
        yield return new WaitForSeconds(0.06f);
        GameObject lightningExplosionInstance = Instantiate(lightningExplosion, position, Quaternion.identity);
        yield return new WaitForSeconds(0.25f); // Delay before destroying the objects
        Destroy(lightningInstance);
        Destroy(lightningExplosionInstance);
    }
}
