using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
