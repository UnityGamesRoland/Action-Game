using UnityEngine;
using System.Collections;

public class WeaponMelee : MonoBehaviour
{
	public int damage = 100;
	public Transform bloodHitPrefab;
	public Transform metalHitPrefab;
	public AudioClip[] swingSounds;

	private Animator animator;
	private PlayerCameraBehavior cam;
	private WeaponSwitch weaponSwitch;
	private PauseMenu menu;

	private float chainTimer;
	public float swingTimer;
	private int swingStep;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		cam = FindObjectOfType<PlayerCameraBehavior>();
		weaponSwitch = FindObjectOfType<WeaponSwitch>();
		menu = FindObjectOfType<PauseMenu>();

		swingTimer = Time.time + 0.25f;
		weaponSwitch.actionTimer = swingTimer;
	}

	private void Update()
	{
		if(!menu.isPaused)
		{
			if(Input.GetMouseButtonDown(0) && Time.time > swingTimer)
			{
				int randomSound = Random.Range(0, swingSounds.Length);
				GetComponent<AudioSource>().PlayOneShot(swingSounds[randomSound]);

				Ray ray = new Ray(cam.playerCamera.transform.position, cam.playerCamera.transform.forward);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, 2.8f))
				{
					if(hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Enemy_Head"))
					{
						hit.transform.root.SendMessage("ApplyDamage", damage);
					}

					if(hit.transform.CompareTag("Breakable"))
					{
						hit.transform.SendMessage("ShatterObject");

						Transform metalPrefab = Instantiate(metalHitPrefab, hit.point, Quaternion.identity) as Transform;
						metalPrefab.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
					}

					else
					{
						Transform metalPrefab = Instantiate(metalHitPrefab, hit.point, Quaternion.identity) as Transform;
						metalPrefab.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
					}
				}

				if(swingStep == 0)
				{
					swingStep++;
					animator.CrossFade("katana_swing1", 0.05f);
					cam.RecoilCamera(-4f, 1.2f, 0.27f);
					chainTimer = Time.time + 0.5f;
				}

				else if(swingStep == 1)
				{
					swingStep = 0;
					animator.CrossFade("katana_swing2", 0.05f);
					cam.RecoilCamera(4f, 1.2f, 0.27f);
				}

				swingTimer = Time.time + 0.27f;
				weaponSwitch.actionTimer = swingTimer;
			}
		}

		if(Time.time > chainTimer) swingStep = 0;
	}
}
