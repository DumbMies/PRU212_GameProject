using UnityEngine;
using System.Collections;

public class DemonCircleAnimationTrigger : MonoBehaviour
{
    private Animator anim;
    private GameObject demonCircleParent;

    private void Start()
    {
        // Find the Animator component
        anim = GetComponent<Animator>();
        if (anim == null) // Search in parent or children if not found
        {
            anim = GetComponentInChildren<Animator>();
        }
        if (anim == null)
        {
            Debug.LogError("Animator component not found on DemonCircleAnimationTrigger!");
        }

        // Find the parent DemonCircle GameObject
        demonCircleParent = transform.parent.gameObject;
    }

    private void AppearAnimationDone()
    {
        anim.Play("Idle");
    }

    private void IdleAnimationDone()
    {
        anim.Play("Vanish");
    }

    private void VanishAnimationDone()
    {
        // Destroy the entire DemonCircle GameObject
        if (demonCircleParent != null)
        {
            Destroy(demonCircleParent);
        }
        else
        {
            Debug.LogWarning("Could not find DemonCircle parent to destroy!");
        }
    }
}