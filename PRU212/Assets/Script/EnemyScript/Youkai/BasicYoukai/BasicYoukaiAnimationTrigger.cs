using UnityEngine;

public class BasicYoukaiAnimationTrigger : MonoBehaviour
{
    private BasicYoukai basicYoukai => GetComponentInParent<BasicYoukai>();

    private void AnimationTrigger()
    {
        basicYoukai.AnimationFinishTrigger();
    }
}
