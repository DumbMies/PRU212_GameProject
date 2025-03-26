using UnityEngine;
using System.Collections;

public class SpearAnimationTrigger : MonoBehaviour
{
    private float countdown = 10f;
    private Animator anim;
    private GameObject boss2;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (anim == null) // Search in parent or children if not found
        {
            anim = GetComponentInChildren<Animator>();
        }

        if (anim == null)
        {
            Debug.LogError("Animator component not found on SpearAnimationTrigger!");
        }
    }

    private void DoneSpawn()
    {
        anim.speed = 0f;
        //StartCoroutine(StartCountdown());
    }

    //private IEnumerator StartCountdown()
    //{
    //    yield return new WaitForSeconds(countdown);
    //    Destroy(gameObject);
    //}
}