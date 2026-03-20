using UnityEngine;

public class Movimento : MonoBehaviour
{
    public float velocita = 5f;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");

        // Muove il personaggio
        rb.linearVelocity = new Vector2(inputX * velocita, rb.linearVelocity.y);

        // Gira il personaggio in base alla direzione
        if (inputX > 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }

    }
}