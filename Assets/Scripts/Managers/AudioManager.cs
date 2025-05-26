using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum ESoundType
    {
        Jump,
        Shoot,
        Damage,
        PlayerDeath,
        EnemyDeath
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

    private void Awake()
    {
        Instance = this;

        foreach (var sound in Sounds) _soundDictionary[sound.Type] = sound;
    }

    public ESoundType SelectedSound;

    public void PlaySound(ESoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound sound))
        {
            Debug.Log($"Som {sound} não encontrado");
            return;
        }

        var soundObject = new GameObject($"Sound_{type}");
        var audioSource = soundObject.AddComponent<AudioSource>();

        float globalSfxVolume = PlayerPrefs.GetFloat("sfxVolume", 50f) / 100f;

        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume * globalSfxVolume;

        audioSource.Play();

        Destroy(soundObject, sound.Clip.length);
    }
}
