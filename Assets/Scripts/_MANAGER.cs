﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class _MANAGER : MonoBehaviour {

	public static _MANAGER instance;

	Shooting playerShooting;

	[Header("For Testing")]
	public bool doLevelFadeInThing = true;
	public bool hackCamsCheatActive = false;

	[HideInInspector]public GameObject player;
	[Header("Loot Prefabs")]
	public GameObject keycardGreenPrefab;
	public GameObject medkitPrefab;
	public GameObject ammoPrefab;
	public GameObject syringePrefab;

	[Header("Other")]
	public AudioClip elevatorStopping;
	public Image blackoutPanel;
	public GameObject rpgStatsPanel;
	Color blackOutPanelColor;
	public Image titleImage;
	Color titleImageColor;
	public Text sublevelText;
	public Text missionText;
	public Text controlsText;
	public Text whichLevel1;
	public Text whichLevel2;
	public GameObject gameOverCanvas;
	public GameObject levelComplete;

	public float fadeInOutTime = 2f;
	[HideInInspector] public float startTime;
	[HideInInspector] public float levelStartTime;

	public bool fadingIn = false;
	public bool fadingOut = false;
	public float delayBeforeFadeStart = 2f;

	public bool gameOver = false;
	public bool levelCamsHacked = false;


	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{		
		player = GameObject.FindGameObjectWithTag ("Player");
		playerShooting = player.GetComponentInChildren<Shooting> ();
		levelStartTime = Time.time;
		startTime = delayBeforeFadeStart;
		Invoke ("MakeAllowedToPause", delayBeforeFadeStart);

		if(RPGelements.rpgElements.endingAmmo >= 30)
		{
			playerShooting.ammoGun = 30;
			playerShooting.ammoReserve = RPGelements.rpgElements.endingAmmo - playerShooting.ammoGun;
		}
		else
		{
			playerShooting.ammoGun = RPGelements.rpgElements.endingAmmo;
			playerShooting.ammoReserve = 0;
		}
		playerShooting.UpdateAmmoText ();

		RPGelements.rpgElements.damageTaken = 0;

		RPGelements.rpgElements.HoloText ();

		string text = "MISSION:\n" +
			"Retrieve the TELUMA Algorithm from sublevel " + RPGelements.rpgElements.telumaOnLevel + ".";
		
		whichLevel1.text = text;
		whichLevel2.text = text;

		if(doLevelFadeInThing)
		{
			StartCoroutine( DoLevelFadeIn());
		}
		else
		{
			PlayerCanStart();
		}
	}

	IEnumerator DoLevelFadeIn()
	{
		fadingIn = true;
		blackoutPanel.gameObject.SetActive (true);
		blackOutPanelColor = blackoutPanel.color;
		titleImageColor = titleImage.color;
		AudioSource.PlayClipAtPoint (elevatorStopping, player.transform.position);

		yield return new WaitForSeconds(delayBeforeFadeStart);

		while(blackoutPanel.color.a != 0)
		{
			Color newColor = Color.Lerp (blackOutPanelColor, Color.clear, (Time.time - levelStartTime - startTime) / fadeInOutTime);
			blackoutPanel.color = newColor;
			Color newColor2 = Color.Lerp (titleImageColor, Color.clear, (Time.time - levelStartTime - startTime) / fadeInOutTime);
			titleImage.color = newColor2;
			sublevelText.color = newColor2;
			sublevelText.text = "Sublevel: "+ RPGelements.rpgElements.level;
			missionText.color = newColor2;
			controlsText.color = newColor2;
			yield return new WaitForEndOfFrame();
		}

		fadingIn = false;
	}


	public void PlayerCanStart()
	{
		GameObject.Find("Panel (Wrist)").gameObject.SetActive(false);
		FindObjectOfType<PlayerMovement>().enabled = true;
		player.GetComponentInChildren<Shooting>().enabled = true;
		player.transform.FindChild("Radar(Output)").gameObject.SetActive(true);
		FindObjectOfType<ClickToPlay>().MouseLockToScreen();
		FindObjectOfType<CameraScript>().SwitchCanShowMap();
		FindObjectOfType<CameraScript>().ToggleMap();
	}


	public GameObject getLootPrefab(string name)
	{
		if (name == "Keycard (Green)") 
		{
			return keycardGreenPrefab;
		} 
		else if(name == "Medkit")
		{
			return medkitPrefab;
		}
		else if( name == "Ammo Pickup")
		{
			return ammoPrefab;
		}
		else if( name == "Syringe")
		{
			return syringePrefab;
		}
		else
			return new GameObject();
	}

	public IEnumerator RPGstatsScreen()
	{
		levelComplete.SetActive (true);
		Cursor.visible = true;

		yield return new WaitForSeconds (6.5f);

		levelComplete.SetActive (false);
		RPGelements.rpgElements.endingHealth = player.GetComponent<Health> ().health;
		RPGelements.rpgElements.endingAmmo = player.GetComponentInChildren<Shooting> ().ammoGun + player.GetComponentInChildren<Shooting> ().ammoReserve;

		rpgStatsPanel.SetActive (true);
	}

	public void EndLevel()
	{
		Debug.Log("I must have been clicked");
		RPGelements.rpgElements.level++;
		RPGelements.rpgElements.hasKey = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void GameOver()
	{
		gameOver = true;

		gameOverCanvas.SetActive (true);
		gameOverCanvas.transform.FindChild ("Panel").FindChild ("Extra Text").GetComponent<Text> ().text =
			"You reached Sublevel: " + RPGelements.rpgElements.level + "\n\n" +
			"Press 'R' to Restart";
		RPGelements.rpgElements.hasKey = false;
		Camera.main.GetComponent<ClickToPlay> ().allowedToPause = false;
	}

	void MakeAllowedToPause()
	{
		Camera.main.GetComponent<ClickToPlay> ().allowedToPause = true;
	}
}
