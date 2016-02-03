using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RPGEndOfLevel : MonoBehaviour {

	public Text statsText;
	public Text statsTotalsText;

	public Text damageText;
	public Text accuracyText;
	public Text maxHealthText;
	public Text runningSpeedText;
	public Text reloadSpeedText;

	float tempDamageStat;
	float tempAccuracyStat;
	float tempMaxHealthStat;
	float tempRunningSpeedStat;
	float tempReloadSpeed;

	public GameObject continueButton;

	int totalXPLoss;

	bool recordedInitialStats = false;


	void Start()
	{
		statsText.text = 
			"Enemy Shots Fired = " + RPGelements.instance.shotsFired + "\n" +
			"Damage Taken = " + (int)RPGelements.instance.damageTaken + "\n" +
			"Adrenaline Found = " + RPGelements.instance.adrenalineFound + "\n" +
			"_______________________\n" +
			"Total...\n" +
			"XP Loss";

		int total = (RPGelements.instance.shotsFired * 1) + (int)(RPGelements.instance.damageTaken * 2) 
			- (int)(RPGelements.instance.adrenalineFound * 1);

		totalXPLoss = (int)total / 10;
		if (totalXPLoss < 0)
			totalXPLoss = 0;

		statsTotalsText.text =
			"-" + RPGelements.instance.shotsFired * 1 + "\n" + 
			"-" + (int)RPGelements.instance.damageTaken * 2 + "\n" + 
			"+" + RPGelements.instance.adrenalineFound + "\n" + 
			"___\n" +
			(total * -1) + "\n" +
			(totalXPLoss * -1) + " XP";

		UpdateTexts ();
		DeactivateIneligibleButtons (totalXPLoss);

		if(totalXPLoss == 0)
		{
			continueButton.SetActive(true);
			DeactivateIneligibleButtons (999);
		}
	}


	public void UpdateTexts()
	{
		damageText.text = "Damage (" + RPGelements.instance.damageStat + ")";
		accuracyText.text = "Accuracy (" + RPGelements.instance.accuracyStat + ")";
		maxHealthText.text = "Max Health (" + RPGelements.instance.maxHealthStat + ")";
		runningSpeedText.text = "Running Speed (" + RPGelements.instance.runningSpeedStat + ")";
		reloadSpeedText.text = "Reload Speed (" + RPGelements.instance.reloadSpeedStat + ")";
	}


	void DeactivateIneligibleButtons(int xpLoss)
	{
		if (xpLoss > RPGelements.instance.accuracyStat)
			accuracyText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.instance.damageStat)
			damageText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.instance.maxHealthStat)
			maxHealthText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.instance.runningSpeedStat)
			runningSpeedText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.instance.reloadSpeedStat)
			reloadSpeedText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
	}

	public void ReduceAccuracy()
	{
		DoInitialCheck();
		RPGelements.instance.accuracyStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		accuracyText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
	public void ReduceDamage()
	{
		DoInitialCheck();
		RPGelements.instance.damageStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		damageText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}
	public void ReduceMaxHealth()
	{
		DoInitialCheck();
		RPGelements.instance.maxHealthStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		maxHealthText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}
	public void ReduceRunningSpeed()
	{
		DoInitialCheck();
		RPGelements.instance.runningSpeedStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		runningSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}
	public void ReduceReloadSpeed()
	{
		DoInitialCheck();
		RPGelements.instance.reloadSpeedStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		reloadSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}


	void RecordStartingStats()
	{
		tempDamageStat = RPGelements.instance.damageStat;
		tempAccuracyStat = RPGelements.instance.accuracyStat;
		tempMaxHealthStat = RPGelements.instance.maxHealthStat;
		tempRunningSpeedStat = RPGelements.instance.runningSpeedStat;
		tempReloadSpeed = RPGelements.instance.reloadSpeedStat;

		recordedInitialStats = true;
	}

	void ResetToStartingStats()
	{
		RPGelements.instance.damageStat = tempDamageStat;
		RPGelements.instance.accuracyStat = tempAccuracyStat;
		RPGelements.instance.maxHealthStat = tempMaxHealthStat;
		RPGelements.instance.runningSpeedStat = tempRunningSpeedStat;
		RPGelements.instance.reloadSpeedStat = tempReloadSpeed;

		accuracyText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = false;
		damageText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = false;
		maxHealthText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = false;
		runningSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = false;
		reloadSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = false;
	}

	void DoInitialCheck()
	{
		if(!recordedInitialStats)
		{
			RecordStartingStats();
		}
		else if(recordedInitialStats)
		{
			ResetToStartingStats();
		}
	}

	void FinishOff()
	{
		continueButton.SetActive(true);
		UpdateTexts ();
	}
}
