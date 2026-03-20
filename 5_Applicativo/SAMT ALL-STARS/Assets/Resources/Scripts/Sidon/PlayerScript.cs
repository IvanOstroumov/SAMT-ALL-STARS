using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    public int velocita;
    private bool isStartedAnimation;
    private float time;
    public float animationTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isStartedAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            isStartedAnimation = true;
            time = 0;
        }
        if (isStartedAnimation) 
        {
            time += Time.deltaTime;
            if (time > animationTime) 
            {
                isStartedAnimation=false;
            }
        }
        animator.SetBool("Punch", isStartedAnimation);
    }
}
