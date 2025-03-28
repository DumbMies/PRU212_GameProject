using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class BossTrigger : MonoBehaviour
{
    private bool hasExited = false;
    private GameObject boss2;

    private void Start()
    {
        boss2 = GameObject.Find("Boss");
    }
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    Marisa marisa = col.GetComponent<Marisa>();
    //    if (marisa != null)
    //    {
    //    }
    //}

    private Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        return Color.white;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (hasExited) return;

        if (other.CompareTag("Player"))
        {
            Light2D globalLight;
            globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
            globalLight.color = HexToColor("#0dadde");
        }

        boss2.GetComponent<Boss2>().enabled = true;
    }
}
