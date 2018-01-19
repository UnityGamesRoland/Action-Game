using UnityEngine;

public class Item : MonoBehaviour
{
	public enum ItemType {melee, pistol, backpack, letter};
	public ItemType type;
	public string pickupText;
	public int pickupAmount;

	public void PickupItem()
	{
		if(type == ItemType.melee)
		{
			WeaponSwitch inv = FindObjectOfType<WeaponSwitch>();

			if(Time.time > inv.actionTimer)
			{
				inv.hasMelee = true;
				inv.EquipItem("katana");
				Destroy(gameObject);
			}
		}

		else if(type == ItemType.pistol)
		{
			WeaponSwitch inv = FindObjectOfType<WeaponSwitch>();

			if(Time.time > inv.actionTimer)
			{
				inv.hasPistol = true;
				inv.EquipItem("pistol");
				Destroy(gameObject);
			}
		}

		else if(type == ItemType.backpack)
		{
			WeaponSwitch inv = FindObjectOfType<WeaponSwitch>();
			WeaponPistol pistol = inv.pistolWeapon.GetComponent<WeaponPistol>();

			if(Time.time > inv.actionTimer)
			{
				inv.pistolAmmo += pickupAmount;
				if(pistol.ammoText.text != "") pistol.ammoText.text = pistol.ammoInClip + "/" + inv.pistolAmmo;
				Destroy(gameObject);
			}
		}

		else if(type == ItemType.letter)
		{
			transform.GetComponent<Letter>().OpenLetter();
		}
	}
}
