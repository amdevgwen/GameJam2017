using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip[] Jump;
    public AudioClip[] Footsteps;
    public AudioClip[] drowningSound;
    public AudioClip Wave;
    public AudioClip SpawnSound;
    public AudioClip dieSound;
    


    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	public void PlayJumpSound()
    {
        SoundManager.Instance.PlayAudio(audioSource, Jump);
    }

    public void PlaySpawnSound()
    {
        SoundManager.Instance.PlayAudio(audioSource, SpawnSound);
    }

    public void PlayDieSounds()
    {
        SoundManager.Instance.PlayAudio(audioSource, dieSound);
    }

    public void PlayWaveSound()
    {
        SoundManager.Instance.PlayAudio(audioSource, Wave);
    }

    public void PlayFootsteps()
    {
        SoundManager.Instance.PlayAudio(audioSource, Footsteps);
    }

    public void PlayDrowningSound()
    {
        SoundManager.Instance.PlayAudio(audioSource, drowningSound);
    }
}
