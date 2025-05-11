using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 50f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 50f);
    }

    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void ResetDefaults()
    {
        musicSlider.value = 50f;
        sfxSlider.value = 50f;
        SaveAudioSettings();
    }
}
