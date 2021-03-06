﻿using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ClickToPlay : MonoBehaviour {

	[Tooltip("Set true if you want the game to keep playing even if it loses focus.")]
	public bool allowedRunInBackground = true;

	[HideInInspector] public bool allowedToPause = true;
	public bool escCanQuit = true;
	public bool rCanRestart = true;
	bool paused = false;
	public Image theCanvas;

	[Header("Stuff to activate/ deactivate at pause")]
	public GameObject pauseScreen;
	public GameObject mouseCursor;
	public GameObject playerGun;

	[Header("If doing a Powerpoint-style presentation with slides through Unity.")]
	public bool usingSlides = false;
	public Sprite [] slides;
	
	public int whichSlideToShow = 0;

	[Header("Loading Bar stuff")]
	public Slider loadingSlider;

	int numRestarts;


	void Awake()
	{
		Cursor.visible = true;

		#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

		if(allowedRunInBackground)
			Application.runInBackground = true;
		#endif
	}
	
	void Start()
	{
		if (usingSlides)
			NextSlide (0);
	}
	
	void Update()
	{
		if(allowedToPause && Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause ();			
		}
		
		if (escCanQuit && Input.GetKeyDown (KeyCode.Escape))
			QuitGame ();
		
		if(_MANAGER.instance)
		{
			if (rCanRestart && (paused || _MANAGER.instance.gameOver) && Input.GetKeyDown(KeyCode.R))
			{
				Restart ();
			}
		}
	}


	public void LoadLevel(int whichLevel)
	{
		SceneManager.LoadScene(whichLevel);
	}


	public void CallLoadLevelAsync(int whichLevel)
	{
		StartCoroutine(LoadLevelAsync(whichLevel));
	}
	IEnumerator LoadLevelAsync(int whichLevel)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(whichLevel);

		while(!async.isDone)
		{
			loadingSlider.value = async.progress;
			yield return null;
		}
	}

	public void TogglePause ()
	{
		if (!paused) {
			Time.timeScale = 0;
			paused = true;
			mouseCursor.SetActive (false);
			playerGun.SetActive (false);
			if (pauseScreen != null)
				pauseScreen.SetActive (true);
		}
		else
			if (paused) {
				Time.timeScale = 1;
				paused = false;
				mouseCursor.SetActive (true);
				playerGun.SetActive (true);
				if (pauseScreen != null)
					pauseScreen.SetActive (false);
			}
	}

	public void Restart ()
	{
		FindObjectOfType<RPGelements> ().ResetToFirstLevel ();
		Time.timeScale = 1;
		numRestarts ++;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void QuitGame()
	{		
		#if UNITY_STANDALONE
		//Quit the application
		Application.Quit();
		#endif

		#if UNITY_WEBPLAYER
		Time.timeScale = 1;
		SceneManager.LoadScene(0);		
		#endif

		#if UNITY_WEBGL
		Time.timeScale = 1;
		SceneManager.LoadScene(0);		
		#endif

		//If we are running in the editor
		#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;

		#endif	

		Shooting playerShootScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().shootScript;

		Analytics.CustomEvent("Quit", new Dictionary<string, object>
			{
				{"Level quit on", RPGelements.instance.level},
				{"Ammo remaining", playerShootScript.ammoReserve + playerShootScript.ammoGun},
				{"Time in session", Time.time},
				{"Restarts", numRestarts}
			});
	}

	public void NextSlide(int fwdOrBackInt)
	{ 
		whichSlideToShow += fwdOrBackInt;
		
		if (whichSlideToShow < 0)
			whichSlideToShow = 0;
		
		else if (whichSlideToShow == slides.Length)
			whichSlideToShow -= 1;
		
		theCanvas.sprite = slides [whichSlideToShow];
	}
	
	public void DestroyMusicManager()
	{
		Destroy(GameObject.FindGameObjectWithTag ("MusicManager"));
	}

	public void Pause()
	{
		Time.timeScale = 0;
	}

	public void MouseLockToScreen()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void PrintTest()
	{
		print ("testing call");
	}
}