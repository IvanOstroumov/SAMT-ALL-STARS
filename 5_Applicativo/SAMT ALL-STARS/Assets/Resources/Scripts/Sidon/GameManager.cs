using Resources.Scripts;
using UnityEngine;

// Singleton globale. Esiste una sola istanza per tutta la durata del gioco e sopravvive ai cambi di scena (DontDestroyOnLoad).

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        LogManager.Info("GameManager avviato");
    }
}
