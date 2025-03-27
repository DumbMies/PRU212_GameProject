using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Marisa : Entity
{
    public bool isBusy { get; private set; }
    [SerializeField] public TextMeshProUGUI tutorial;
    [SerializeField] public TextMeshProUGUI CoolDown;

    [Header("Attack info")]
    public Vector2[] attackMovement;
    public Transform longAttackPoint;
    public Transform ultimateAttackPoint;

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float DashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }

    [Header("Bullet info")]
    public GameObject longAttackPrefab;
    public GameObject ultimateAttackPrefab;
    public GameObject ultimateAttackEffectPrefab;

    [Header("UI info")]
    public Image ultimatePopup;
    public CanvasGroup ultimatePopupCanvasGroup;
    public Image ultimateReadyNotification;

    [Header("Character stats")]
    public int maxHealth = 3;
    public int currentHealth;
    public MarisaHealth healthDisplay;

    public AudioManager2 audioManager;

    private bool isTriggered = false;

    #region States
    public MarisaStateMachine stateMachine { get; private set; }
    public MarisaIdleState idleState { get; private set; }
    public MarisaMoveState moveState { get; private set; }
    public MarisaAirState airState { get; private set; }
    public MarisaJumpState jumpState { get; private set; }
    public MarisaDashState dashState { get; private set; }
    public MarisaPrimaryAttackState primaryAttack { get; private set; }
    public MarisaLongAttackState longAttackState { get; private set; }
    public MarisaUltimateState ultimateState { get; private set; }
    public MarisaHurtState hurtState { get; private set; }
    public MarisaDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new MarisaStateMachine();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager2>();

        idleState = new MarisaIdleState(stateMachine, this, "Idle");
        moveState = new MarisaMoveState(stateMachine, this, "Move");
        airState = new MarisaAirState(stateMachine, this, "Jump");
        jumpState = new MarisaJumpState(stateMachine, this, "Jump");
        primaryAttack = new MarisaPrimaryAttackState(stateMachine, this, "PrimaryAttack");
        longAttackState = new MarisaLongAttackState(stateMachine, this, "LongAttack");
        hurtState = new MarisaHurtState(stateMachine, this, "Hurt");
        dashState = new MarisaDashState(stateMachine, this, "Dash");
        ultimateState = new MarisaUltimateState(stateMachine, this, "Ultimate");
        deathState = new MarisaDeathState(stateMachine, this, "Death");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        // Hide the ultimate popup at the start of the game
        if (ultimatePopupCanvasGroup != null)
        {
            ultimatePopupCanvasGroup.alpha = 0f;
            ultimatePopupCanvasGroup.gameObject.SetActive(false);
        }

        string currentScene = SceneManager.GetActiveScene().name;


        if (currentScene == "Scene1")
        {
            ShowText("Use WASD to move around\r\nFirst let move to the right following the signboard");
        }
        else if (currentScene == "Scene4")
        {
            StartCoroutine(CooldownTimer());
        }
    }

    private IEnumerator CooldownTimer()
    {
        float cooldown = 76f;

        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            CoolDown.text = Mathf.CeilToInt(cooldown).ToString();
            yield return null;
        }
        CoolDown.text = "0";
        UnityEngine.SceneManagement.SceneManager.LoadScene("VisualNovel6");
        Debug.Log("Fight ended!");
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        if(IsTrapDetected())
        {
            TakeDamage(99);
        }

        CheckForAttackInput();
        CheckForDashInput();
        CheckForLongAttackInput();
        CheckForUltimateInput();

        if (isTriggered)
        {
            NotShowText("zzz is game gooner >.<");
            moveSpeed = 1f;
            jumpForce = 2.4f;
        }
    }

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void TakeDamage(int damage)
    {
        if (stateMachine.currentState.CanTakeDamage())
        {
            currentHealth -= damage;
            if (currentHealth > 0)
            {
                stateMachine.ChangeState(hurtState);
            }
            else
            {
                stateMachine.ChangeState(deathState);
            }
            healthDisplay.TakeDamage(damage);
        }
    }

    #region text

    public void ShowText(string text)
    {
        tutorial.text = text;
        StartCoroutine(FadeInAndOutTutorial());
    }

    private IEnumerator FadeInAndOutTutorial()
    {
        float fadeDuration = 1.3f;
        float displayDuration = 3f;
        float elapsedTime = 0f;

        tutorial.gameObject.SetActive(true);

        // Fade in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tutorial.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Display for a duration
        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tutorial.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        tutorial.gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Warning5")
        {
            isTriggered = true;
        }
    }

    public void NotShowText(string text)
    {
        tutorial.text = text;
        tutorial.gameObject.SetActive(true);
        StartCoroutine(NotFadeInAndOutTutorial());
    }

    private IEnumerator NotFadeInAndOutTutorial()
    {
        float fadeDuration = 1f;
        float displayDuration = 2f;
        float elapsedTime = 0f;

        // Fade in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tutorial.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Display for a duration
        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tutorial.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        tutorial.gameObject.SetActive(false);
    }
    #endregion

    #region Skill Input Function
    private void CheckForAttackInput()
    {
        if (stateMachine.currentState is MarisaUltimateState || stateMachine.currentState is MarisaDeathState || stateMachine.currentState is MarisaDashState || stateMachine.currentState is MarisaPrimaryAttackState)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            stateMachine.ChangeState(primaryAttack);
        }
    }

    private void CheckForDashInput()
    {
        if (stateMachine.currentState is MarisaUltimateState || stateMachine.currentState is MarisaDeathState)
        {
            return;
        }

        DashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.L) && DashUsageTimer < 0)
        {
            DashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    private void CheckForLongAttackInput()
    {
        if (stateMachine.currentState is MarisaUltimateState || stateMachine.currentState is MarisaDeathState || stateMachine.currentState is MarisaDashState || stateMachine.currentState is MarisaLongAttackState)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            stateMachine.ChangeState(longAttackState);
        }
    }

    private void CheckForUltimateInput()
    {
        if (stateMachine.currentState is MarisaUltimateState || stateMachine.currentState is MarisaDeathState || stateMachine.currentState is MarisaDashState)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(ultimateState);
        }
    }
    #endregion

    #region Save/Load Function

    public void Save(ref PlayerSaveData data)
    {
        data.Position = transform.position;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
    }

    #endregion
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
}
