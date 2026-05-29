using UnityEngine;

// Script di prova per il movimento orizzontale + flip dello sprite.
// Versione semplificata che usavo nel sandbox: il movimento vero del gioco
// e' dentro PlayerController.
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

        rb.linearVelocity = new Vector2(inputX * velocita, rb.linearVelocity.y);

        // Flip: scala X positiva = guarda a destra, negativa = guarda a sinistra.
        // Mathf.Abs preserva la grandezza, cambia solo il segno.
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
