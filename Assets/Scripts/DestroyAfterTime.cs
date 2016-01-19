using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float howMuchTime = 1;

	void Start ()
	{
		Invoke("DestroyNow", howMuchTime);
	}
	
	void DestroyNow()
	{
		Destroy(gameObject);
	}
}
