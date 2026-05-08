using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wiki_changePlayer : MonoBehaviour
{
    private String tag = "PlayerGroup";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePlayer(GameObject select) 
    {
        GameObject[] groups = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject group in groups) 
        {
            RectTransform transform = group.GetComponent<RectTransform>();
         
            if (select.Equals(group))
            {
                Debug.Log(group.name);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
            }
            else 
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1300f);
            }
        }
    }
}
