using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    public int velocita;
    private bool isStartedAnimation;
    private float time;
    public float animationTime;
    private string animation;
    
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


        if (Input.GetKeyDown(KeyCode.D)) 
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            transform.localScale = new Vector3(
                - Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        
        animator.SetBool(animation, isStartedAnimation);
    }
}
