using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTimer : MonoBehaviour {

    AudioSource audioSource;
    public AudioList soundList;
    public float minSoundTimer;
    public float maxSoundTimer;
    float nextSoundTime;

    int soundPlayCount = 0;
    int playSoundCount = 0;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        nextSoundTime = Time.time + minSoundTimer;

        playSoundCount = Random.Range(15, 30);
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= nextSoundTime)
        {
            nextSoundTime = Time.time + Random.Range(minSoundTimer, maxSoundTimer);
            soundPlayCount++;
            if (soundPlayCount > playSoundCount)
            {
                SoundManager.Instance.PlayAudio(audioSource, soundList.superRareCreak);
                playSoundCount = Random.Range(20, 50);
            }
            else
                SoundManager.Instance.PlayAudio(audioSource, soundList.ShipCreakSounds);
        }
	}
}
