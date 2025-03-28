using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Boss2AnimationTrigger : MonoBehaviour
{
    private Boss2 boss2 => GetComponentInParent<Boss2>();
    public float spearSpeed = 200;
    public float maxSpearDistance = 50f;

    public float firstSpreadAngle = 40f;
    public float secondSpreadAngle = 20f;

    public float launchPower = 50f;

    private void DoneAnimation()
    {
        boss2.AnimationFinishTrigger();
    }
    #region ATTACK 1
    private void SummonSpear()
    {
        StartCoroutine(SpearThrowSequence());
    }

    private IEnumerator SpearThrowSequence()
    {
        ThrowSpearVolley(firstSpreadAngle);

        yield return new WaitForSeconds(0.5f);

        ThrowSpearVolley(secondSpreadAngle);
    }

    private void ThrowSpearVolley(float spreadAngle)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found! Cannot throw spears.");
            return;
        }

        Vector3 targetPosition = new Vector2 (player.transform.position.x, player.transform.position.y -1f);
        Vector3 directionToPlayer = (targetPosition - boss2.transform.position).normalized;

        for (int i = 0; i < 3; i++)
        {
            float offsetAngle = 0f;
            switch (i)
            {
                case 0: // Left spear
                    offsetAngle = -spreadAngle / 2;
                    break;
                case 1: // Center spear
                    offsetAngle = 0f;
                    break;
                case 2: // Right spear
                    offsetAngle = spreadAngle / 2;
                    break;
            }

            Vector3 spearDirection = Quaternion.Euler(0, 0, offsetAngle) * directionToPlayer;

            Vector3 spawnPosition = boss2.transform.position + new Vector3(0, 2f, 0);

            GameObject spearClone = Instantiate(boss2.Spear, spawnPosition, Quaternion.identity);
            spearClone.GetComponent<Spear>().enabled = true;

            float angle = Mathf.Atan2(spearDirection.y, spearDirection.x) * Mathf.Rad2Deg;
            spearClone.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            Collider2D spearCollider = spearClone.GetComponent<Collider2D>();
            if (spearCollider == null)
            {
                spearCollider = spearClone.AddComponent<BoxCollider2D>();
            }
            spearCollider.isTrigger = true;

            Rigidbody2D rb = spearClone.GetComponent<Rigidbody2D>();
            rb.linearVelocity = spearDirection * spearSpeed;

            Destroy(spearClone, maxSpearDistance / spearSpeed);
        }
    }
    #endregion

    private void Launch()
    {
        boss2.rb.linearVelocity = new Vector2(boss2.facingDirection * -1 * launchPower, 0);
    }

    private void SummonDemonCircle()
    {
        Vector3 spawnPosition = boss2.transform.position;

        GameObject DemonCircleClone = Instantiate(boss2.DemonCircle, spawnPosition, Quaternion.identity);
        DemonCircleClone.GetComponent<DemonCircle>().enabled = true;
        DeleteObject(DemonCircleClone);
    }

    async void DeleteObject(GameObject objectToDelete)
    {
        await Task.Delay(1500);
        Destroy(objectToDelete);
    }

    //private void WallLaunch()
    //{
    //    Collider2D collider = new Collider2D();
    //    if (collider.gameObject.layer == LayerMask.NameToLayer("Player") != null) {
    //        Marisa marisa1 = collider.GetComponent<Marisa>();
    //        {
    //            marisa1.TakeDamage(1);
    //        }
    //    }
    //    boss2.stateMachine.ChangeState(boss2.floatState);
    //}
}