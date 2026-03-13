using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
    public float speed = 1.5f;
    public float jumpPower = 10f;

    private float move;
    public LayerMask groundLayer;
    private bool onGround;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform transform;

    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = false;
    }

    void Update()
    {
        move = Input.GetAxis("Horizontal");
        if (move > 0)
        {
            spriteRenderer.flipX = true;
        } else if (move < 0)
        {
            spriteRenderer.flipX = false;
        }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        if (Input.GetButtonDown("Jump") && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            onGround = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (spriteRenderer.flipX && transform.position.x < 8.3)
            {
                transform.position =  new Vector2(transform.position.x + 2.5f, transform.position.y);
            }
            else if (transform.position.x > -8.3)
            {
                transform.position = new Vector2(transform.position.x - 2.5f, transform.position.y);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (groundLayer == (1 << other.gameObject.layer))
        {
            onGround = true;
        }
    }
}