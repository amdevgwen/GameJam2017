using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRef : MonoBehaviour {

    public static AudioRef instance;

    public AudioClip Jump;
    public AudioClip drowningSound;
    public AudioClip[] Wave;
    public AudioClip SpawnSound;
    public AudioClip dieSound;
    public AudioClip[] Footsteps;

    public AudioClip[] ShipCreakSounds;
    public AudioClip superRareCreak;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this.gameObject);
        }

    }
}
