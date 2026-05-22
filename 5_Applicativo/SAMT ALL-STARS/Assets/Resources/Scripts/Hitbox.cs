using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    // Nome del suono da far partire quando questa hitbox segna un colpo.
    // Impostalo nell'Inspector: "punch" sulla hitbox del pugno, "kick" su quella del calcio.
    public string hitSfx = "punch";

    private Collider2D col;
    private GameObject owner;   // il player che possiede questa hitbox = chi colpisce

    // I bersagli già colpiti DURANTE l'attacco corrente.
    // Si azzera a ogni nuovo attacco (in EnableHitbox), così ogni colpo
    // fa danno una sola volta per bersaglio.
    private readonly HashSet<GameObject> alreadyHit = new HashSet<GameObject>();

    // Awake invece di Start: gira prima, così il collider è già spento
    // prima del primo frame fisico (niente colpi accidentali al via).
    void Awake()
    {
        col = GetComponent<Collider2D>();
        owner = transform.root.gameObject;  // la root della gerarchia è il player
        col.enabled = false;
    }

    // Chiamato dall'Animation Event quando inizia il colpo
    public void EnableHitbox()
    {
        alreadyHit.Clear();   // nuovo attacco -> resetto chi ho già colpito
        col.enabled = true;
    }

    // Chiamato dall'Animation Event quando il colpo finisce
    public void DisableHitbox()
    {
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignoro me stesso: il mio collider e il mio corpo hanno la stessa root.
        if (other.transform.root == transform.root) return;

        // GetComponentInParent: trova il PlayerController anche se il collider
        // colpito sta su un figlio (es. un collider del corpo sotto il player).
        PlayerController target = other.GetComponentInParent<PlayerController>();
        if (target == null) return;

        // Già colpito in questo attacco? Esco. Altrimenti lo segno.
        if (alreadyHit.Contains(target.gameObject)) return;
        alreadyHit.Add(target.gameObject);

        // NON chiamo più target.TakeDamage(...) direttamente.
        // Annuncio l'evento: chi è interessato reagirà (il player col danno, l'AudioManager col suono).
        CombatEvents.RaiseHit(new DamageInfo(target.gameObject, owner, damage, hitSfx));
    }
}