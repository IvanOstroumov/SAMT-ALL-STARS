using System;
using System.Xml.Schema;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
    public static String selectedMap;

    public static GameObject sidon;
    public static GameObject ivan;
    public static GameObject quan;
    public static GameObject yasser;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedMap = string.Empty;
        sidon = GameObject.Find("sidon");
        ivan = GameObject.Find("ivan");
        quan = GameObject.Find("quan");
        yasser = GameObject.Find("yasser");
        hider();
    }

    // Update is called once per frame
    void Update()
    {
        if (!String.IsNullOrEmpty(selectedMap)){
            if (Input.GetKeyDown(KeyCode.Return)){
                PlayerPrefs.SetString("Map", selectedMap);
                SceneManager.LoadScene("CharacterSelection");
            }
        }
    }


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
    private static void hider()
    {
        sidon.SetActive(false);
        ivan.SetActive(false);
        quan.SetActive(false);
        yasser.SetActive(false);
    }
}
