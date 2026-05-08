using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public static string player1Character;
    public static string player2Character;

    private string[] characters = { "sidon", "ivan", "quan", "yasser" };

    private int p1Index = 0;
    private int p2Index = 0;

    private bool p1Confirmed = false;
    private bool p2Confirmed = false;

    private bool p2HasChosen = false;

    // visti della conferma
    private GameObject p1Confirmation;
    private GameObject p2Confirmation;

    // Highlight mentre si naviga (XSelect)
    private GameObject sidonSelect, ivanSelect, quanSelect, yasserSelect;

    // Immagini che appaiono quando P1 conferma (numero 1)
    private GameObject sidonP1, ivanP1, quanP1, yasserP1;

    // Immagini che appaiono quando P2 conferma (numero 2)
    private GameObject sidonP2, ivanP2, quanP2, yasserP2;

    public GameObject P1, P2;

    // Cooldown per non scorrere troppo veloce (solo P2)
    private float inputCooldown = 0.25f;
    private float p2Timer = 0f;

    void Start()
    {
        player1Character = string.Empty;
        player2Character = string.Empty;

        p1Confirmation = GameObject.Find("P1Confirmed");
        p2Confirmation = GameObject.Find("P2Confirmed");

        sidonSelect = GameObject.Find("selectSidon");
        ivanSelect = GameObject.Find("selectIvan");
        quanSelect = GameObject.Find("selectQuan");
        yasserSelect = GameObject.Find("selectYasser");

        P1 = GameObject.Find("P1");
        P2 = GameObject.Find("P2");

        sidonP1 = GameObject.Find("Sidon (1)");
        ivanP1 = GameObject.Find("Ivan (1)");
        quanP1 = GameObject.Find("Quan (1)");
        yasserP1 = GameObject.Find("Yasser (1)");

        sidonP2 = GameObject.Find("Sidon (2)");
        ivanP2 = GameObject.Find("Ivan (2)");
        quanP2 = GameObject.Find("Quan (2)");
        yasserP2 = GameObject.Find("Yasser (2)");

        HideAllP1();
        p1Confirmation.SetActive(false);
        HideAllP2();
        UpdateDisplay();
    }

    void Update()
    {
        p2Timer -= Time.deltaTime;

        // ── PLAYER 2 (DualSense — unico joystick collegato) ───────────
        if (!p2Confirmed)
        {
            float axis2 = Input.GetAxis("Horizontal");

            if (p2Timer <= 0f && Mathf.Abs(axis2) > 0.5f)
            {
                if (axis2 > 0) p2Index = (p2Index + 1) % characters.Length;
                else p2Index = (p2Index - 1 + characters.Length) % characters.Length;
                p2Timer = inputCooldown;
                p2HasChosen = true;

                // Mostra subito l'immagine del personaggio su cui stai navigando
                HideAllP2();
                GetP2Image(p2Index).SetActive(true);
                GetSelect(p2Index).SetActive(true);
                P2.SetActive(false);
            }

            // X (Croce PS5) conferma il personaggio
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                p2Confirmed = true;
                player2Character = characters[p2Index];
                P2.SetActive(false);
                p2Confirmation.SetActive(true);
            }
        }

        // ── Entrambi confermati → vai alla scena di gioco ─────────────
        if (p1Confirmed && p2Confirmed)
        {
            PlayerPrefs.SetString("Player1", player1Character);
            PlayerPrefs.SetString("Player2", player2Character);
            SceneManager.LoadScene("game");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // FUNZIONI P1 — metti ognuna sul bottone corrispondente in Inspector
    // ═══════════════════════════════════════════════════════════════════

    public void SelectSidonP1()
    {
        if (p1Confirmed) return;
        SelectP1(0);
        HideAllP1();
        sidonP1.SetActive(true);
    }

    public void SelectIvanP1()
    {
        if (p1Confirmed) return;
        SelectP1(1);
        HideAllP1();
        ivanP1.SetActive(true);
    }

    public void SelectQuanP1()
    {
        if (p1Confirmed) return;
        SelectP1(2);
        HideAllP1();
        quanP1.SetActive(true);
    }

    public void SelectYasserP1()
    {
        if (p1Confirmed) return;
        SelectP1(3);
        HideAllP1();
        yasserP1.SetActive(true);
    }

    // Logica comune per la selezione P1
    private void SelectP1(int index)
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (string.IsNullOrEmpty(player1Character)) return;
            p1Confirmed = true;
            p1Confirmation.SetActive(true);
        }; 
        p1Index = index;
        player1Character = characters[index];
        P1.SetActive(false); // nasconde la scritta "P1"
        UpdateDisplay();
    }

    // ═══════════════════════════════════════════════════════════════════
    // DISPLAY
    // ═══════════════════════════════════════════════════════════════════

    public void UpdateDisplay()
    {
        HideAllP1();
       
        // P2: mostra highlight sul personaggio dove sta navigando
        if (!p2Confirmed)
            GetSelect(p2Index).SetActive(true);
        
        // Mostra immagine del personaggio scelto
        if(p1Confirmed) GetP1Image(p1Index).SetActive(true);
        if(p2Confirmed) GetP2Image(p2Index).SetActive(true);
    }

    private void HideAllP1()
    {
        // Nasconde tutte le immagini P1
        sidonP1.SetActive(false);
        ivanP1.SetActive(false);
        quanP1.SetActive(false);
        yasserP1.SetActive(false);
    }

    private void HideAllP2()
    {
        // Nasconde tutte le immagini P2
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