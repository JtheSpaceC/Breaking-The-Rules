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
			"Enemy Shots Fired = " + RPGelements.rpgElements.shotsFired + "\n" +
			"Damage Taken = " + (int)RPGelements.rpgElements.damageTaken + "\n" +
			"Adrenaline Found = " + RPGelements.rpgElements.adrenalineFound + "\n" +
			"_______________________\n" +
			"Total...\n" +
			"XP Loss";

		int total = (RPGelements.rpgElements.shotsFired * 1) + (int)(RPGelements.rpgElements.damageTaken * 2) 
			- (int)(RPGelements.rpgElements.adrenalineFound * 1);

		totalXPLoss = (int)total / 10;
		if (totalXPLoss < 0)
			totalXPLoss = 0;

		statsTotalsText.text =
			"-" + RPGelements.rpgElements.shotsFired * 1 + "\n" + 
			"-" + (int)RPGelements.rpgElements.damageTaken * 2 + "\n" + 
			"+" + RPGelements.rpgElements.adrenalineFound + "\n" + 
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
		damageText.text = "Damage (" + RPGelements.rpgElements.damageStat + ")";
		accuracyText.text = "Accuracy (" + RPGelements.rpgElements.accuracyStat + ")";
		maxHealthText.text = "Max Health (" + RPGelements.rpgElements.maxHealthStat + ")";
		runningSpeedText.text = "Running Speed (" + RPGelements.rpgElements.runningSpeedStat + ")";
		reloadSpeedText.text = "Reload Speed (" + RPGelements.rpgElements.reloadSpeedStat + ")";
	}


	void DeactivateIneligibleButtons(int xpLoss)
	{
		if (xpLoss > RPGelements.rpgElements.accuracyStat)
			accuracyText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.rpgElements.damageStat)
			damageText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.rpgElements.maxHealthStat)
			maxHealthText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.rpgElements.runningSpeedStat)
			runningSpeedText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
		if (xpLoss > RPGelements.rpgElements.reloadSpeedStat)
			reloadSpeedText.transform.parent.GetComponentInChildren<Button> ().interactable = false;
	}

	public void ReduceAccuracy()
	{
		DoInitialCheck();
		RPGelements.rpgElements.accuracyStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		accuracyText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
	public void ReduceDamage()
	{
		DoInitialCheck();
		RPGelements.rpgElements.damageStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		damageText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}
	public void ReduceMaxHealth()
	{
		DoInitialCheck();
		RPGelements.rpgElements.maxHealthStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		maxHealthText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}
	public void ReduceRunningSpeed()
	{
		DoInitialCheck();
		RPGelements.rpgElements.runningSpeedStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		runningSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}
	public void ReduceReloadSpeed()
	{
		DoInitialCheck();
		RPGelements.rpgElements.reloadSpeedStat -= totalXPLoss;
		//DeactivateIneligibleButtons (999);
		reloadSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		FinishOff();
	}


	void RecordStartingStats()
	{
		tempDamageStat = RPGelements.rpgElements.damageStat;
		tempAccuracyStat = RPGelements.rpgElements.accuracyStat;
		tempMaxHealthStat = RPGelements.rpgElements.maxHealthStat;
		tempRunningSpeedStat = RPGelements.rpgElements.runningSpeedStat;
		tempReloadSpeed = RPGelements.rpgElements.reloadSpeedStat;

		recordedInitialStats = true;
	}

	void ResetToStartingStats()
	{
		RPGelements.rpgElements.damageStat = tempDamageStat;
		RPGelements.rpgElements.accuracyStat = tempAccuracyStat;
		RPGelements.rpgElements.maxHealthStat = tempMaxHealthStat;
		RPGelements.rpgElements.runningSpeedStat = tempRunningSpeedStat;
		RPGelements.rpgElements.reloadSpeedStat = tempReloadSpeed;

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
