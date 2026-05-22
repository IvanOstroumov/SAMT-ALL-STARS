using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class wiki_changePlayer : MonoBehaviour
{
    private CharacterManager characterManager;
    private Character characterSelected;
    private String tag = "PlayerGroup";
    public GameObject wins;
    public GameObject hp;
    public GameObject speed;
    public GameObject story;
    
    void Start()
    {
        characterManager = GameManager.Instance.characterManager;
        characterSelected = characterManager.getCharByName("yasser");
        ChangeTexts();
    }
    
    public void changePlayer(GameObject select) 
    {
        characterSelected = characterManager.getCharByName(select.name);
        GameObject[] groups = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject group in groups) 
        {
            RectTransform transform = group.GetComponent<RectTransform>();
         
            if (select.Equals(group))
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
            }
            else 
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1300f);
            }
        }
        ChangeTexts();
    }

    private void ChangeTexts()
    {

        speed.GetComponent<Text>().text = characterSelected.Data.Speed.ToString(CultureInfo.CurrentCulture);
        hp.GetComponent<Text>().text = characterSelected.Data.MaxHp.ToString();
        story.GetComponent<Text>().text = characterSelected.Data.Description;
        wins.GetComponent<Text>().text = characterSelected.Data.WinCount.ToString();

    }
}
