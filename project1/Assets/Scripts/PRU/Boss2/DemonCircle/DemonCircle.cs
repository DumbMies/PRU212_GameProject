using UnityEngine;
using System.Collections;

public class DemonCircle : MonoBehaviour
{
    private Animator anim;
    private bool appeared = false;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (appeared == false && anim != null)
        {
            appeared = true;
            anim.Play("Appear");
        }
    }
}