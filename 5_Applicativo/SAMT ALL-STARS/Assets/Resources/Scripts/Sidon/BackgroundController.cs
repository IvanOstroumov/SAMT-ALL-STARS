using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

// Sta sul background della scena Game. Legge la mappa scelta in PlayerPrefs e mette lo sprite giusto. Funziona insieme al MapManager che fa da indice.
public class BackgroundController : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite sidonBackground;
    public Sprite sidonTeil;
    public Sprite ivanBackground;
    public Sprite ivanTeil;
    public Sprite quanBackground;
    public Sprite quanTeil;
    public Sprite yasserBackground;
    public Sprite yasserTeil;
    private MapManager mapManager;
    private SpriteRenderer spriteRenderer;
    private string imageName;
    private GameObject[] teils;
    
    void Start()
    {
        mapManager = new MapManager(sidonBackground, sidonTeil, ivanBackground, ivanTeil,quanBackground, quanTeil, yasserBackground, yasserTeil);
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageName = PlayerPrefs.GetString("Map");
        spriteRenderer.sprite = mapManager.getMapByName(imageName).Background;
        teils = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject tiel in teils){
            tiel.GetComponent<SpriteRenderer>().sprite = mapManager.getMapByName(imageName).Teil;
            
        }

    }
=======
    public Sprite sidon;
    public Sprite ivan;
    public Sprite quan;
    public Sprite yasser;

    private MapManager mapManager;
    private SpriteRenderer spriteRenderer;
    private string imageName;

    void Start()
    {
        mapManager = new MapManager(sidon, quan, yasser, ivan);
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageName = PlayerPrefs.GetString("Map");
>>>>>>> Stashed changes

        Map chosen = mapManager.getMapByName(imageName);
        if (chosen == null)
        {
            LogManager.Error($"BackgroundController: mappa '{imageName}' non trovata");
            return;
        }

        spriteRenderer.sprite = chosen.Image;
    }
}
