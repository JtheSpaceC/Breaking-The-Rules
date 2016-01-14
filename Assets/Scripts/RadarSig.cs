using UnityEngine;
using System.Collections;

public class RadarSig : MonoBehaviour {

	SpriteRenderer myRenderer;


	void Awake () 
	{
		myRenderer = GetComponent<SpriteRenderer> ();
		myRenderer.enabled = true;
	}

}
