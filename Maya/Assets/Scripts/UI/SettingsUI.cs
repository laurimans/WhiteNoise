using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;

    public void SetMasterVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20;
        mainMixer.SetFloat("Master", dB);
    }

    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20;
        mainMixer.SetFloat("Music", dB);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20;
        mainMixer.SetFloat("SFX", dB);
    }
}
