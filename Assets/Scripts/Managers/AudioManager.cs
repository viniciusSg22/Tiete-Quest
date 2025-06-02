using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum ESoundType
    {
        Jump,
        Shoot,
        Forest
    }

    [Serializable]
    public class Sound
    {
        public ESoundType Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [HideInInspector]
        public AudioSource Source;
    }

    public static AudioManager Instance;
    public Sound[] Sounds;

    private readonly Dictionary<ESoundType, Sound> soundDictionary = new();
    private AudioSource musicSource;

    private void Awake()
    {
        Instance = this;

        foreach (var sound in Sounds) soundDictionary[sound.Type] = sound;
    }

    public void PlaySound(ESoundType type)
    {
        if (!soundDictionary.TryGetValue(type, out Sound sound)) return;

        var soundObject = new GameObject($"Sound_{type}");
        var audioSource = soundObject.AddComponent<AudioSource>();

        float globalSfxVolume = PlayerPrefs.GetFloat("sfxVolume", 50f) / 100f;

        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume * globalSfxVolume;

        audioSource.Play();

        Destroy(soundObject, sound.Clip.length);
    }

    public void PlayMusic(ESoundType type)
    {
        if (!soundDictionary.TryGetValue(type, out Sound sound)) return;

        if (musicSource != null) Destroy(musicSource.gameObject);

        var musicObject = new GameObject($"Music_{type}");
        DontDestroyOnLoad(musicObject);
        musicSource = musicObject.AddComponent<AudioSource>();

        float globalMusicVolume = PlayerPrefs.GetFloat("musicVolume", 50f) / 100f;

        musicSource.clip = sound.Clip;
        musicSource.volume = sound.Volume * globalMusicVolume;
        musicSource.loop = true;

        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            Destroy(musicSource.gameObject);
            musicSource = null;
        }
    }

    public void UpdateMusicVolume()
    {
        if (musicSource != null)
        {
            float globalMusicVolume = PlayerPrefs.GetFloat("musicVolume", 50f) / 100f;
            musicSource.volume = globalMusicVolume;
        }
    }
}
