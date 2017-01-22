using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTriggers : MonoBehaviour {
    public bool musicTrigger;
    public bool sent = false;

    void Update()
    {
        if(musicTrigger && !sent)
        {
            sent = true;
            musicTrigger = false;
            StartMusic();
        }
    }

    public void StartMusic()
    {
        PersistentMusic.instance.StartTitleMusic();
    }
}
