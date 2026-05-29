using Resources.Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Schermata Settings: collega gli slider audio al mixer e gestisce
// la scelta della periferica di gioco (tastiera o controller).
//
// I volumi vengono salvati in PlayerPrefs e riapplicati ad ogni avvio,
// cosi quello che il giocatore imposta una volta resta per le prossime sessioni.
public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;

    [Header("Keyboard")]
    public GameObject keyboardButton;
    public GameObject keyboardButtonClicked;
    public GameObject keboardImage;
    public GameObject keyCodeImage;

    [Header("Controller")]
    public GameObject controllerButton;
    public GameObject controllerButtonClicked;
    public GameObject controllerImage;
    public GameObject controlImage;


    void Start()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        voiceSlider.onValueChanged.RemoveAllListeners();
        
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music  = PlayerPrefs.GetFloat("MusicVolume",  1f);
        float sfx    = PlayerPrefs.GetFloat("SFXVolume",    1f);
        float voice  = PlayerPrefs.GetFloat("VoiceVolume",  1f);

        masterSlider.value = master;
        musicSlider.value  = music;
        sfxSlider.value    = sfx;
        voiceSlider.value  = voice;

        ApplyVolume("MasterVolume", master);
        ApplyVolume("MusicVolume",  music);
        ApplyVolume("SFXVolume",    sfx);
        ApplyVolume("VoiceVolume",  voice);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        voiceSlider.onValueChanged.AddListener(SetVoiceVolume);

        LogManager.Info($"Settings: master={master:F2} music={music:F2} sfx={sfx:F2} voice={voice:F2}");

        ShowController();
    }

    private void ApplyVolume(string parameter, float value)
    {
        float db = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        if (parameter == "VoiceVolume") db += 3f;
        mixer.SetFloat(parameter, db);
    }

    public void SetMasterVolume(float value)
    {
        ApplyVolume("MasterVolume", value);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        ApplyVolume("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float value)
    {
        ApplyVolume("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void SetVoiceVolume(float value)
    {
        ApplyVolume("VoiceVolume", value);
        PlayerPrefs.SetFloat("VoiceVolume", value);
        PlayerPrefs.Save();
    }

    // Bottoni "PC" / "Controller" della UI: mostrano l'immagine giusta dei tasti.
    public void ShowPC()
    {
        keyboardButtonClicked.SetActive(true);
        keyboardButton.SetActive(false);
        keyCodeImage.SetActive(true);
        keboardImage.SetActive(true);
        controllerButtonClicked.SetActive(false);
        controllerImage.SetActive(false);
        controlImage.SetActive(false);
        controllerButton.SetActive(true);
    }

    public void ShowController()
    {
        keyboardButtonClicked.SetActive(false);
        keyboardButton.SetActive(true);
        keyCodeImage.SetActive(false);
        keboardImage.SetActive(false);
        controllerButtonClicked.SetActive(true);
        controllerImage.SetActive(true);
        controlImage.SetActive(true);
        controllerButton.SetActive(false);
    }
}