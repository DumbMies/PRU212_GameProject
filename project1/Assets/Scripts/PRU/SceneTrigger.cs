using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class SceneTrigger : MonoBehaviour
{
    public GameObject backgroundScene1;
    public GameObject backgroundScene2;
    private bool hasExited = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OUDWOIJSOIW");
        Marisa marisa = col.GetComponent<Marisa>();
        if (marisa != null)
        {
            Debug.Log("Memaybeo");
        }
    }

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
            globalLight.color = HexToColor("#FF3E3E");

            Debug.Log("Player has exited the trigger.");
            backgroundScene1.SetActive(false);
            backgroundScene2.SetActive(true);
            
            Debug.Log("WallPos = " + gameObject.transform.position.x);
            Debug.Log("PlayerPos = " + other.transform.position.x);
            if (gameObject.transform.position.x < other.transform.position.x)
            {
                hasExited = true;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
            }

        }


    }
}
