using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
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
    
    private Animator animator;


    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = false;
        animator  = GetComponent<Animator>();
    }

    void Update()
    {
        move = Input.GetAxis("Horizontal");
        if (!isDashing)
        {
            if (move > 0)
            {
                spriteRenderer.flipX = true;
            } else if (move < 0)
            {
                spriteRenderer.flipX = false;
            }
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }
        if (Input.GetButtonDown("Jump") && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            onGround = false;
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
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float direction = spriteRenderer.flipX ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
}