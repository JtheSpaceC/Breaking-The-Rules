using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

	AudioSource myAudioSource;
	public AudioClip[] barrelHitSounds;
	
	
	void Awake()
	{
		myAudioSource = GetComponent<AudioSource> ();
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void PlayHitSound()
	{
		myAudioSource.clip = barrelHitSounds[Random.Range(0, barrelHitSounds.Length)];
		myAudioSource.pitch = Random.Range (0.95f, 1.05f);
		myAudioSource.Play ();
	}
}