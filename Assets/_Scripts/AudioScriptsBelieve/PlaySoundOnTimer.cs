using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTimer : MonoBehaviour {

    AudioSource audioSource;
    public AudioList soundList;
    public float minSoundTimer;
    public float maxSoundTimer;
    float nextSoundTime;


	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        nextSoundTime = Time.time + minSoundTimer;
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= nextSoundTime)
        {
            nextSoundTime = Time.time + Random.Range(minSoundTimer, maxSoundTimer);
            SoundManager.Instance.PlayAudio(audioSource, soundList.ShipCreakSounds);
        }
	}
}
