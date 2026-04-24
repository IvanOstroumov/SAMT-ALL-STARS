using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    private Collider2D col;
    private PlayerController owner;

    void Start()
    {
        col = GetComponent<Collider2D>();
        owner = GetComponentInParent<PlayerController>();
        col.enabled = false; 
    }

    public void EnableHitbox()  { col.enabled = true; }
    public void DisableHitbox() { col.enabled = false; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == transform.root) return;

        PlayerController target = other.GetComponent<PlayerController>();
        if (target != null)
        {
            target.TakeDamage(damage);
            col.enabled = false; 
        }
    }
}