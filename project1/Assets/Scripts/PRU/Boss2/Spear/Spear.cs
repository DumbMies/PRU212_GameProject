using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour
{
    private Animator anim;
    public int spearDamage = 1;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.Play("Appear");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Marisa marisa = collision.GetComponent<Marisa>();
            marisa.TakeDamage(1);
        }
    }
}