using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WebGLLoading : MonoBehaviour {

	public void LoadLevel()
	{
		SceneManager.LoadScene(1);
	}
}
