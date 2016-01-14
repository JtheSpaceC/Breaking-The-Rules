using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HackingZone : MonoBehaviour {
	
	float timer = 0;
	float hackDifficultyInSeconds = 3;
	Image fillerImage;
	SpriteRenderer hintImage;
	public Sprite mapUpdatedImage;
	bool alreadyHacked = false;
	AudioSource audioSource;


	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
		fillerImage = GameObject.FindGameObjectWithTag ("HackGraphic").GetComponent<Image> ();
		fillerImage.fillAmount = 0;
		hintImage = GetComponent<SpriteRenderer> ();
	}

	void Start()
	{
		InvokeRepeating ("CheckLevelHacked", 1.5f, 1.5f);
		float rot = hintImage.transform.parent.rotation.z;
		hintImage.transform.rotation = Quaternion.Euler (0, 0, rot);
	}

	void Update()
	{
		if(_MANAGER.instance.hackCamsCheatActive && Input.GetKeyDown(KeyCode.Backspace))
			HackLevelCams();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			hintImage.enabled = true;
			fillerImage.transform.root.transform.position = transform.position;
		}

		if (Input.GetKey (KeyCode.E))
		{
			timer += Time.deltaTime;
			fillerImage.fillAmount = timer / hackDifficultyInSeconds;
			hintImage.enabled = false;
			if(!audioSource.isPlaying && other.tag == "Player")
			{
				audioSource.Play();
			}
		}
		else
		{
			timer = 0;
			fillerImage.fillAmount = timer / hackDifficultyInSeconds;
		}

		if(fillerImage.fillAmount >= 1 && !alreadyHacked)
		{
			HackLevelCams();
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
			hintImage.enabled = false;
	}


	void CheckLevelHacked()
	{
		if (_MANAGER.instance.levelCamsHacked)
		{
			Destroy(gameObject, 1.5f);
		}

	}

	void HackLevelCams()
	{
		alreadyHacked = true;
		timer = 0;
		fillerImage.fillAmount = 0;
		hintImage.sprite = mapUpdatedImage;
		hintImage.enabled = true;

		GameObject[] fogs = GameObject.FindGameObjectsWithTag("Fog");

		foreach(GameObject fog in fogs)
		{
			fog.SendMessage("TurnOffFog", SendMessageOptions.DontRequireReceiver);
		}
		_MANAGER.instance.levelCamsHacked = true;
		GetComponent<BoxCollider2D> ().enabled = false;
		Destroy(gameObject, 1.5f);
		audioSource.Stop ();
	}
}
