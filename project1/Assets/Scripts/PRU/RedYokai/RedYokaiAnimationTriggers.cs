using UnityEngine;

public class RedYokaiAnimationTriggers : MonoBehaviour
{
    private RedYokai redYokai => GetComponentInParent<RedYokai>(); 

    private void EndAnimation()
    {
        redYokai.AnimationTrigger();
    }

    private void StartDash()
    {
        redYokai.Dash();
    }

    private void GroundSlam_StartCharging()
    {
        redYokai.groundSlamState.StartCharging();
    }
    private void GroundSlam_SlamDown()
    {
        redYokai.groundSlamState.SlamDown();
    }
}
