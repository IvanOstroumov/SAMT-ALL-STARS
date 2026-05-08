using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite sidon;
    public Sprite ivan;
    public Sprite quan;
    public Sprite yasser;
    private MapManager mapManager;
    private SpriteRenderer spriteRenderer;
    private string imageName;
    void Start()
    {
        mapManager = new MapManager(sidon,quan,yasser,ivan);
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageName = PlayerPrefs.GetString("Map");
        spriteRenderer.sprite = mapManager.getMapByName(imageName).Image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
