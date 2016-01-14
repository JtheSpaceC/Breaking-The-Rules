using UnityEngine;
using System.Collections;

public class cursor : MonoBehaviour {

	public Sprite [] cursors;
	SpriteRenderer myRenderer;


	void Awake()
	{
		myRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnEnable()
	{
		myRenderer.sprite = cursors [0];
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(Cursor.lockState != CursorLockMode.Confined && Input.GetMouseButton(0))
		{
			Cursor.lockState = CursorLockMode.Confined;
		}
		Vector2 mousePos = (Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition));
		transform.position = mousePos;
	}

	void OnTriggerStay2D()
	{

	}

	void OnDisable()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
