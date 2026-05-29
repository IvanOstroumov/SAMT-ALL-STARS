using System;
using Resources.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
    [Header("Test / Default")]
    [SerializeField] private string defaultCharacter = "ivan";

    private TextMeshProUGUI hpText;
    private TextMeshProUGUI nameText;

    private Keyboard keyboard;
    private Gamepad gamepad;

    private CharacterManager characterManager;
    private Character character;

    private InputType inputType;
    private string playerName;
    private string opponentName;

    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int OnGround = Animator.StringToHash("OnGround");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");

    public float speed = 1.5f;
    public float jumpPower = 10f;

    public LayerMask groundLayer;
    private bool onGround;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private new Transform transform;

    private float dashPower = 24f;
    private float dashTime = 0.1f;
    private float dashCooldown = 0.5f;
    private bool canDash = true;
    private bool isDashing;

    private bool isStartedAnimation;
    private float time;
    public float animationTime;
    private string typeAnimation;

    private Animator animator;

    [Header("Hitbox")]
    public Hitbox punchHitbox;
    public Hitbox kickHitbox;

    private float lastDamageVoiceTime = -999f;
    private const float DAMAGE_VOICE_COOLDOWN = 2.5f;

    private void OnEnable()
    {
        CombatEvents.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        CombatEvents.OnHit -= HandleHit;
    }

    private void HandleHit(DamageInfo info)
    {
        if (info.Target != gameObject) return;
        TakeDamage(info.Damage);
    }

    void Start()
    {
        gamepad = new Gamepad();
        gamepad.Gameplay.Dash.performed += ctx => Dash(InputType.Controller);
        gamepad.Gameplay.Jump.performed += ctx => Jump(InputType.Controller);
        gamepad.Gameplay.Kick.performed += ctx => Kick(InputType.Controller);
        gamepad.Gameplay.Punch.performed += ctx => Punch(InputType.Controller);
        gamepad.Gameplay.Move.performed += ctx => Movement(InputType.Controller, ctx.ReadValue<float>());
        gamepad.Gameplay.Move.canceled += ctx => Movement(InputType.Controller, 0);

        keyboard = new Keyboard();
        keyboard.Gameplay.Dash.performed += ctx => Dash(InputType.Keyboard);
        keyboard.Gameplay.Jump.performed += ctx => Jump(InputType.Keyboard);
        keyboard.Gameplay.Kick.performed += ctx => Kick(InputType.Keyboard);
        keyboard.Gameplay.Punch.performed += ctx => Punch(InputType.Keyboard);
        keyboard.Gameplay.Move.performed += ctx => Movement(InputType.Keyboard, ctx.ReadValue<float>());
        keyboard.Gameplay.Move.canceled += ctx => Movement(InputType.Keyboard, 0);

        OnEnableKeyboard();
        OnEnableGamepad();

        characterManager = GameManager.Instance.characterManager;

        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        onGround = false;
        isStartedAnimation = false;

        if (gameObject.name == "Player_Joystick")
        {
            inputType = InputType.Controller;
            playerName = "Player2";
            opponentName = "Player1";

            nameText = GameObject.Find("Player_2").GetComponent<TextMeshProUGUI>();
            hpText = GameObject.Find("Hp_Player_2").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            inputType = InputType.Keyboard;
            playerName = "Player1";
            opponentName = "Player2";

            nameText = GameObject.Find("Player_1").GetComponent<TextMeshProUGUI>();
            hpText = GameObject.Find("Hp_Player_1").GetComponent<TextMeshProUGUI>();
        }

        string chosen = PlayerPrefs.GetString(playerName, defaultCharacter);
        character = characterManager.getCharByName(chosen);

        if (character == null)
        {
            LogManager.Error($"Personaggio '{chosen}' non trovato per {playerName}");
            return;
        }

        character.CurrentHp = character.Data.MaxHp;

        animator.runtimeAnimatorController = character.Controller;
        spriteRenderer.sprite = character.Sprite;

        nameText.text = character.Data.Name;
        hpText.text = character.CurrentHp + " HP";

        LogManager.Info($"{playerName} pronto come {character.Data.Name}");
    }

    void Update()
    {
        if (isStartedAnimation)
        {
            time += Time.deltaTime;
            if (time > animationTime)
            {
                isStartedAnimation = false;
                animator.SetBool(typeAnimation, false);
                typeAnimation = "";
            }
        }

        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f && !isStartedAnimation && onGround && !isDashing)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);

        if (!string.IsNullOrEmpty(typeAnimation))
            animator.SetBool(typeAnimation, isStartedAnimation);
    }

    private void Dash(InputType inputTry)
    {
        if (inputTry != inputType || !canDash) return;
        StartCoroutine(Dash());
    }

    private void Punch(InputType inputTry)
    {
        if (inputTry != inputType || isStartedAnimation) return;
        isStartedAnimation = true;
        typeAnimation = "Punch";
        time = 0;
    }

    private void Kick(InputType inputTry)
    {
        if (inputTry != inputType || isStartedAnimation) return;
        isStartedAnimation = true;
        typeAnimation = "Kick";
        time = 0;
    }

    private void Jump(InputType inputTry)
    {
        if (inputTry != inputType || !onGround) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        onGround = false;

        animator.SetBool(OnGround, false);
        animator.SetBool(IsJumping, true);

        AudioManager.Instance.PlaySFX("jump");
    }

    private void Movement(InputType inputTry, float move)
    {
        if (inputTry != inputType || isDashing) return;

        if (move > 0) SetFacing(true);
        else if (move < 0) SetFacing(false);

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    private void SetFacing(bool faceRight)
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (faceRight ? 1f : -1f);
        transform.localScale = s;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (groundLayer == (1 << other.gameObject.layer))
        {
            onGround = true;
            animator.SetBool(IsJumping, false);
            animator.SetBool(OnGround, true);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Empty"))
            Die();
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (groundLayer == (1 << other.gameObject.layer))
        {
            onGround = false;
            animator.SetBool(OnGround, false);
        }
    }
    
    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(8,8 , true);
        canDash = false;
        isDashing = true;
        AudioManager.Instance.PlaySFX("dash");
        animator.SetBool(IsDashing, isDashing);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        

        float direction = transform.localScale.x < 0 ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool(IsDashing, isDashing);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Physics2D.IgnoreLayerCollision(8,8 , false);
    }

    private void TakeDamage(int damage)
    {
        character.CurrentHp -= damage;

        if (character.CurrentHp <= 0) Die();
        hpText.text = character.CurrentHp + "Hp";

        if (Time.time - lastDamageVoiceTime >= DAMAGE_VOICE_COOLDOWN)
        {
            lastDamageVoiceTime = Time.time;
            AudioManager.Instance.PlayVoice(character.Data.Name, VoiceLine.Damage);
        }
    }

    private void Die()
    {
        PlayerPrefs.SetString("Loser", character.Data.Name);
        PlayerPrefs.SetString("Winner", PlayerPrefs.GetString(opponentName, defaultCharacter));
        
        LogManager.Info($"{character.Data.Name} ({playerName}) e' KO");
        AudioManager.Instance.PlaySFX("win");

        // La voiceline di vittoria la dice L'AVVERSARIO, non chi muore.
        PlayerController[] all = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController p in all)
        {
            if (p == this) continue;
            if (p.character == null) continue;
            AudioManager.Instance.PlayVoice(p.character.Data.Name, VoiceLine.Victory);
            break;
        }
        gamepad.Disable();
        keyboard.Disable();
        UIManager.openPostMatch();
    }

    private void OnEnableKeyboard() => keyboard.Gameplay.Enable();
    private void OnDisableKeyboard() => keyboard.Gameplay.Disable();
    private void OnEnableGamepad() => gamepad.Gameplay.Enable();
    private void OnDisableGamepad() => gamepad.Gameplay.Disable();

    public void EnablePunchHitbox() => punchHitbox.EnableHitbox();
    public void DisablePunchHitbox() => punchHitbox.DisableHitbox();
    public void EnableKickHitbox() => kickHitbox.EnableHitbox();
    public void DisableKickHitbox() => kickHitbox.DisableHitbox();
}