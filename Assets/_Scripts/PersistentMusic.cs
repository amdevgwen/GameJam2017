using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentMusic : MonoBehaviour {

    public AudioClip Title;
    public AudioClip music;
    public static PersistentMusic instance;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (PersistentMusic.instance == null)
        {
            PersistentMusic.instance = this;
         }else if(PersistentMusic.instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    
    public void StartGameMusic()
    {
        GetComponent<AudioSource>().clip = music;
        GetComponent<AudioSource>().Play();
    }
    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
    public void StartTitleMusic()
    {
        GetComponent<AudioSource>().clip = Title;
        GetComponent<AudioSource>().Play();
    }
}
