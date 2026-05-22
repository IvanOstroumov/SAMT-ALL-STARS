using System;
using UnityEngine;

/// <summary>
/// Il "canale" su cui passano tutti i colpi del gioco.
/// La Hitbox annuncia un colpo chiamando RaiseHit(...).
/// Chiunque sia interessato (i player, la UI della vita, i suoni...)
/// si iscrive a OnHit e reagisce.
/// È statica: esiste una sola istanza per tutto il gioco, nessun riferimento da collegare.
/// </summary>
public static class CombatEvents
{
    // L'evento. Il "?" in RaiseHit serve a non lanciare errori se nessuno è iscritto.
    public static event Action<DamageInfo> OnHit;

    // La Hitbox chiama questo per annunciare un colpo a tutti gli iscritti.
    public static void RaiseHit(DamageInfo info)
    {
        OnHit?.Invoke(info);
    }

    // SICUREZZA: gli eventi statici NON si azzerano da soli quando riavvii il Play
    // nell'editor (se hai disattivato il domain reload). Questo metodo li ripulisce
    // a ogni avvio, così non restano iscrizioni "fantasma" di sessioni precedenti.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        OnHit = null;
    }
}
