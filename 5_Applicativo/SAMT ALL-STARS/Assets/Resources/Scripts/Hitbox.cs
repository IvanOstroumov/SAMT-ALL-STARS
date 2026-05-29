using System.Collections.Generic;
using UnityEngine;

// Una hitbox di attacco (pugno o calcio). Sta su un GameObject figlio del player,
// con un Collider2D in trigger. Quando il collider entra in un altro player,
// annuncia il colpo sul canale CombatEvents: poi il PlayerController bersaglio
// si prendera' il danno e l'AudioManager riprodurra' il suono d'impatto.
public class Hitbox : MonoBehaviour
{
    public int damage = 10;


    public string hitSfx = "punch";

    private Collider2D col;
    private GameObject owner;   


    private readonly HashSet<GameObject> alreadyHit = new HashSet<GameObject>();


    void Awake()
    {
        col = GetComponent<Collider2D>();
        owner = transform.root.gameObject;
        col.enabled = false;
    }


    public void EnableHitbox()
    {
        alreadyHit.Clear();
        col.enabled = true;
    }


    public void DisableHitbox()
    {
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.transform.root == transform.root) return;

        PlayerController target = other.GetComponentInParent<PlayerController>();
        if (target == null) return;

        if (alreadyHit.Contains(target.gameObject)) return;
        alreadyHit.Add(target.gameObject);


        CombatEvents.RaiseHit(new DamageInfo(target.gameObject, owner, damage, hitSfx));
    }
}
