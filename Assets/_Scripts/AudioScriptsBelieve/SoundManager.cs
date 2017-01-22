using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance = null;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this) {
            Destroy(this);
        }
    }

    public void PlayAudio(AudioSource source, AudioClip clip)
    {
        if (clip == null)
            return;
        source.clip = clip;
        source.Play();
    }

    public void PlayAudio(AudioSource source, AudioClip[] clip)
    {
        if (clip == null || clip.Length == 0)
            return;
        int randomIndex = Random.Range(0, clip.Length);
        source.clip = clip[randomIndex];
        source.Play();
    }
}
