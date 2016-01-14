using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public static MusicManager musicManager;

	public bool persistAfterLoad = true;

	public AudioSource[] musicClips;
	private AudioSource turntableA;
	public float turntableAMaxVolume = 1.0f;
	private AudioSource turntableB;
	public float turntableBMaxVolume = 0.5f;

	public float fadeSpeed = 0.25f;

	public bool muteMusic = false;



	
	void Awake()
	{
		if(musicManager == null && persistAfterLoad)
		{
			DontDestroyOnLoad(gameObject);
			musicManager = this;
		}
		else if(musicManager == null && !persistAfterLoad)
		{
			musicManager = this;
		}
		else if (musicManager != this)
		{
			Destroy(gameObject);
		}

		try{
		turntableA = musicClips [0];
		turntableB = musicClips [1];
		}
		catch{
		}

	}


	
	public void FadeIn(AudioSource whichTrack)
	{
		if(whichTrack == turntableA && turntableA.volume < turntableAMaxVolume)
		{
			whichTrack.volume += fadeSpeed * Time.deltaTime; 
		}

		else if(whichTrack == turntableB && turntableB.volume < turntableBMaxVolume)
		{
			whichTrack.volume += fadeSpeed * Time.deltaTime; 
		}
	}
	
	public void FadeOut(AudioSource whichTrack)
	{
		if(whichTrack == turntableA && turntableA.volume > 0)
		{
			whichTrack.volume -= fadeSpeed * Time.deltaTime;
		}
		else if(whichTrack == turntableB && turntableB.volume >0)
		{
			whichTrack.volume -= fadeSpeed * Time.deltaTime;
		}
	}

	public void BecomeDontDestroyOnLoad()
	{
		DontDestroyOnLoad (gameObject);
		musicManager = this;
		persistAfterLoad = true;
	}
	public void BecomeDestroyOnLoad()
	{
		DontDestroyOnLoad (gameObject);
		persistAfterLoad = false;
	}
	public void DestroyThisNow()
	{
		DestroyImmediate (gameObject);
	}
	
}//Mono