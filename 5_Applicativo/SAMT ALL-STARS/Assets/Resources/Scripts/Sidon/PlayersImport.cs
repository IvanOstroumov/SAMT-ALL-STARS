using UnityEngine;
using Resources.Scripts;
using UnityEngine.UI;

// Inietta sprite e animator controller nei Character.
public class PlayersImport : MonoBehaviour
{
    private CharacterManager characterManager;

    [Header("Controllers")]
    public RuntimeAnimatorController sidonController;
    public RuntimeAnimatorController ivanController;
    public RuntimeAnimatorController quanController;
    public RuntimeAnimatorController yasserController;

    [Header("Sprites")]
    public Sprite sidonSprite;
    public Sprite ivanSprite;
    public Sprite quanSprite;
    public Sprite yasserSprite;

    [Header("Names")]
    public Sprite sidonText;
    public Sprite ivanText;
    public Sprite quanText;
    public Sprite yasserText;

    GameManager gameManager;

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

        characterManager.getCharByName("sidon").NameImage = sidonText;
        characterManager.getCharByName("ivan").NameImage = ivanText;
        characterManager.getCharByName("quan").NameImage = quanText;
        characterManager.getCharByName("yasser").NameImage = yasserText;
    }
}