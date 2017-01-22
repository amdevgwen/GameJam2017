using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip hoverSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

	public void PlayHoverSound()
    {

        SoundManager.Instance.PlayAudio(audioSource, hoverSound);
    }
}
