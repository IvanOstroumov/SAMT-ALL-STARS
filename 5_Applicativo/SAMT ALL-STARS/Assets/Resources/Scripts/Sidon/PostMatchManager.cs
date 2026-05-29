using Resources.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PostMatchManager : MonoBehaviour
{
    private CharacterManager characterManager;

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
        
        characterManager = GameManager.Instance.characterManager;
        winnerCharacter = characterManager.getCharByName(PlayerPrefs.GetString("Winner"));
        loserCharacter = characterManager.getCharByName(PlayerPrefs.GetString("Loser"));
        Debug.Log(winnerCharacter.Data.Name);
        winner.GetComponent<Image>().sprite = winnerCharacter.Sprite;
        winner.GetComponent<Image>().SetNativeSize();
        Transform winnerTransform = winner.transform;
        winnerTransform.localPosition = new Vector3(winnerTransform.localPosition.x, winnerTransform.localPosition.y, 0f);
        
        loser.GetComponent<Image>().sprite = loserCharacter.Sprite; 
        loser.GetComponent<Image>().SetNativeSize();
        Transform loserTransform = loser.transform;
        loserTransform.localPosition = new Vector3(loserTransform.localPosition.x, loserTransform.localPosition.y, 0f);
        
        winnerName.GetComponent<Image>().sprite = winnerCharacter.NameImage;
        winnerName.GetComponent<Image>().SetNativeSize();
        Transform winnerNameTransform = winnerName.transform;
        winnerNameTransform.localPosition = new Vector3(winnerNameTransform.localPosition.x, winnerNameTransform.localPosition.y, 0f);

        winsValue.GetComponent<Text>().text = winnerCharacter.Data.WinCount.ToString();
        
        
    }
    void Update()
    {
        
    }
}
