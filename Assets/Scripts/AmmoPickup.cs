using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour {

	public AudioClip clip;
	public int ammoAmount = 25;
	
	void Start()
	{
		Invoke ("ColliderOn", 1);
	}
	
	void ColliderOn()
	{
		this.GetComponent<BoxCollider2D> ().enabled = true;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			if(other.GetComponentInChildren<Shooting>().ammoReserve >= 120)
			{
				return;
			}

			int ammo = other.GetComponentInChildren<Shooting>().ammoReserve;

			ammo += (int)(ammoAmount * Random.Range(0.5f, 2));
			if(ammo >= RPGelements.instance.maxAmmoPlayerCanCarry)
				ammo = RPGelements.instance.maxAmmoPlayerCanCarry;

			other.GetComponentInChildren<Shooting>().ammoReserve = ammo;
			other.GetComponentInChildren<Shooting>().UpdateAmmoText();
			AudioSource.PlayClipAtPoint(clip, transform.position);

			Destroy(gameObject);
		}
	}
}
