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

    private Dictionary<ESoundType, Sound> _soundDictionary = new();
    private AudioSource _musicSource;

    private void Awake()
    {
        Instance = this;

        foreach (var sound in Sounds) _soundDictionary[sound.Type] = sound;
    }

    public void PlaySound(ESoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound sound)) return;

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
        if (!_soundDictionary.TryGetValue(type, out Sound sound)) return;

        if (_musicSource != null) Destroy(_musicSource.gameObject);

        var musicObject = new GameObject($"Music_{type}");
        DontDestroyOnLoad(musicObject);
        _musicSource = musicObject.AddComponent<AudioSource>();

        float globalMusicVolume = PlayerPrefs.GetFloat("musicVolume", 50f) / 100f;

        _musicSource.clip = sound.Clip;
        _musicSource.volume = sound.Volume * globalMusicVolume;
        _musicSource.loop = true;

        _musicSource.Play();
    }

    public void StopMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            Destroy(_musicSource.gameObject);
            _musicSource = null;
        }
    }

    public void UpdateMusicVolume()
    {
        if (_musicSource != null)
        {
            float globalMusicVolume = PlayerPrefs.GetFloat("musicVolume", 50f) / 100f;
            _musicSource.volume = globalMusicVolume;
        }
    }
}
