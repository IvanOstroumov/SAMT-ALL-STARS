using System;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script di scelta della mappa che salva la mappa scelta in PlayerPrefs
public class MapSelection : MonoBehaviour
{
    public static String selectedMap;
    
    public static GameObject sidon, ivan, yasser, quan;

    void Start()
    {
        selectedMap = string.Empty;
        sidon = GameObject.Find("sidon");
        ivan = GameObject.Find("ivan");
        quan = GameObject.Find("quan");
        yasser = GameObject.Find("yasser");
        hider();
    }

    void Update()
    {
        // Tasto di conferma: Invio sulla tastiera, X (Joystick1Button1) sul pad.
        if (!String.IsNullOrEmpty(selectedMap))
        {
            if (Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                LogManager.Info($"Mappa confermata: {selectedMap}");
                PlayerPrefs.SetString("Map", selectedMap);
                SceneManager.LoadScene("CharacterSelection");
            }
        }
    }

    // Questi metodi vengono chiamati dai Button della UI tramite OnClick.
    public static void selectSidon()
    {
        selectedMap = "sidon";
        hider();
        sidon.SetActive(true);
    }

    public static void selectQuan()
    {
        selectedMap = "quan";
        hider();
        quan.SetActive(true);
    }

    public static void selectIvan()
    {
        selectedMap = "ivan";
        hider();
        ivan.SetActive(true);
    }

    public static void selectYasser()
    {
        selectedMap = "yasser";
        hider();
        yasser.SetActive(true);
    }

    // Spegne tutte le anteprime. Lo chiamiamo prima di accendere quella scelta,
    // cosi ne resta visibile sempre solo una.
    private static void hider()
    {
        sidon.SetActive(false);
        ivan.SetActive(false);
        quan.SetActive(false);
        yasser.SetActive(false);
    }
}
