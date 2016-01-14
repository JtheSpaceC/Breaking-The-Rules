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

	public GameObject continueButton;

	int totalXPLoss;


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
		if (total < 0)
			total = 0;
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
		print("Deactivating buttons");
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
		RPGelements.rpgElements.accuracyStat -= totalXPLoss;
		DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		accuracyText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
	public void ReduceDamage()
	{
		RPGelements.rpgElements.damageStat -= totalXPLoss;
		DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		damageText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
	public void ReduceMaxHealth()
	{
		RPGelements.rpgElements.maxHealthStat -= totalXPLoss;
		DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		maxHealthText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
	public void ReduceRunningSpeed()
	{
		RPGelements.rpgElements.runningSpeedStat -= totalXPLoss;
		DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		runningSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
	public void ReduceReloadSpeed()
	{
		RPGelements.rpgElements.reloadSpeedStat -= totalXPLoss;
		DeactivateIneligibleButtons (999);
		continueButton.SetActive(true);
		reloadSpeedText.transform.parent.FindChild ("Tick Image").GetComponent<Image> ().enabled = true;
		UpdateTexts ();
	}
}
