using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimateScript : MonoBehaviour {

	public Sprite[] frames;
	SpriteRenderer myRenderer;
	public float framesPerSecond = 4f;

	int currentFrame = 0;

	public bool randomiseFPSEveryFrame = false;
	public bool randomiseFPSAfterCycle = false;

	public int minFPS = 1;
	public int maxFPS = 12;


	void Start () 
	{
		myRenderer = GetComponent<SpriteRenderer> ();
		currentFrame = 0;
		StartCoroutine (Animate ());
	}


	public IEnumerator Animate()
	{
		if (currentFrame >= frames.Length)
		{
			currentFrame = 0;
			if(randomiseFPSAfterCycle)
			{
				framesPerSecond = Random.Range(minFPS, maxFPS);
			}
		}

		if(randomiseFPSEveryFrame)
		{
			framesPerSecond = Random.Range(minFPS, maxFPS);
		}

		myRenderer.sprite = frames [currentFrame];
		currentFrame++;

		yield return new WaitForSeconds (1/framesPerSecond);

		StartCoroutine (Animate ());
	}

	public void StopAnimating()
	{
		currentFrame = 0;
		myRenderer.sprite = frames [currentFrame];
		StopAllCoroutines ();
	}
}
