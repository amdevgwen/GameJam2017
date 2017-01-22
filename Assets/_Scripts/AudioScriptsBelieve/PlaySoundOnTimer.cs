using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTimer : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip[] ShipCreakSounds;
    public float soundTimer;
    float nextSoundTime;


	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        nextSoundTime = Time.time + soundTimer;
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= nextSoundTime)
        {
            nextSoundTime = Time.time + soundTimer;
            SoundManager.Instance.PlayAudio(audioSource, ShipCreakSounds);
        }
	}
}
