using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

	public AudioSource audioSource;
	public AudioClip[] Jump;
	public AudioClip[] Footsteps;
	public AudioClip[] drowningSound;
	public AudioClip Wave;
	public AudioClip SpawnSound;
	public AudioClip dieSound;
    


	// Use this for initialization
	//	void Start()
	//	{
	//		audioSource = GetComponent<AudioSource>();
	//	}

	public void PlayJumpSound()
	{
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), Jump);
	}

	public void PlaySpawnSound()
	{
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), SpawnSound);
	}

	public void PlayDieSounds()
	{
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), dieSound);
	}

	public void PlayWaveSound()
	{
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), Wave);
	}

	public void PlayFootsteps()
	{
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), Footsteps);
	}

	public void PlayDrowningSound()
	{
		SoundManager.Instance.PlayAudio(AudioSourcePool.GetSource(this.transform), drowningSound);
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