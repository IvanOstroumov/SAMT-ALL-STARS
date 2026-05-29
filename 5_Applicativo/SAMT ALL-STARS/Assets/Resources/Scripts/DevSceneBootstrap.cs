using Resources.Scripts;
using UnityEngine;

// SOLO TEST. Permette di lanciare la scena Game in autonomia, saltando
// la selezione personaggi/mappa. Comodo per provare il gameplay in fretta.
//
// Mettilo su un GameObject vuoto della scena Game e assegnagli nell'Inspector
// gli stessi sprite e controller che usa la scena di selezione.
//
// Se invece arrivi dalla selezione (flusso normale), questo script si accorge
// che i dati ci sono gia' e non fa nulla: puoi lasciarlo dentro anche in build.
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

    // Awake e non Start: gira prima di tutti gli Start, in particolare prima
    // che PlayerController.Start vada a leggere GameManager.Instance e le PlayerPrefs.
    // Cosi quando il player parte, trova tutto il setup gia' pronto.
    private void Awake()
    {
        // 1) Mi assicuro che esista un GameManager. Il suo Awake crea il CharacterManager.
        //    Se vengo dalla selezione esiste gia' (DontDestroyOnLoad), e non ne creo un altro.
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager (Dev)");
            gm.AddComponent<GameManager>();
            LogManager.Info("DevSceneBootstrap: creato GameManager al volo (test mode)");
        }

        CharacterManager cm = GameManager.Instance.characterManager;

        // 2) Se trovo gia' sprite assegnati (= vengo dalla selezione vera) esco.
        //    Non voglio sovrascrivere la scelta del giocatore.
        Character probe = cm.getCharByName("ivan");
        if (probe != null && probe.Sprite != null) return;

        LogManager.Info("DevSceneBootstrap: avvio diretto della scena Game, popolo dati di default");

        // 3) Da qui sono in modalita' test: popolo sprite e controller di tutti i 4 personaggi.
        cm.getCharByName("sidon").Sprite = sidonSprite;
        cm.getCharByName("ivan").Sprite = ivanSprite;
        cm.getCharByName("quan").Sprite = quanSprite;
        cm.getCharByName("yasser").Sprite = yasserSprite;

        cm.getCharByName("sidon").Controller = sidonController;
        cm.getCharByName("ivan").Controller = ivanController;
        cm.getCharByName("quan").Controller = quanController;
        cm.getCharByName("yasser").Controller = yasserController;

        // 4) Forzo i due default. Sovrascrive qualunque scelta precedente nelle PlayerPrefs
        //    cosi il test e' sempre prevedibile.
        PlayerPrefs.SetString("Player1", player1Default);
        PlayerPrefs.SetString("Player2", player2Default);
    }
}
