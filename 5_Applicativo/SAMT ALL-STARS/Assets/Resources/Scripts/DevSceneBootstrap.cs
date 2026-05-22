using Resources.Scripts;
using UnityEngine;

/// <summary>
/// SOLO PER TEST. Permette di lanciare direttamente la scena Game,
/// senza passare dalla scena di selezione personaggi.
///
/// Mettilo su un GameObject vuoto nella scena Game e assegna nell'Inspector
/// gli stessi sprite e controller che usi nella scena di selezione.
///
/// Se arrivi dalla selezione (flusso normale), questo script si accorge che
/// i dati ci sono gia e NON fa nulla: puoi lasciarlo anche nel gioco finito.
/// </summary>
public class DevSceneBootstrap : MonoBehaviour
{
    [Header("Personaggi di default per il test")]
    [SerializeField] private string player1Default = "ivan";
    [SerializeField] private string player2Default = "sidon";

    [Header("Sprite (gli stessi della scena di selezione)")]
    [SerializeField] private Sprite sidonSprite;
    [SerializeField] private Sprite ivanSprite;
    [SerializeField] private Sprite quanSprite;
    [SerializeField] private Sprite yasserSprite;

    [Header("Animator Controller (gli stessi della selezione)")]
    [SerializeField] private RuntimeAnimatorController sidonController;
    [SerializeField] private RuntimeAnimatorController ivanController;
    [SerializeField] private RuntimeAnimatorController quanController;
    [SerializeField] private RuntimeAnimatorController yasserController;

    // Awake gira PRIMA di tutti gli Start, quindi prima che PlayerController.Start
    // legga GameManager.Instance e le PlayerPrefs: il setup è pronto in tempo.
    private void Awake()
    {
        // 1) Mi assicuro che esista un GameManager.
        //    Il suo Awake crea il CharacterManager. Se vengo dalla selezione
        //    esiste gia grazie al DontDestroyOnLoad, quindi non ne creo un altro.
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager (Dev)");
            gm.AddComponent<GameManager>(); // AddComponent fa partire subito il suo Awake
        }

        CharacterManager cm = GameManager.Instance.characterManager;

        // 2) Se i personaggi sono gia popolati (vengo dalla selezione) esco subito:
        //    non sovrascrivo la scelta vera del giocatore.
        Character probe = cm.getCharByName("ivan");
        if (probe != null && probe.Sprite != null) return;

        // --- Da qui in poi: ho lanciato la scena Game DA SOLA, faccio il setup di test ---

        // 3) Popolo sprite e controller, esattamente come fa CharacterSelection.
        cm.getCharByName("sidon").Sprite = sidonSprite;
        cm.getCharByName("ivan").Sprite = ivanSprite;
        cm.getCharByName("quan").Sprite = quanSprite;
        cm.getCharByName("yasser").Sprite = yasserSprite;

        cm.getCharByName("sidon").Controller = sidonController;
        cm.getCharByName("ivan").Controller = ivanController;
        cm.getCharByName("quan").Controller = quanController;
        cm.getCharByName("yasser").Controller = yasserController;

        // 4) Forzo i due personaggi di default, cosi il test e sempre prevedibile
        //    anche se in passato avevi gia salvato una scelta nelle PlayerPrefs.
        PlayerPrefs.SetString("Player1", player1Default);
        PlayerPrefs.SetString("Player2", player2Default);
    }
}
