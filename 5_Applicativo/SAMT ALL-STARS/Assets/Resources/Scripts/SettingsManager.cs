using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;

    [Header("Control Images")]
    public GameObject pcImage;
    public GameObject controllerImage;

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

        ShowPC();
    }
    
    private void ApplyVolume(string parameter, float value)
    {
        float db = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
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

    public void ShowPC()
    {
        pcImage.SetActive(true);
        controllerImage.SetActive(false);
    }

    public void ShowController()
    {
        pcImage.SetActive(false);
        controllerImage.SetActive(true);
    }
}