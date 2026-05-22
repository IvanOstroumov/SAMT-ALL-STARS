using System;
using Resources.Scripts;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
    [Header("Test / Default")]
    [SerializeField] private string defaultCharacter = "ivan";
    
    private Keyboard keyboard;
    private Gamepad gamepad;
    
    private CharacterManager characterManager;
    private Character character;
    private InputType inputType;
    private string playerName;

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

    [Header("Vita")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Hitbox")]
    public Hitbox punchHitbox;
    public Hitbox kickHitbox;

    public void Awake()
    {
        
        
    }

    // OnEnable/OnDisable: qui il player si ISCRIVE e si DISISCRIVE dall'evento.
    // Iscriversi/disiscriversi in coppia evita "iscrizioni fantasma" quando il
    // player viene disattivato o distrutto (es. fine partita, cambio scena).
    private void OnEnable()
    {
        CombatEvents.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        CombatEvents.OnHit -= HandleHit;
    }

    // IL LISTENER: viene chiamato per OGNI colpo del gioco.
    // Controllo "il bersaglio sono io?": solo allora applico il danno.
    private void HandleHit(DamageInfo info)
    {
        if (info.Target != gameObject) return;  // non sono io il bersaglio -> ignoro
        TakeDamage(info.Damage);
    }

    void Start()
    {
        gamepad = new Gamepad();
        gamepad.Gameplay.Dash.performed += ctx => Dash(InputType.Controller);
        gamepad.Gameplay.Jump.performed += ctx => Jump(InputType.Controller);
        gamepad.Gameplay.Kick.performed += ctx => Kick(InputType.Controller);
        gamepad.Gameplay.Punch.performed += ctx => Punch(InputType.Controller);
        gamepad.Gameplay.Move.performed += ctx => Movement(InputType.Controller,ctx.ReadValue<float>());
        gamepad.Gameplay.Move.canceled += ctx => Movement(InputType.Controller,0);
        
        keyboard = new Keyboard();
        keyboard.Gameplay.Dash.performed += ctx => Dash(InputType.Keyboard);
        keyboard.Gameplay.Jump.performed += ctx => Jump(InputType.Keyboard);
        keyboard.Gameplay.Kick.performed += ctx => Kick(InputType.Keyboard);
        keyboard.Gameplay.Punch.performed += ctx => Punch(InputType.Keyboard);
        keyboard.Gameplay.Move.performed += ctx => Movement(InputType.Keyboard,ctx.ReadValue<float>());
        keyboard.Gameplay.Move.canceled += ctx => Movement(InputType.Keyboard,0);
        OnEnableKeyboard();
        OnEnableGamepad();
        
        characterManager = GameManager.Instance.characterManager;
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = false;
        animator = GetComponent<Animator>();
        isStartedAnimation = false;
        currentHealth = maxHealth;

        if (gameObject.name == "Player_Joystick")
        {
            inputType = InputType.Controller;
            playerName = "Player2";
        }
        else 
        {
             inputType = InputType.Keyboard;
            playerName = "Player1";
        }
        string chosen = PlayerPrefs.GetString(playerName, defaultCharacter);
        character = characterManager.getCharByName(chosen);

        if (character == null)
        {
            Debug.LogError($"Personaggio '{chosen}' non trovato per {playerName}. Controlla il nome.");
            return; // evito il NullReferenceException sulle righe sotto
        }

        animator.runtimeAnimatorController = character.Controller;
        spriteRenderer.sprite = character.Sprite;

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

        if (Mathf.Abs(rb.linearVelocityX) > 0.01 && !isStartedAnimation && onGround && !isDashing)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        
        if (!string.IsNullOrEmpty(typeAnimation))
            animator.SetBool(typeAnimation, isStartedAnimation);
    }

    private void Dash(InputType inputTry)
    {
        if (inputTry == inputType && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Punch(InputType inputTry)
    {
        if(inputTry != inputType || isStartedAnimation) return;
        isStartedAnimation = true;
        typeAnimation = "Punch";
        time = 0;
    }
    private void Kick(InputType inputTry)
    {
        Debug.Log("kick");
        if(inputTry != inputType || isStartedAnimation) return;
        isStartedAnimation = true;
        typeAnimation = "Kick";
        time = 0;
    }
    private void Jump(InputType inputTry)
    {
        if (inputTry != inputType || !onGround) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        onGround = false;
        animator.SetBool(OnGround, onGround);
        animator.SetBool(IsJumping, true);
    }

    private void Movement(InputType inputTry, float move)
    {
        if(inputTry != inputType || isDashing) return;
        if (move > 0)
        {
            SetFacing(true);   // guarda a destra
        }
        else if (move < 0)
        {
            SetFacing(false);  // guarda a sinistra
        }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    // Gira l'INTERO player (sprite + hitbox figlie) invertendo il segno di localScale.x.
    // Mathf.Abs preserva la grandezza della scala: cambia solo la direzione, non le dimensioni.
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
            animator.SetBool(OnGround, onGround);
        }
    }
    
    public void OnCollisionExit2D(Collision2D other)
    {
        if ((groundLayer == (1 << other.gameObject.layer)))
        {
            onGround = false;
            animator.SetBool(OnGround, onGround);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetBool(IsDashing, isDashing);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float direction = transform.localScale.x < 0 ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool(IsDashing, isDashing);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{base.gameObject.name} ha {currentHealth} HP");

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log($"{base.gameObject.name} è morto!");
    }
    
    private void OnEnableKeyboard() => keyboard.Gameplay.Enable();
    private void OnDisableKeyboard() => keyboard.Gameplay.Disable();
    private void OnEnableGamepad() => gamepad.Gameplay.Enable();
    private void OnDisableGamepad() => gamepad.Gameplay.Disable();
    public void EnablePunchHitbox()  => punchHitbox.EnableHitbox();
    public void DisablePunchHitbox() => punchHitbox.DisableHitbox();
    public void EnableKickHitbox()   => kickHitbox.EnableHitbox();
    public void DisableKickHitbox()  => kickHitbox.DisableHitbox();
}