using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PostMatchManager : MonoBehaviour
{
    private CharacterManager characterManager;
    
    private Gamepad gamepad;

    private Character winnerCharacter;
    private Character loserCharacter;

    public GameObject winsValue;
    public GameObject winner;
    public GameObject loser;
    public GameObject winnerName;
    
    
    void Start()
    { 
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFX("win");
        
        gamepad = new Gamepad();
        gamepad.Gameplay.Enable();
        gamepad.Gameplay.Jump.performed += ctx => Exit();
        gamepad.Gameplay.Move.performed += ctx => Nothing();
        gamepad.Gameplay.Kick.performed += ctx => Nothing();
        gamepad.Gameplay.Punch.performed += ctx => Nothing();
        gamepad.Gameplay.Move.performed += ctx => Nothing();
        gamepad.Gameplay.Move.canceled += ctx => Nothing();
        
        characterManager = GameManager.Instance.characterManager;
        winnerCharacter = characterManager.getCharByName(PlayerPrefs.GetString("Winner"));
        loserCharacter = characterManager.getCharByName(PlayerPrefs.GetString("Loser"));
        Debug.Log(winnerCharacter.Data.Name);
        winner.GetComponent<Image>().sprite = winnerCharacter.Sprite;
        SetWidthKeepAspect(winner, 600);
        Transform winnerTransform = winner.transform;
        winnerTransform.localPosition = new Vector3(winnerTransform.localPosition.x, winnerTransform.localPosition.y, 0f);
        
        loser.GetComponent<Image>().sprite = loserCharacter.Sprite; 
        SetWidthKeepAspect(loser,227);
        Transform loserTransform = loser.transform;
        loserTransform.localPosition = new Vector3(loserTransform.localPosition.x, loserTransform.localPosition.y, 0f);
        
        winnerName.GetComponent<Image>().sprite = winnerCharacter.NameImage;
        SetWidthKeepAspect(winnerName, 1500);
        Transform winnerNameTransform = winnerName.transform;
        winnerNameTransform.localPosition = new Vector3(winnerNameTransform.localPosition.x, winnerNameTransform.localPosition.y, 0f);
        int wins = winnerCharacter.Data.WinCount;
        wins = wins + 1;
        winnerCharacter.Data.WinCount = wins;
        winsValue.GetComponent<Text>().text = wins.ToString();
        
    }
    
    
    private void SetWidthKeepAspect(GameObject img, float width)
    {
        img.GetComponent<Image>().SetNativeSize();
        float ratio = img.GetComponent<RectTransform>().rect.height / img.GetComponent<RectTransform>().rect.width;

        img.GetComponent<RectTransform>().sizeDelta =
            new Vector2(width, width * ratio);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Exit();
        }
    }
    
    private void Nothing(){}

    private void Exit()
    {
        AudioManager.Instance.StopMusic();
        characterManager.saveCharsToJSON();
        gamepad.Disable();
        UIManager.openMainMenu();
    }
    
}
