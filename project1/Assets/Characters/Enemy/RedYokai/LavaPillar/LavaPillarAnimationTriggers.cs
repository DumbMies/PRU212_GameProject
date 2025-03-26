using UnityEngine;

public class LavaPillarAnimationTrigger : MonoBehaviour
{
    private LavaPillar lavaPillar => GetComponentInParent<LavaPillar>(); 

    private void AnimationTrigger()
    {
        lavaPillar.DestroyPillar();
    }
}
