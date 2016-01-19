	using UnityEngine;
	using System.Collections;

	public class FurniturePrefab : MonoBehaviour {

	[Tooltip("If you plan on keeping your prefabs messy, as opposed to having JUST empty gameobjects, then check this.")]
	public bool tidyGrandChildreAtStart = true;
	public bool leaveOutSomeObjects = false;

	[Header("Put the prefabs for objects here")]
	public GameObject bedPrefab;
	public GameObject cabinetPrefab;
	public GameObject tablePrefab;
	public GameObject table1Prefab;
	public GameObject chairPrefab;
	public GameObject chairWidePrefab;
	public GameObject chestPrefab;
	public GameObject partitionPrefab;
	public GameObject partition1Prefab;
	public GameObject largeLockerPrefab;
	public GameObject showerPrefab;
	public GameObject sinkPrefab;
	public GameObject storagePrefab;
	public GameObject trayPrefab;
	public GameObject coverObjectPrefab;
	public GameObject barrelPrefab;
	public GameObject controlPanelPrefab;
	public GameObject controlPanelCornerPrefab;
	public GameObject securityFloorPrefab;
	public GameObject cratePrefab;
	public GameObject doorPrefab;
	public GameObject securityCameraPrefab;
	public GameObject enemyPrefab;


	[Tooltip("Any object you want to spawn at runtime, put an empty game object here.")]
	[Header("Arrays of GameObjects for the room")]
	public GameObject[] beds;
	public GameObject[] cabinets;
	public GameObject[] tables;
	public GameObject[] table1s;
	public GameObject[] chairs;
	public GameObject[] chairWides;
	public GameObject[] chests;
	public GameObject[] partitions;
	public GameObject[] partition1s;
	public GameObject[] largeLockers;
	public GameObject[] showers;
	public GameObject[] sinks;
	public GameObject[] storages;
	public GameObject[] trays;
	public GameObject[] coverObjects;
	public GameObject[] barrels;

	public GameObject[] controlPanels;
	public GameObject[] controlPanelCorners;
	public GameObject[] securityFloors;
	public GameObject[] crates;
	public GameObject[] doors;
	public GameObject[] securityCameras;

	public GameObject[] enemies;

	void Start ()
	{
	        if(tidyGrandChildreAtStart)
	                ClearGrandchildren ();

	        GenerateRoom ();
	}

	[ContextMenu("Generate Room")]
	public void GenerateRoom()
	{
	        foreach(GameObject obj in beds)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(bedPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in cabinets)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(cabinetPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in tables)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(tablePrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in table1s)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(table1Prefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in chairs)
	        {	
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(chairPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in chairWides)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(chairWidePrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in chests)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(chestPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in partitions)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(partitionPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in partition1s)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(partition1Prefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in largeLockers)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(largeLockerPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in showers)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(showerPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in sinks)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(sinkPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in storages)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(storagePrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in trays)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(trayPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in coverObjects)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(coverObjectPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in barrels)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(barrelPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in controlPanels)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(controlPanelPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in controlPanelCorners)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(controlPanelCornerPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in securityFloors)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(securityFloorPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
	        foreach(GameObject obj in crates)
	        {
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
	                GameObject prefab = Instantiate(cratePrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
	        }
			foreach(GameObject obj in crates)
			{
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
					GameObject prefab = Instantiate(cratePrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
			}
			foreach(GameObject obj in doors)
			{
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
					GameObject prefab = Instantiate(doorPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
			}
			foreach(GameObject obj in securityCameras)
			{
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
					GameObject prefab = Instantiate(securityCameraPrefab, obj.transform.position, obj.transform.rotation)as GameObject;
					prefab.transform.SetParent(obj.transform, true);}
			}
			foreach(GameObject obj in enemies)
			{
				if(!leaveOutSomeObjects || (leaveOutSomeObjects && Random.Range (0,2) >= 1)){
					GameObject prefab = Instantiate(enemyPrefab, obj.transform.position, Quaternion.identity)as GameObject;
					prefab.transform.parent = null;
					prefab.name = "Enemy";					
					FindObjectOfType<LevelGenerator>().enemies.Add (prefab);
					FindObjectOfType<LevelGenerator>().securityRoomGuards.Add (prefab);
					prefab.GetComponent<Soldier>().state = Soldier.stateMachine.Guarding;
					prefab.GetComponent<Soldier>().GuardingState();
					prefab.GetComponent<Soldier>().targetLook = transform.position;}
			}			
	}

		//use in editor to remove visuals (real game assets) from placeholder-empty-Gameobjects if they've been left on
		[ContextMenu("Clear Grandchildren")]
		public void ClearGrandchildren()
		{
	        foreach(GameObject obj in beds)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in cabinets)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in tables)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in table1s)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in chairs)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in chairWides)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in chests)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in partitions)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in partition1s)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in largeLockers)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in showers)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in sinks)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in storages)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in trays)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in coverObjects)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in barrels)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in controlPanels)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in controlPanelCorners)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }

	        foreach(GameObject obj in securityFloors)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
	        foreach(GameObject obj in crates)
	        {
	                while(obj.transform.childCount > 0)
	                {
	                        DestroyImmediate(obj.transform.GetChild(0).gameObject);
	                }
	        }
			foreach(GameObject obj in doors)
			{
				while(obj.transform.childCount > 0)
				{
					DestroyImmediate(obj.transform.GetChild(0).gameObject);
				}
			}
			foreach(GameObject obj in securityCameras)
			{
				while(obj.transform.childCount > 0)
				{
					DestroyImmediate(obj.transform.GetChild(0).gameObject);
				}
			}
			foreach(GameObject obj in enemies)
			{
				while(obj.transform.childCount > 0)
				{
					DestroyImmediate(obj.transform.GetChild(0).gameObject);
				}
			}

		}//end of ClearGrandchildren()

}//Mono