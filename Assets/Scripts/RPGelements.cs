using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RPGelements : MonoBehaviour {

	public static RPGelements rpgElements; 
	public bool persistAfterLoad = true;

	public int telumaOnLevel = 10;

	public bool hasKey = false;
	public int level = 1;

	public int shotsFired = 0;
	public float damageTaken = 0;
	public float adrenalineFound = 0;
	public float endingHealth;
	public int startingAmmo = 90;
	[HideInInspector] public int endingAmmo;
	public int maxAmmoPlayerCanCarry = 120;

	public float accuracyStat = 100;
	public float damageStat = 100;
	public float maxHealthStat = 100;
	public float runningSpeedStat = 100;
	public float reloadSpeedStat = 100;

	public float reloadTime = 1;
	public float runningSpeed = 5;
	public float walkingSpeed = 2.5f;
	public float damage = 30;

	public int retries = 0;

	Text briefingText;


	void Awake()
	{
		if (persistAfterLoad && rpgElements == null)
		{
			rpgElements = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(rpgElements == null && !persistAfterLoad)
		{
			rpgElements = this;
		}
		else if (rpgElements != this)
		{
			Destroy(gameObject);
			return;
		}
		endingAmmo = startingAmmo;
		RecalculateStats ();
		endingHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().maxHealth;
	}
	
	public void ResetToFirstLevel()
	{
		hasKey = false;
		level = 1;
		shotsFired = 0;
		damageTaken = 0;
		endingHealth = 100;
		endingAmmo = 90;
		reloadTime = 1;

		accuracyStat = 100;
		damageStat = 100;
		maxHealthStat = 100;
		runningSpeedStat = 100;
		reloadSpeedStat = 100;

		retries++;

		RecalculateStats ();
	}

	public void HoloText ()
	{
		briefingText = GameObject.Find("Briefing Text").GetComponent<Text>();

		int num;

		if (retries != 0)
			num = (int)Random.Range (0, 12);
		else
			num = 0;

		switch(num)
		{
		default: briefingText.text = "Something went wrong.";
			break;
		case 0: briefingText.text = "The elevator has stopped. You need a KEYCARD to take the elevator deeper into the installation.\n" +
			"Find the SECURITY ROOM and take the keycard from one of the guards. Then RETURN to the elevator.\n" +
			"Explore this sublevel and use the map (Q) to find your way.";
			break;
		case 1: briefingText.text = "MEDKITS can restore your health, but they won't relieve the stress of having been shot.\n" +
			"That's what Adrenaline SYRINGES are for!";
			break;
		case 2: briefingText.text = "Enemies' guns don't have AMMO compatible with your weapon. You can only find more ammo in Storage Cabinets.";
			break;
		case 3: briefingText.text = "After leaving a floor in the elevator, you have a moment to rest. Stress and adrenal fatigue " +
			"will start to take their toll on your abilities.";
			break;
		case 4: briefingText.text = "Use Q to show/hide your map. After entering a new room, your onboard computer will automatically sync " +
			"with the security feed from that room, allowing you to spot enemy movement and to map out where you've been.";
			break;
		case 5: briefingText.text = "The terminals in the Security Room can be hacked to reveal the map of the current floor completely.\n" +
			"This is useful for preventing enemies from getting the drop on you on your way back to the elevator.";
			break;
		case 6: briefingText.text = "The Enemy are evil. You are not.";
			break;
		case 7: briefingText.text = "If you are low on health or ammo, or if you've taken a lot of fire on the current floor, try exploring for " +
			"resources before you move on, but be careful of running into more enemies.";
			break;
		case 8: briefingText.text = "One of the security room guards will always hold the keycard you need to continue your mission.";
			break;
		case 9: briefingText.text = "Enemies may sometimes drop MEDKITS, but they don't carry ADRENALINE SYRINGES and their AMMO won't work " +
			"in your gun. However, all of these can be found in Storage Cabinets.";
			break;
		case 10: briefingText.text = "If an enemy hasn't seen you yet, you'll often be able to kill them in one shot as you have more time " +
			"to line up a headshot.";
			break;
		case 11: briefingText.text = "You can see and shoot whatever your gun's laser can reach. If the laser stops on an object like a door or closet, " +
			"this means it is tall and you can use it as cover or hide behind it.";
			break;
		}
	}

	public void RecalculateStats()
	{
		reloadTime = 10 - (9*(reloadSpeedStat/100));

		//accuracy is handled direct from Shooting to accuracyStat (above)

		runningSpeed = 2.5f + (2.5f * (runningSpeedStat / 100));
		walkingSpeed = 1.25f + (1.25f * (runningSpeedStat / 100));

		damage = 20 +  (40 * (damageStat / 100));

		maxHealthStat = 35 + (65 * maxHealthStat / 100);
	}
}
