using UnityEngine;

/// <summary>
/// Il "pacchetto" di dati che viaggia insieme all'evento di un colpo.
/// Contiene tutto quello che serve a chi ascolta per reagire.
/// È una struct (tipo a valore) perché è piccola e usa-e-getta.
/// </summary>
public struct DamageInfo
{
    public GameObject Target;    // chi viene colpito
    public GameObject Attacker;  // chi ha colpito (utile per punti, combo, knockback...)
    public int Damage;           // quanto danno

    public DamageInfo(GameObject target, GameObject attacker, int damage)
    {
        Target = target;
        Attacker = attacker;
        Damage = damage;
    }
}
