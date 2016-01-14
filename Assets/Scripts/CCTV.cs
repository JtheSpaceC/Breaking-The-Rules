using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCTV : MonoBehaviour {

	LevelGenerator levelScript;
	public float camJumpTime = 3;
	List<TileRecord> tileRecords;
	Vector3 offset = new Vector3(0,0,-10);

	void Start () {
		levelScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator> ();
		tileRecords = levelScript.goodTileRecords;
		InvokeRepeating ("ShiftPosition", 0, camJumpTime);
	}
	
	void ShiftPosition()
	{
		if(tileRecords.Count > 0)
			transform.position = tileRecords [Random.Range (0, tileRecords.Count)].gridObject.transform.position + offset;
	}
}
