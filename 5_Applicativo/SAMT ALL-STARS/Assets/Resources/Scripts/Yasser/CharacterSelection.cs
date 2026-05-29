using Resources.Scripts;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script che gestice la selezione dei personaggi salvando quelli scelti nei PalyerPrefs
public class CharacterSelection : MonoBehaviour
{
    private Gamepad gamepad;

    private bool isLoadedScene;

    public static string player1Character;
    public static string player2Character;
    
    private string[] characters = { "sidon", "ivan", "quan", "yasser" };

    private int p1Index = 0;
    private int p2Index = 0;

    private bool p1Confirmed = false;
    private bool p2Confirmed = false;
    private bool karl;

    private bool p2HasChosen = false;
    
    private GameObject p1Confirmation;
    private GameObject p2Confirmation;
    
    private GameObject sidonSelect, ivanSelect, quanSelect, yasserSelect;
    
    private GameObject sidonP1, ivanP1, quanP1, yasserP1;

    private GameObject sidonP2, ivanP2, quanP2, yasserP2;

    private GameObject P1, P2;

    // Cooldown per evitare che P2 scorra a velocità luce quando tiene lo stick.
    private float inputCooldown = 0.25f;
    private float p2Timer = 0f;
    private float axis2;

    void Start()
    {
        karl = false;
        gamepad = new Gamepad();
        gamepad.Gameplay.Enable();
        
        gamepad.Gameplay.Move.performed += ctx => axis2 = ctx.ReadValue<float>();
        gamepad.Gameplay.Move.canceled  += ctx => axis2 = ctx.ReadValue<float>();
        gamepad.Gameplay.Jump.performed += ctx => karl  = ctx.ReadValueAsButton();

        isLoadedScene = false;
        player1Character = string.Empty;
        player2Character = string.Empty;
        
        p1Confirmation = GameObject.Find("P1Confirmed");
        p2Confirmation = GameObject.Find("P2Confirmed");

        sidonSelect = GameObject.Find("selectSidon");
        ivanSelect  = GameObject.Find("selectIvan");
        quanSelect  = GameObject.Find("selectQuan");
        yasserSelect = GameObject.Find("selectYasser");

        P1 = GameObject.Find("P1");
        P2 = GameObject.Find("P2");

        sidonP1 = GameObject.Find("Sidon (1)");
        ivanP1  = GameObject.Find("Ivan (1)");
        quanP1  = GameObject.Find("Quan (1)");
        yasserP1 = GameObject.Find("Yasser (1)");

        sidonP2 = GameObject.Find("Sidon (2)");
        ivanP2  = GameObject.Find("Ivan (2)");
        quanP2  = GameObject.Find("Quan (2)");
        yasserP2 = GameObject.Find("Yasser (2)");

        HideAllP1();
        p1Confirmation.SetActive(false);
        HideAllP2();
        UpdateDisplay();
    }

    void Update()
    {
        p2Timer -= Time.deltaTime;
        
        if (!p2Confirmed)
        {
            if (p2Timer <= 0f && Mathf.Abs(axis2) > 0.5f)
            {
                if (axis2 > 0) p2Index = (p2Index + 1) % characters.Length;
                else           p2Index = (p2Index - 1 + characters.Length) % characters.Length;
                p2Timer = inputCooldown;
                p2HasChosen = true;

                HideAllP2();
                GetP2Image(p2Index).SetActive(true);
                GetSelect(p2Index).SetActive(true);
                P2.SetActive(false);
            }
            
            if (karl)
            {
                p2Confirmed = true;
                player2Character = characters[p2Index];
                P2.SetActive(false);
                p2Confirmation.SetActive(true);

                LogManager.Info($"P2 ha scelto: {player2Character}");
                AudioManager.Instance.PlayVoice(player2Character, VoiceLine.Selection);
            }
        }
        
        if (p1Confirmed && p2Confirmed && !isLoadedScene)
        {
            PlayerPrefs.SetString("Player1", player1Character);
            PlayerPrefs.SetString("Player2", player2Character);
            LogManager.Info($"Match: {player1Character} vs {player2Character}");

            SceneManager.LoadScene("Game");
            isLoadedScene = true;
        }
    }
    
    public void SelectSidonP1()  { if (p1Confirmed) return; SelectP1(0); HideAllP1(); sidonP1.SetActive(true); }
    public void SelectIvanP1()   { if (p1Confirmed) return; SelectP1(1); HideAllP1(); ivanP1.SetActive(true); }
    public void SelectQuanP1()   { if (p1Confirmed) return; SelectP1(2); HideAllP1(); quanP1.SetActive(true); }
    public void SelectYasserP1() { if (p1Confirmed) return; SelectP1(3); HideAllP1(); yasserP1.SetActive(true); }


    private void SelectP1(int index)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (string.IsNullOrEmpty(player1Character)) return;
            p1Confirmed = true;
            p1Confirmation.SetActive(true);

            LogManager.Info($"P1 ha scelto: {player1Character}");
            AudioManager.Instance.PlayVoice(player1Character, VoiceLine.Selection);
        }
        p1Index = index;
        player1Character = characters[index];
        P1.SetActive(false);
        UpdateDisplay();
    }
    

    public void UpdateDisplay()
    {
        HideAllP1();

        if (!p2Confirmed)
            GetSelect(p2Index).SetActive(true);

        if (p1Confirmed) GetP1Image(p1Index).SetActive(true);
        if (p2Confirmed) GetP2Image(p2Index).SetActive(true);
    }

    private void HideAllP1()
    {
        sidonP1.SetActive(false);
        ivanP1.SetActive(false);
        quanP1.SetActive(false);
        yasserP1.SetActive(false);
    }

    private void HideAllP2()
    {
        sidonP2.SetActive(false);
        ivanP2.SetActive(false);
        quanP2.SetActive(false);
        yasserP2.SetActive(false);
        p2Confirmation.SetActive(false);
    }
    

    public GameObject GetSelect(int index)
    {
        switch (index)
        {
            case 0: return sidonSelect;
            case 1: return ivanSelect;
            case 2: return quanSelect;
            case 3: return yasserSelect;
            default: return sidonSelect;
        }
    }

    public GameObject GetP1Image(int index)
    {
        switch (index)
        {
            case 0: return sidonP1;
            case 1: return ivanP1;
            case 2: return quanP1;
            case 3: return yasserP1;
            default: return sidonP1;
        }
    }

    public GameObject GetP2Image(int index)
    {
        switch (index)
        {
            case 0: return sidonP2;
            case 1: return ivanP2;
            case 2: return quanP2;
            case 3: return yasserP2;
            default: return sidonP2;
        }
    }
}
