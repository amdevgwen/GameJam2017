using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioList : ScriptableObject {

    //Player Sounds
    public AudioClip Jump;
    public AudioClip drowningSound;
    public AudioClip[] Wave;
    public AudioClip SpawnSound;
    public AudioClip dieSound;
    public AudioClip[] Footsteps;

    public AudioClip[] ShipCreakSounds;
    public AudioClip superRareCreak;

}
