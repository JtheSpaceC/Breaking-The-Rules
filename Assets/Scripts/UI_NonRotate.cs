using UnityEngine;
using System.Collections;

public class UI_NonRotate : MonoBehaviour {
	
	public bool keepRotation = true;
	[Tooltip("To do with having an offset")] public bool keepPositionRelativeToParent = true;

	private Quaternion rotation;
	private Vector3 offset;

	void Awake()
	{
		rotation = transform.rotation;
		offset = transform.localPosition;
	}
	
	void LateUpdate () 
	{
		if(keepRotation)
		transform.rotation = rotation;	

		if (keepPositionRelativeToParent)
		transform.position = transform.root.position + offset;
	}
}
