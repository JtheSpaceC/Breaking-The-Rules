using UnityEngine;
using System.Collections;

public class spawnChanceByLevel : MonoBehaviour {

	public float baseChanceToSpawn = 10;
	public float chanceMultiplierPerLevel = 10;

	void Start () 
	{
		float chanceToSpawn = baseChanceToSpawn + (RPGelements.instance.level * chanceMultiplierPerLevel);
		float roll = Random.Range (0, 101);
		if (roll >= chanceToSpawn)
			Destroy (gameObject);
	}
}
