using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Regista dell'audio del gioco.
// Si crea da solo all'avvio, sopravvive ai cambi di scena, trova il mixer
// e i 4 gruppi (Master/Music/SFX/Voice) da solo.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioMixer mixer;
    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup musicGroup;
    private AudioMixerGroup voiceGroup;
    
    private const int SFX_VOICES = 8;
    private AudioSource[] sfxSources;
    private int sfxIndex;

    private AudioSource musicSource;   
    private AudioSource voiceSource;   

    // Listener "di scorta": lo porta l'AudioManager cosi siamo sicuri che ce ne sia sempre uno in scena.
    private AudioListener listener;

    // Clip già caricati: una volta sola da Resources, poi sta in RAM.
    private readonly Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();
    
    private readonly HashSet<string> missing = new HashSet<string>();

    private const string SFX_PATH = "Audio/SFX/";
    private const string MUSIC_PATH = "Audio/Music/";
    private const string VOICE_PATH = "Audio/Voice/";


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        GameObject go = new GameObject("AudioManager");
        go.AddComponent<AudioManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupMixer();
        SetupSources();
        StartCoroutine(ApplyVolumesFromPrefs());

        listener = gameObject.AddComponent<AudioListener>();
        LogManager.Info("AudioManager pronto");
    }

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
    

    private void SetupMixer()
    {
        mixer = UnityEngine.Resources.Load<AudioMixer>("Audio/MainMixer");
        if (mixer == null)
        {
            LogManager.Error("MainMixer non trovato in Resources/Audio/MainMixer");
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
            LogManager.Error($"Gruppo '{groupName}' non trovato nel mixer");
            return null;
        }
        return found[0];
    }

    // Legge i volumi salvati in PlayerPrefs e li applica subito al mixer.
    private IEnumerator ApplyVolumesFromPrefs()
    {
        yield return null;  
        if (mixer == null) yield break;

        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music  = PlayerPrefs.GetFloat("MusicVolume",  1f);
        float sfx    = PlayerPrefs.GetFloat("SFXVolume",    1f);
        float voice  = PlayerPrefs.GetFloat("VoiceVolume",  1f);

        ApplyVolume("MasterVolume", master);
        ApplyVolume("MusicVolume",  music);
        ApplyVolume("SFXVolume",    sfx);
        ApplyVolume("VoiceVolume",  voice);

        LogManager.Info($"Volumi caricati da prefs: master={master:F2} music={music:F2} sfx={sfx:F2} voice={voice:F2}");
    }
    
    private void ApplyVolume(string parameter, float value)
    {
        float db = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        if (parameter == "VoiceVolume") db += 3f;
        mixer.SetFloat(parameter, db);
    }

    private void SetupSources()
    {
        sfxSources = new AudioSource[SFX_VOICES];
        for (int i = 0; i < SFX_VOICES; i++)
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.playOnAwake = false;
            s.spatialBlend = 0f;                  
            s.outputAudioMixerGroup = sfxGroup;
            sfxSources[i] = s;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        musicSource.outputAudioMixerGroup = musicGroup;

        voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.playOnAwake = false;
        voiceSource.loop = false;
        voiceSource.spatialBlend = 0f;
        voiceSource.outputAudioMixerGroup = voiceGroup;
    }
    

    private AudioClip Load(string fullPath)
    {
        if (missing.Contains(fullPath)) return null;
        if (cache.TryGetValue(fullPath, out AudioClip clip)) return clip;

        clip = UnityEngine.Resources.Load<AudioClip>(fullPath);
        if (clip == null)
        {
            missing.Add(fullPath);
            return null;
        }
        cache[fullPath] = clip;
        return clip;
    }
    

    public void PlaySFX(string clipName, float volume = 1f)
    {
        AudioClip clip = Load(SFX_PATH + clipName);
        if (clip == null)
        {
            LogManager.Error($"SFX non trovato: {clipName}");
            return;
        }
        AudioSource s = sfxSources[sfxIndex];
        sfxIndex = (sfxIndex + 1) % SFX_VOICES;
        s.PlayOneShot(clip, volume);
    }

    public void PlayMusic(string clipName)
    {
        AudioClip clip = Load(MUSIC_PATH + clipName);
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    // Riproduce una voiceline. Ritorna la durata del clip (0 se non trovato), utile alle coroutine per attendere la fine.
    public float PlayVoice(string characterName, VoiceLine line, string opponent = null)
    {
        if (string.IsNullOrEmpty(characterName)) return 0f;

        AudioClip clip = LoadVoiceClip(characterName, line, opponent);
        if (clip == null) return 0f;

        voiceSource.Stop();
        voiceSource.clip = clip;
        voiceSource.Play();
        return clip.length;
    }
    
    public IEnumerator PlayVoiceAndWait(string characterName, VoiceLine line, string opponent = null)
    {
        float duration = PlayVoice(characterName, line, opponent);
        if (duration > 0f) yield return new WaitForSeconds(duration);
    }

    // Costruisce il path della voice e lo carica.
    private AudioClip LoadVoiceClip(string characterName, VoiceLine line, string opponent)
    {
        string folder = VOICE_PATH + Capitalize(characterName) + "/";

        switch (line)
        {
            case VoiceLine.Selection: return Load(folder + "Selection");
            case VoiceLine.Damage:    return Load(folder + "Damage");
            case VoiceLine.Victory:   return Load(folder + "Victory");

            case VoiceLine.Vs:
                if (string.IsNullOrEmpty(opponent)) return null;
                return Load(folder + Capitalize(characterName) + "To" + Capitalize(opponent));
        }
        return null;
    }

    private static string Capitalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s.Substring(1).ToLower();
    }
    

    private void OnHit(DamageInfo info)
    {
        if (!string.IsNullOrEmpty(info.HitSfx))
            PlaySFX(info.HitSfx);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogManager.Info($"Scena caricata: {scene.name}");

        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic("MainMenu");
                break;
            case "Game":
                PlayStageMusic();
                StartCoroutine(MatchIntro());
                break;
        }

        EnsureSingleListener();
        HookButtons();
    }

    private void PlayStageMusic()
    {
        string map = PlayerPrefs.GetString("Map", "sidon");
        if (string.IsNullOrEmpty(map)) map = "sidon";
        PlayMusic(Capitalize(map));
    }

    // Intro a inizio partita: P1 dice "Vs<avv>", poi P2 dice il suo. Sequenziale.
    private IEnumerator MatchIntro()
    {
        yield return new WaitForSeconds(0.4f);

        string p1 = PlayerPrefs.GetString("Player1", "");
        string p2 = PlayerPrefs.GetString("Player2", "");

        if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
        {
            yield return PlayVoiceAndWait(p1, VoiceLine.Vs, p2);
            yield return new WaitForSeconds(0.2f);
            yield return PlayVoiceAndWait(p2, VoiceLine.Vs, p1);
        }
    }

    // Tiene esattamente un AudioListener attivo nella scena.
    private void EnsureSingleListener()
    {
        AudioListener[] all = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        bool keptOne = false;
        foreach (AudioListener l in all)
        {
            if (!l.enabled) continue;
            if (!keptOne) keptOne = true;
            else l.enabled = false;
        }
        if (!keptOne) listener.enabled = true;
    }

    // Aggancia "button sound" a tutti i bottoni della scena, automaticamente.
    private void HookButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => PlaySFX("button sound"));
        }
    }
}

// Tipi di voiceline supportati.
public enum VoiceLine
{
    Selection,   
    Damage,     
    Victory,     
    Vs          
}