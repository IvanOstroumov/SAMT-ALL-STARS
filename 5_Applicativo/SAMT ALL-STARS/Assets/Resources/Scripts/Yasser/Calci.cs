using UnityEngine;

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
        // CALCIO IN AVANTI — K a terra
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("CalcioAvanti");
        }

        // CALCIO IN GIÙ — K in aria
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