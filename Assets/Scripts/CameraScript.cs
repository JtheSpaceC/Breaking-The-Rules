using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public Transform player;
	public Transform cursor;
	public GameObject laser;
	public GameObject map;
	public GameObject leftArm;
	public GameObject healthPanel;
	public GameObject ammoPanel;
	public bool alwaysOnPlayer = false;
	Vector3 offset;
	public float camSize;
	
	bool zoomed = false;
	[HideInInspector] public bool canShowMap = false;
	
	// Use this for initialization
	void Awake () {
		offset = transform.position - player.transform.position;
		camSize = Camera.main.orthographicSize;
	}
	
	void Start()
	{
		map.SetActive(false);
		zoomed = !zoomed;
		leftArm.SetActive(!leftArm.activeSelf);
		healthPanel.SetActive(!healthPanel.activeSelf);
		laser.SetActive(!laser.activeSelf);
	}
	
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Q) && canShowMap)
		{
			ToggleMap();
		}
		
		if(zoomed && Camera.main.orthographicSize != 1)
			Camera.main.orthographicSize = (Mathf.Lerp(Camera.main.orthographicSize,1, 0.5f));
		else if(!zoomed && Camera.main.orthographicSize != camSize)
			Camera.main.orthographicSize = (Mathf.Lerp(Camera.main.orthographicSize,camSize, 0.5f));
		
	}
	
	void LateUpdate () 
	{
		if (!alwaysOnPlayer)
			transform.position = (player.position + cursor.position) / 2 + offset;
		else
			transform.position = player.position + offset;
	}
	
	public void ToggleMap()
	{
		zoomed = !zoomed;
		map.SetActive(!map.activeSelf);
		leftArm.SetActive(!leftArm.activeSelf);
		healthPanel.SetActive(!healthPanel.activeSelf);
		ammoPanel.SetActive(!ammoPanel.activeSelf);
		laser.SetActive(!laser.activeSelf);
	}
	
	public void SwitchCanShowMap()
	{
		canShowMap = !canShowMap;
	}
}
