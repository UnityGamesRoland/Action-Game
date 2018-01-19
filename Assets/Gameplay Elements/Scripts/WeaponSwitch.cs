using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
	public GameObject meleeWeapon;
	public GameObject pistolWeapon;
	public bool hasMelee;
	public bool hasPistol;
	public int pistolAmmo;

	[HideInInspector]
	public float actionTimer;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			EquipItem("katana");
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			EquipItem("pistol");
		}
	}

	public void EquipItem(string id)
	{
		if(Time.time < actionTimer) return;

		if(id == "katana" && hasMelee)
		{
			meleeWeapon.SetActive(true);
			pistolWeapon.SetActive(false);

			meleeWeapon.GetComponent<WeaponMelee>().swingTimer = Time.time + 0.25f;
			actionTimer = Time.time + 0.25f;
		}

		else if(id == "pistol" && hasPistol)
		{
			meleeWeapon.SetActive(false);
			pistolWeapon.SetActive(true);

			WeaponPistol pistol = pistolWeapon.GetComponent<WeaponPistol>();
			pistol.ammoText.text = pistol.ammoInClip + "/" + pistolAmmo;

			pistol.shootTimer = Time.time + 0.385f;
			actionTimer = Time.time + 0.385f;
		}
	}
}
