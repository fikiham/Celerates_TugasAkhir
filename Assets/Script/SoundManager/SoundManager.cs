using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEffect
    {
        public string soundName;
        public AudioClip clip;
    }

    public static SoundManager Instance;

    public List<SoundEffect> soundEffects;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the SoundManager is not destroyed on scene load
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy the new one
            return; // Early return to avoid setting up audio source again
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string soundName)
    {
        SoundEffect sfx = soundEffects.Find(s => s.soundName == soundName);
        if (sfx != null && sfx.clip != null)
        {
            audioSource.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
}
