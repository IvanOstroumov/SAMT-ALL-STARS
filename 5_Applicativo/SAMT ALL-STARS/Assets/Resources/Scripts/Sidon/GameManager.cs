 using Resources.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Dichiara solo la variabile, NON fare = new CharacterManager() qui!
    public CharacterManager characterManager;

    private void Awake()
    { 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        characterManager = new CharacterManager();
    }
}