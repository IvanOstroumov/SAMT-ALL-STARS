using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Regista centrale dell'audio del gioco.
///
/// Cosa fa, in breve:
/// - Si crea DA SOLO all'avvio del gioco: non devi metterlo in nessuna scena.
/// - Sopravvive ai cambi di scena (DontDestroyOnLoad), come il GameManager.
/// - Trova il MainMixer e i suoi gruppi (SFX/Music/Voice) da solo.
/// - Carica i suoni per NOME da Resources/Audio/... e li tiene in cache.
/// - Riproduce: SFX (anche sovrapposti), musica in loop, suono dei colpi.
/// - Mette in automatico la musica giusta a ogni scena e il "click" ai bottoni.
///
/// Per usarlo dagli altri script chiami solo:
///     AudioManager.Instance.PlaySFX("jump");
///     AudioManager.Instance.PlayMusic("MainMenu");
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Punto d'accesso globale: da qualsiasi script usi AudioManager.Instance
    public static AudioManager Instance { get; private set; }

    // --- Mixer e gruppi (trovati da soli all'avvio) ---
    private AudioMixer mixer;
    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup musicGroup;
    private AudioMixerGroup voiceGroup;   // pronto per il futuro (voci dei personaggi)

    // --- Sorgenti audio ---
    private const int SFX_VOICES = 8;     // quanti SFX possono suonare insieme
    private AudioSource[] sfxSources;     // "pool" di sorgenti per gli effetti
    private int sfxIndex;                 // a rotazione: quale sorgente usare adesso
    private AudioSource musicSource;      // una sola sorgente per la musica in loop

    // --- Cache dei clip già caricati (così ogni file si carica una volta sola) ---
    private readonly Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();

    // Cartelle dentro Resources dove cercare i suoni
    private const string SFX_PATH = "Audio/SFX/";
    private const string MUSIC_PATH = "Audio/Music/";

    // ───────────────────────────────────────────────────────────────────
    // AUTO-CREAZIONE
    // Questo metodo gira da solo all'avvio del gioco, PRIMA che parta la
    // prima scena. Crea il GameObject dell'AudioManager: tu non devi fare
    // assolutamente nulla in nessuna scena.
    // ───────────────────────────────────────────────────────────────────
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        GameObject go = new GameObject("AudioManager");
        go.AddComponent<AudioManager>();
    }

    private void Awake()
    {
        // Singleton: se per caso ne esiste già uno, questo si distrugge.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupMixer();
        SetupSources();
    }

    // Iscrizione/disiscrizione agli eventi. Sempre in coppia, per non lasciare
    // "iscrizioni fantasma".
    private void OnEnable()
    {
        CombatEvents.OnHit += OnHit;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        CombatEvents.OnHit -= OnHit;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ───────────────────────────────────────────────────────────────────
    // SETUP
    // ───────────────────────────────────────────────────────────────────
    private void SetupMixer()
    {
        // Carico il mixer condiviso. È lo STESSO che usa SettingsManager,
        // quindi i volumi degli slider valgono anche per questi suoni.
        mixer = UnityEngine.Resources.Load<AudioMixer>("Audio/MainMixer");
        if (mixer == null)
        {
            Debug.LogError("[AudioManager] MainMixer non trovato in Resources/Audio/MainMixer");
            return;
        }
        sfxGroup = FindGroup("SFX");
        musicGroup = FindGroup("Music");
        voiceGroup = FindGroup("Voice");
    }

    private AudioMixerGroup FindGroup(string groupName)
    {
        AudioMixerGroup[] found = mixer.FindMatchingGroups(groupName);
        if (found == null || found.Length == 0)
        {
            Debug.LogWarning($"[AudioManager] Gruppo '{groupName}' non trovato nel mixer.");
            return null;
        }
        return found[0];
    }

    private void SetupSources()
    {
        // Pool di sorgenti per gli SFX: serve a far suonare più effetti insieme
        // (es. salto + colpo) senza che si taglino a vicenda.
        sfxSources = new AudioSource[SFX_VOICES];
        for (int i = 0; i < SFX_VOICES; i++)
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.playOnAwake = false;
            s.outputAudioMixerGroup = sfxGroup;   // -> finisce nel gruppo SFX del mixer
            sfxSources[i] = s;
        }

        // Sorgente dedicata alla musica, in loop.
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicGroup;  // -> gruppo Music del mixer
    }

    // ───────────────────────────────────────────────────────────────────
    // CARICAMENTO CLIP (con cache)
    // ───────────────────────────────────────────────────────────────────
    private AudioClip Load(string folder, string clipName)
    {
        string key = folder + clipName;          // es. "Audio/SFX/jump"
        if (cache.TryGetValue(key, out AudioClip clip)) return clip;

        clip = UnityEngine.Resources.Load<AudioClip>(key);
        if (clip == null)
            Debug.LogWarning($"[AudioManager] Clip non trovato: Resources/{key}");

        cache[key] = clip;   // salvo anche se è null, così non riprovo a ogni chiamata
        return clip;
    }

    // ───────────────────────────────────────────────────────────────────
    // API PUBBLICA — i metodi che chiami dagli altri script
    // ───────────────────────────────────────────────────────────────────

    /// <summary>Riproduce un effetto sonoro da Resources/Audio/SFX/.</summary>
    public void PlaySFX(string clipName, float volume = 1f)
    {
        AudioClip clip = Load(SFX_PATH, clipName);
        if (clip == null) return;

        AudioSource s = sfxSources[sfxIndex];
        sfxIndex = (sfxIndex + 1) % SFX_VOICES;   // passo alla sorgente successiva
        s.PlayOneShot(clip, volume);
    }

    /// <summary>Avvia una musica in loop da Resources/Audio/Music/.</summary>
    public void PlayMusic(string clipName)
    {
        AudioClip clip = Load(MUSIC_PATH, clipName);
        if (clip == null) return;

        // Se sta già suonando proprio quella, non la riavvio.
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ───────────────────────────────────────────────────────────────────
    // EVENTI AUTOMATICI
    // ───────────────────────────────────────────────────────────────────

    // Suono del colpo: parte SOLO quando la Hitbox segna un colpo a segno.
    // Il nome del suono ("punch"/"kick") arriva dentro DamageInfo.
    private void OnHit(DamageInfo info)
    {
        if (!string.IsNullOrEmpty(info.HitSfx))
            PlaySFX(info.HitSfx);
    }

    // A ogni nuova scena: scelgo la musica giusta e aggancio il suono ai bottoni.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic("MainMenu");
                break;
            case "Game":
                PlayStageMusic();
                break;
            // Le altre scene (Settings, Wiki, selezioni) tengono la musica del menu.
        }

        HookButtons();
    }

    // Musica della mappa scelta. MapSelection salva la mappa in PlayerPrefs "Map"
    // con valori "sidon/ivan/quan/yasser". I file musica iniziano con la maiuscola.
    private void PlayStageMusic()
    {
        string map = PlayerPrefs.GetString("Map", "sidon");
        if (string.IsNullOrEmpty(map)) map = "sidon";

        // "sidon" -> "Sidon", "ivan" -> "Ivan", ecc.
        string trackName = char.ToUpper(map[0]) + map.Substring(1);
        PlayMusic(trackName);
    }

    // Aggiunge il suono "button sound" al click di TUTTI i bottoni della scena.
    // Così non devi collegare niente a mano. I bottoni vengono ricreati ad ogni
    // cambio scena, quindi l'aggancio si rifà da capo ogni volta: nessun doppione.
    private void HookButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => PlaySFX("button sound"));
        }
    }
}