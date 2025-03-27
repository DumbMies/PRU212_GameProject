using UnityEngine;

public class WingedYoukaiAnimationTrigger : MonoBehaviour
{
    private WingedYoukaiAI wingedYoukaiAI => GetComponentInParent<WingedYoukaiAI>();

    private void AnimationTrigger()
    {
        wingedYoukaiAI.AnimationFinishTrigger();
    }
}
