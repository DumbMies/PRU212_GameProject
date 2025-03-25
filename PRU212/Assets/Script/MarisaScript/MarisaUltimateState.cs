using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MarisaUltimateState : MarisaState
{
    private float _fireDelay = 1f;
    private float _ultimateCooldown = 30f;
    private bool _isUltimateOnCooldown = false;
    private Light2D ultLightChange;
    private Light2D globalLight;

    public MarisaUltimateState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!_isUltimateOnCooldown)
        {
            player.audioManager.PlaySFX(player.audioManager.ultimateSound);
            player.StartCoroutine(UltDelay());
            player.rb.linearVelocity = Vector2.zero;
            player.rb.bodyType = RigidbodyType2D.Static;
            ShowUltimatePopup();

            ultLightChange = GameObject.Find("UltLightChange").GetComponent<Light2D>();
            globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();

            if (ultLightChange != null)
            {
                ultLightChange.intensity = 0.2f;
            }

            if (player.ultimateReadyNotification != null)
            {
                player.ultimateReadyNotification.enabled = false;
            }
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        HideUltimatePopup();

        // Revert the light intensity
        if (ultLightChange != null)
        {
            ultLightChange.intensity = 1f;
        }
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void Shoot()
    {
        if (player.ultimateAttackPrefab != null && player.ultimateAttackPoint != null)
        {
            Vector2 direction = player.isFacingRight ? Vector2.right : Vector2.left;
            GameObject bullet = GameObject.Instantiate(player.ultimateAttackPrefab, player.ultimateAttackPoint.position, Quaternion.identity);
            bullet.GetComponent<MarisaUltInfo>().Initialize(direction);

            // Ensure the bullet is facing the correct direction
            if (!player.isFacingRight)
            {
                bullet.transform.localScale = new Vector3(-bullet.transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);
            }

            if (player.ultimateAttackEffectPrefab != null)
            {
                GameObject effect = GameObject.Instantiate(player.ultimateAttackEffectPrefab, player.longAttackPoint.position, Quaternion.identity);
            }
        }
        StartUltimateCooldown();
    }

    private IEnumerator UltDelay()
    {
        yield return new WaitForSeconds(_fireDelay);
        Shoot();
    }

    private void StartUltimateCooldown()
    {
        _isUltimateOnCooldown = true;
        player.StartCoroutine(UltimateCooldownRoutine());
    }

    private IEnumerator UltimateCooldownRoutine()
    {
        yield return new WaitForSeconds(_ultimateCooldown);
        _isUltimateOnCooldown = false;

        // Show the ultimate ready notification
        if (player.ultimateReadyNotification != null)
        {
            player.ultimateReadyNotification.enabled = true;
        }
    }

    private void ShowUltimatePopup()
    {
        if (player.ultimatePopupCanvasGroup != null)
        {
            player.ultimatePopupCanvasGroup.gameObject.SetActive(true);
            player.StartCoroutine(FadeOutPopupAfterDelay(0.5f));
        }
    }

    private void HideUltimatePopup()
    {
        if (player.ultimatePopupCanvasGroup != null)
        {
            player.ultimatePopupCanvasGroup.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeOutPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return FadeCanvasGroup(player.ultimatePopupCanvasGroup, 1f, 0f, 1.5f);
        HideUltimatePopup();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    public override bool CanTakeDamage()
    {
        return false;
    }
}
