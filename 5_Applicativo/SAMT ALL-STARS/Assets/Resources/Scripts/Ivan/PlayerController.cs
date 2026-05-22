using System;
using Resources.Scripts;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
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

   // private float move;
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
        character = characterManager.getCharByName(PlayerPrefs.GetString(playerName));

        GetComponent<Animator>().runtimeAnimatorController = character.Controller;
        GetComponent<SpriteRenderer>().sprite = character.Sprite;

    }

    void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.J) && !isStartedAnimation)
        {
            isStartedAnimation = true;
            typeAnimation = "Punch";
            time = 0;
        }
        if (Input.GetKeyDown(KeyCode.K) && !isStartedAnimation)
        {
            isStartedAnimation = true;
            typeAnimation = "Kick";
            time = 0;
        }*/

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
        
        /*  move = Input.GetAxis("Horizontal");
          if (!isDashing)
          {
              if (move > 0)
              {
                  spriteRenderer.flipX = false;
              }
              else if (move < 0)
              {
                  spriteRenderer.flipX = true;
              }
              rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
          }

        if (Input.GetButtonDown("Jump") && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            onGround = false;
            animator.SetBool(OnGround, onGround);
            animator.SetBool(IsJumping, true);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) )
        {
            StartCoroutine(Dash());
        }*/
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
            spriteRenderer.flipX = false;
        }
        else if (move < 0)
        {
            spriteRenderer.flipX = true;
        }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
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
        float direction = spriteRenderer.flipX ? -1f : 1f;
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