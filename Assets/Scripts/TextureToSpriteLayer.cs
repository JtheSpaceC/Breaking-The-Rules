using UnityEngine;
using System.Collections;

public class TextureToSpriteLayer : MonoBehaviour {

	public string desiredLayerName;
	public int desiredSortingOrder = 0;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().sortingLayerName = desiredLayerName;
		GetComponent<Renderer> ().sortingOrder = desiredSortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
