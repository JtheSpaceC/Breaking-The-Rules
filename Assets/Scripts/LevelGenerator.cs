using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class LevelGenerator : MonoBehaviour {

	public AstarPath pathfinderScript;
	public GameObject fogPrefab;
	public GameObject keyPrefab;
	public GameObject securityRoomPrefab;

	public int mapSizeX = 9;
	public int mapSizeY = 9;
	public float gridSize = 12;
	Vector2 centrePoint;
	int centreX;
	int centreY;


	public GameObject emptyWorldHolder;
	public GameObject[] allGrids;
	public GameObject[] allFurniture;
	[HideInInspector]public List<TileRecord> firstTileRecords;
	[HideInInspector]public List<TileRecord> goodTileRecords;
	[HideInInspector]public List<TileRecord> roomTiles;
	[HideInInspector]public List<GameObject> enemies;
	TileRecord secRoom;

	[Header("Enemy Stuff")]
	public bool spawnEnemies = true;
	[Tooltip("This is chances out of 10, and is run per room.")] public float likelihoodOfEnemies = 5;
	public GameObject enemyPrefab;
	[Tooltip("out of 100")] public float lootDropChance = 50;
	public GameObject medkit;
	public GameObject syringe;
	public GameObject ammo;

	[HideInInspector] public int noOfEnemies = 0;


	void Start () 
	{
		SetUpLevel ();
	}

	void SetUpLevel()
	{
		StepOne ();
		StepTwo ();
		StepThree ();
		StepFour ();
		StepFive (); //TODO: WebGL builds are breaking when I call step 7 or try to do anything with it, sometimes step 6, sometimes even 5
		StepSix (); 
		//StepSeven (); //called from StepSix
		//SpawnEnemies (); //called from StepSeven
	}

	void StepOne()
	{
		//find the centre tile and put the Elevator there
		centreX = Mathf.CeilToInt ((float)mapSizeX / 2);
		centreY = Mathf.CeilToInt ((float)mapSizeX / 2);
		centrePoint = TilePos (centreX, centreY);
		string elevName = "ELEVATOR ROOM";

		SpawnTheRoom(elevName, centreX, centreY);

		//put the player in it
		GameObject.FindGameObjectWithTag ("Player").transform.position = centrePoint + new Vector2(0, 2);

		//get all tiles with even coords
		for (int x = 1; x <= mapSizeX; x++)
		{
			for(int y = 1; y <= mapSizeY; y++)
			{
				if((x % 2 == 0) && y % 2 == 0) //then we've found our even number grids
				{
					// Pick a grid type and NSEW access. Can be room or corridor or null. All Grids. Don't allow "storage room" corridors
					string tileName = ChooseRoomType(false, false, x, y);

					//now check the generated name against all in the list, and spawn that one
					SpawnTheRoom(tileName, x, y);
				}
			}
		}

	}

	void StepTwo()
	{
		//next get all the odd tiles, which are diagonally in between all those others.
		for (int x = 1; x <= mapSizeX; x++)
		{
			for(int y = 1; y <= mapSizeY; y++)
			{
				if((x % 2 == 1) && y % 2 == 1) //then we've found our odd number grids
				{
					//must exclude all outer edges in this step, plus exclude the centre elevator
					if(x != 1 && x != mapSizeX && y != 1 && y != mapSizeY && !(x == centreX && y == centreY))
					{
						// Pick a grid type and NSEW access. Can be room or corridor or null. All Grids. Don't allow "storage room" corridors
						string tileName = ChooseRoomType(false, false, x, y);
						
						//now check the generated name against all in the list, and spawn that one
						SpawnTheRoom(tileName, x, y);
					}
				}
			}
		}
	}

	void StepThree()
	{
		//next we spawn the diagonals on the outsides of the grid. care has to be taken that no rooms face out.
		for (int x = 1; x <= mapSizeX; x++)
		{
			for(int y = 1; y <= mapSizeY; y++)
			{
				if((x % 2 == 1) && y % 2 == 1) //then we've found our odd number grids
				{

					// This time we only want to spawn on the outsides
					if(x == 1 || x == mapSizeX || y == 1 || y == mapSizeY)
					{
						//we only want to spawn grids that face inwards.
						// Pick a grid type and NSEW access. Can be room or corridor or null. Inward Grids. Don't allow "storage room" corridors
						string tileName = ChooseRoomType(false, true, x, y);

						//now check the generated name against all in the list, and spawn that one
						SpawnTheRoom(tileName, x, y);
					}
				}
			}
		}
	}

	void StepFour()
	{
		//lastly we spawn corridors only, in between all other tiles.
		for (int x = 1; x <= mapSizeX; x++)
		{
			for(int y = 1; y <= mapSizeY; y++)
			{
				//these remaining squares are always (odd, even) or (even, odd)
				if((x % 2 == 1 && y % 2 == 0) || (x %2 == 0 && y % 2 == 1)) //then we've found our grids
				{
					//HAS to be a corridor, needs to connect to what's around it (nsew, but in snwe order), and outer ones can't face outwards.
					string tileName = "C";

					if(y != mapSizeY) //if we're not at the top, check do we have to connect North
					{
						if(WhatRoomIsHere(x, y+1).Contains("S"))
							tileName += "N";
					}
					if(y != 1)
					{
						if(WhatRoomIsHere(x, y-1).Contains("N"))
							tileName += "S";

					}
					if(x != mapSizeX)
					{
						if(WhatRoomIsHere(x+1, y).Contains("W"))
							tileName += "E";
					}
					if(x != 1)
					{
						if(WhatRoomIsHere(x - 1, y).Contains("E"))
							tileName += "W";
					}

					//now check the generated name against all in the list, and spawn that one
					SpawnTheRoom(tileName, x, y);
				}
			}
		}

	}

	void StepFive () //deleting the rooms that are inaccessible
	{
		//set up the pathfinding first time
		pathfinderScript.Scan ();

		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;

		GraphNode a = AstarPath.active.GetNearest(player.position, NNConstraint.Default).node;

		foreach(TileRecord rec in firstTileRecords) //check if each room can be reached by the player
		{
			GraphNode b = AstarPath.active.GetNearest(rec.gridObject.transform.position, NNConstraint.Default).node;

			//single room corridors gave problems because their centre point wasn't accessible. This is getting specific
			if(rec.gridObject.name == "CN")
				b = AstarPath.active.GetNearest(rec.gridObject.transform.position + new Vector3(0,3,0), NNConstraint.Default).node;
			if(rec.gridObject.name == "CS")
				b = AstarPath.active.GetNearest(rec.gridObject.transform.position + new Vector3(0,-3,0), NNConstraint.Default).node;
			if(rec.gridObject.name == "CE")
				b = AstarPath.active.GetNearest(rec.gridObject.transform.position + new Vector3(3,0,0), NNConstraint.Default).node;
			if(rec.gridObject.name == "CW")
				b = AstarPath.active.GetNearest(rec.gridObject.transform.position + new Vector3(-3,0,0), NNConstraint.Default).node;

			if (Pathfinding.PathUtilities.IsPathPossible (a, b)) //if yes, add to a new list of good rooms and spawn fog there (room as parent);
			{
				GameObject fog = Instantiate(fogPrefab, rec.gridObject.transform.position, Quaternion.identity)as GameObject;
				fog.transform.SetParent(rec.gridObject.transform);
				goodTileRecords.Add (rec);
			} 
			else
			{
				//put down fog (World as parent this time), set its parent to World, delete bad room
				GameObject fog = Instantiate(fogPrefab, rec.gridObject.transform.position, Quaternion.identity)as GameObject;
				fog.layer = LayerMask.NameToLayer("Obstacle");
				fog.transform.SetParent(emptyWorldHolder.transform);
				fog.SendMessage ("CantTurnOffFog");
				Destroy(rec.gridObject);
			}
		}
	}


	void StepSix()
	{
		//decorate the rooms and redo pathfinding

		//then populate all other rooms
		foreach(TileRecord tile in goodTileRecords)
		{
			if(tile.name.ToCharArray()[0].ToString() == "R")
			{
				int rotation = Random.Range(0,4) * 90;
				GameObject prefab = Instantiate(allFurniture[Random.Range(0, allFurniture.Length)],
				            tile.gridObject.transform.position, Quaternion.Euler(0,0, rotation))as GameObject;
				prefab.transform.SetParent(tile.gridObject.transform);
				tile.furniture = prefab;

				if(tile.name != "NSEW XXX")
					roomTiles.Add(tile);
			}
		}
		//Spawn Security Room

		secRoom = roomTiles [Random.Range (0, roomTiles.Count)];
		Destroy (secRoom.furniture);
		int rot = Random.Range(0,4) * 90;
		GameObject pref = Instantiate(securityRoomPrefab, secRoom.gridObject.transform.position, Quaternion.Euler(0,0, rot))as GameObject;
		pref.transform.SetParent(secRoom.gridObject.transform);
		secRoom.furniture = pref;

		//lastly scan it all into the world pathfinding
		Invoke ("StepSeven", 0.21f);
	}

	void StepSeven() 
	{
		pathfinderScript.Scan ();
		Invoke("SpawnEnemies", 0.05f);
	}


	void SpawnEnemies()
	{
		if (!spawnEnemies)
			return;

		foreach(TileRecord tile in goodTileRecords)
		{
			if(tile.name.ToCharArray()[0].ToString() == "R")
			{
				int i = Random.Range(0,11);

				if(i <= likelihoodOfEnemies)
				{
					noOfEnemies ++;
					GameObject enemy = Instantiate(enemyPrefab, tile.gridObject.transform.position, Quaternion.identity) as GameObject;
					enemy.name = "Enemy " + noOfEnemies;
					GiveLoot(enemy);
					enemies.Add(enemy);
				}
			}

		}
		if(enemies.Count == 0)
		{
			Debug.Log("Spawned 0 enemies on first attempt");
			SpawnEnemies();
			return;
		}
		Invoke ("GiveKeycard", 1);
	}

	void GiveLoot(GameObject enemy)
	{
		float diceRoll = Random.Range(0,100);
		if (diceRoll > lootDropChance)
			return;
		
		enemy.GetComponent<Health>().myLoot.Add(medkit);
	}


	void GiveKeycard()
	{
		GameObject keyholder = secRoom.furniture.transform.FindChild("enemy").FindChild("Enemy").gameObject;
		keyholder.GetComponent<Health>().myLoot.Add(keyPrefab);
	}


	Vector2 TilePos(int x, int y)
	{
		return (new Vector2 (x * gridSize, y * gridSize));
	}


	string WhatRoomIsHere(int x, int y)
	{
		string answer = "";

		foreach(TileRecord rec in firstTileRecords)
		{
			if(rec.coords == x + "," + y)
			{
				answer = rec.name;
				break;
			}
		}
		return answer;
	}


	string ChooseRoomType(bool wantStorageRooms, bool mustFaceInwards, int x, int y)
	{
		string tileName;

		//first decide room or corridor
		if(Random.value > 0.1f)
			tileName = "R";
		else
			tileName = "C";

		if(!mustFaceInwards)
		{
			//then let it choose from all directions, one after another
			char[] nsew = "NSEW".ToCharArray();
			
			foreach(char c in nsew)
			{
				if (Random.value > 0.5f)
				{
					tileName += c;
				}
			}
		}
		else
		{
			//go through all permutations of top, bottom, left, right, including corners, to specify allowed directions
			char [] nsew = "".ToCharArray();

			//4 corners first
			if(x == 1 && y == 1)
			{
				nsew = "NE".ToCharArray();
			}
			else if(x == 1 && y == mapSizeY)
			{
				nsew = "SE".ToCharArray();
			}
			else if(x == mapSizeX && y == 1)
			{
				nsew = "NW".ToCharArray();
			}
			else if(x == mapSizeX && y == mapSizeY)
			{
				nsew = "SW".ToCharArray();
			}

			//then 4 edges
			else if(x == 1)
			{
				nsew = "NSE".ToCharArray();
			}
			else if(x == mapSizeX)
			{
				nsew = "NSW".ToCharArray();
			}
			else if(y == 1)
			{
				nsew = "NEW".ToCharArray();
			}
			else if(y == mapSizeY)
			{
				nsew = "SEW".ToCharArray();
			}

			//then look at the allowed directions and decide on one. 
			foreach(char c in nsew)
			{
				if (Random.value > 0.5f)
				{
					tileName += c;
				}
			}
		}

		//check to see did we wind up with a storage room (CN, CS, CE, or CW) because maybe we don't want those
		if(wantStorageRooms)
		{
			//then it's fine as it was
			return tileName;
		}
		else
		{
			if(tileName.ToCharArray().Length == 2 && tileName.ToCharArray()[0].ToString() == "C")
			{
				//then run this again to try not get a storage room
				return ChooseRoomType(wantStorageRooms, mustFaceInwards, x, y);
			}
			else
				return tileName; //it was fine anyway
		}
	}


	void SpawnTheRoom(string roomName, int x, int y)
	{

		foreach(GameObject grid in allGrids)
		{
			if(grid.name == roomName)
			{
				GameObject obj = Instantiate(grid , TilePos(x, y), Quaternion.identity) as GameObject;
				obj.name = roomName;
				obj.transform.SetParent(emptyWorldHolder.transform);

				if(obj.name == "ELEVATOR ROOM")
				{
					roomName = "NSEW XXX";
				}

				//TODO: spawn a fog of war black cover to every room. Layer Aerial -1, with a collider to turn off when you're in the room
				TileRecord record = new TileRecord();
				record.coords = x + "," + y;
				record.name = roomName;
				record.gridObject = obj;
				firstTileRecords.Add (record);
				break;
			}
		}
	}
}
