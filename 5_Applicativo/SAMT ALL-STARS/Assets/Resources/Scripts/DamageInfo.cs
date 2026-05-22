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
    public string HitSfx;        // nome del suono da riprodurre quando il colpo va a segno ("punch"/"kick")

    // hitSfx ha un valore di default (null), così il vecchio codice che non lo passa continua a compilare.
    public DamageInfo(GameObject target, GameObject attacker, int damage, string hitSfx = null)
    {
        Target = target;
        Attacker = attacker;
        Damage = damage;
        HitSfx = hitSfx;
    }
}
