using UnityEngine;
using Resources.Scripts;

public class PlayersImport : MonoBehaviour
{ 
    private CharacterManager characterManager;
    
    public RuntimeAnimatorController sidonController;
    public RuntimeAnimatorController ivanController;
    public RuntimeAnimatorController quanController;
    public RuntimeAnimatorController yasserController;
    
    GameManager gameManager;

    public Sprite sidonSprite;
    public Sprite ivanSprite;
    public Sprite quanSprite;
    public Sprite yasserSprite;
    
    void Start()
    {
        characterManager = GameManager.Instance.characterManager;
        
        characterManager.getCharByName("sidon").Sprite = sidonSprite;
        characterManager.getCharByName("ivan").Sprite = ivanSprite;
        characterManager.getCharByName("quan").Sprite = quanSprite;
        characterManager.getCharByName("yasser").Sprite = yasserSprite;

        characterManager.getCharByName("sidon").Controller = sidonController;
        characterManager.getCharByName("ivan").Controller = ivanController;
        characterManager.getCharByName("quan").Controller = quanController;
        characterManager.getCharByName("yasser").Controller = yasserController;
    }


}
