using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    //public AudioSource audioSource;
    public AudioList sounds;


    // Use this for initialization
    void Awake()
    {
        //sounds = ScriptableObject.CreateInstance("Sounds") as AudioList;
    }

    public void PlayJumpSound()
	{
        if(sounds != null)
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), sounds.Jump);
	}

	public void PlaySpawnSound()
	{
        if (sounds != null)
            SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), sounds.SpawnSound);
	}

	public void PlayDieSounds()
	{
        if (sounds != null)
            SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), sounds.dieSound);
	}

	public void PlayWaveSound()
	{
        if (sounds != null)
            SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), sounds.Wave);
	}

	public void PlayFootsteps()
	{
        if (sounds != null)
            SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), sounds.Footsteps);
	}

	public void PlayDrowningSound()
	{
        if (sounds != null)
            SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), sounds.drowningSound);
	}
}


public static class AudioSourcePool
{

	private static List<AudioSource> sources = new List<AudioSource>();

	public static AudioSource GetSource(Transform targetToAttachTo)
	{
		if (sources.Exists(obj => obj != null && obj.isActiveAndEnabled && !obj.isPlaying))
		{
			var src = sources.Find(obj => obj != null && obj.isActiveAndEnabled && !obj.isPlaying);

			if (targetToAttachTo != null)
				src.transform.SetParent(targetToAttachTo);

			return src;
		}
		else
		{
			GameObject go = new GameObject();

			var src = go.AddComponent<AudioSource>();

			src.spatialBlend = 0.5f;

			if (targetToAttachTo != null)
				src.transform.SetParent(targetToAttachTo);

			sources.Add(src);

			return src;
		}
	}
}