using UnityEngine;

public class MarisaAnimationTrigger : MonoBehaviour
{
    private Marisa player => GetComponentInParent<Marisa>(); 

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}
