using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    public int velocita;
    private bool isStartedAnimation;
    private float time;
    public float animationTime;
<<<<<<< Updated upstream
    private string animation;
    
=======
    // Start is called once before the first execution of Update after the MonoBehaviour is created
>>>>>>> Stashed changes
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isStartedAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isStartedAnimation)
        {
            isStartedAnimation = true;
            animation = "Punch";
            time = 0;
        }
        if (Input.GetKeyDown(KeyCode.K) && !isStartedAnimation)
        {
            isStartedAnimation = true;
            animation = "Kick";
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
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes

        if (Input.GetKeyDown(KeyCode.D)) 
        {
            transform.localScale = new Vector3(
<<<<<<< Updated upstream
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
=======
                    Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
>>>>>>> Stashed changes
        }
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            transform.localScale = new Vector3(
<<<<<<< Updated upstream
                - Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        
        animator.SetBool(animation, isStartedAnimation);
=======
                    -Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
        }
        
        animator.SetBool("Punch", isStartedAnimation);
 
        
>>>>>>> Stashed changes
    }
}
