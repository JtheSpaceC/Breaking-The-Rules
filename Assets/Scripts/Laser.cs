using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class Laser : MonoBehaviour {

	LineRenderer line;
	public LayerMask laserHittable;

	Vector2 mousePos;

	void Awake () {
		line = GetComponent<LineRenderer> ();
	}


	void Update () 
	{
		line.SetPosition (0, transform.position);

		mousePos = (Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition));

		float dist = Vector2.Distance (mousePos, transform.position);

		RaycastHit2D hit = Physics2D.Raycast (transform.position, mousePos - (Vector2)transform.position, dist, laserHittable);
		if (hit.collider != null) 
		{
			line.SetPosition (1, hit.point);
		} else 
		{
			line.SetPosition (1, mousePos);
		}
	}
}
