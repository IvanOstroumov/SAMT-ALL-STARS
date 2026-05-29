using UnityEngine;

// Pacchetto che viaggia con ogni colpo del gioco.
// Lo crea la Hitbox quando va a segno e lo passa a CombatEvents.RaiseHit().
public struct DamageInfo
{
    public GameObject Target;    
    public GameObject Attacker;  
    public int Damage;
    public string HitSfx;       
    
    public DamageInfo(GameObject target, GameObject attacker, int damage, string hitSfx = null)
    {
        Target = target;
        Attacker = attacker;
        Damage = damage;
        HitSfx = hitSfx;
    }
}
