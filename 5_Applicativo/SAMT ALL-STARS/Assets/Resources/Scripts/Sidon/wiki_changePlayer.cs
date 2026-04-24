using UnityEngine;

public class wiki_changePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePlayer() 
    {
        GameObject gameObject = GameObject.FindGameObjectsWithTag("p")[0];
    }
}
