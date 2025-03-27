using UnityEngine;

public class ThunderYoukaiAnimationTrigger : MonoBehaviour
{
    private ThunderYoukai thunderYoukai => GetComponentInParent<ThunderYoukai>();

    private void AnimationTrigger()
    {
        thunderYoukai.AnimationFinishTrigger();
    }
}
