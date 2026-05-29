using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

// Sta sul background della scena Game. Legge la mappa scelta in PlayerPrefs e mette lo sprite giusto.
public class BackgroundController : MonoBehaviour
{
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
        mapManager = new MapManager(
            sidonBackground, sidonTeil,
            ivanBackground, ivanTeil,
            quanBackground, quanTeil,
            yasserBackground, yasserTeil
        );

        spriteRenderer = GetComponent<SpriteRenderer>();
        imageName = PlayerPrefs.GetString("Map");

        spriteRenderer.sprite = mapManager.getMapByName(imageName).Background;

        teils = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject tiel in teils)
        {
            tiel.GetComponent<SpriteRenderer>().sprite =
                mapManager.getMapByName(imageName).Teil;
        }
    }
}