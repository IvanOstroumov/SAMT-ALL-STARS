using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int OnGround = Animator.StringToHash("OnGround");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    public float speed = 1.5f;
    public float jumpPower = 10f;


    private float move;
    public LayerMask groundLayer;
    private bool onGround;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform transform;

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


    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = false;
        animator  = GetComponent<Animator>();
        isStartedAnimation = false;
    }

    void Update()
    {
        //Animazioni
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
        }

        if (isStartedAnimation)
        {
            time += Time.deltaTime;
            if (time > animationTime)
            {
                isStartedAnimation = false;
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

        animator.SetBool(typeAnimation, isStartedAnimation);

        // Movimento, Salto e Dash
        move = Input.GetAxis("Horizontal");
        if (!isDashing)
        {
            if (move > 0)
            {
                spriteRenderer.flipX = false;
            } else if (move < 0)
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            
        }
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
    
}