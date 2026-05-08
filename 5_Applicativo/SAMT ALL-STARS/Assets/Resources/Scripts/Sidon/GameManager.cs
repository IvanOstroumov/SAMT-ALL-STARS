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

        // CREA l'istanza qui, dentro Awake. 
        // Adesso Unity ti permette di usare Resources.Load o altre funzioni.
        characterManager = new CharacterManager();
    }

    // ... resto del codice
}