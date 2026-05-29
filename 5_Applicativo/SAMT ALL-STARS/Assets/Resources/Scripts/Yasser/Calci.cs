using UnityEngine;

// Script di test per le animazioni dei calci. Lo lanciavo nella scena di prova
// per vedere se i trigger dell'Animator partivano. Nel gameplay vero non e' usato:
// il combat sta dentro PlayerController.
public class Calci : MonoBehaviour
{
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Calcio in avanti (a terra).
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("CalcioAvanti");
        }

        // Calcio verso il basso (in aria).
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("CalcioGiu");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGrounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("isGrounded", false);
        }
    }
}
