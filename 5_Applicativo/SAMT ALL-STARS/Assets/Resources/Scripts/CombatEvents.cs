using System;
using UnityEngine;

// Canale globale dei colpi del gioco.
// La Hitbox chiama RaiseHit() quando un attacco va a segno
public static class CombatEvents
{
    public static event Action<DamageInfo> OnHit;

    public static void RaiseHit(DamageInfo info)
    {
        OnHit?.Invoke(info);
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        OnHit = null;
    }
}
