using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float howMuchTime = 1;
	public enum DestroyOrDisable {Destroy, Disable};
	public DestroyOrDisable myBehaviour;

	void OnEnable ()
	{
		Invoke("DestroyNow", howMuchTime);
	}
	
	void DestroyNow()
	{
		if(myBehaviour == DestroyOrDisable.Destroy)
		{
			Destroy(gameObject);
		}
		else if(myBehaviour == DestroyOrDisable.Disable)
		{
			gameObject.SetActive(false);
		}
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}
