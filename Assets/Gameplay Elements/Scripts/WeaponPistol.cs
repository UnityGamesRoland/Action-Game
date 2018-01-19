using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponPistol : MonoBehaviour
{
	public int damage = 100;
	public float spread = 3;
	public float reloadTime = 1f;
	public int ammoInClip = 7;
	public Transform trailStartPos;
	public Transform trailPrefab;
	public Transform metalHitPrefab;
	public ParticleSystem cartridgeParticle;
	public AudioClip[] shootSounds;
	public Text ammoText;
	public bool isAiming;

	[HideInInspector] public CanvasGroup ammoGroup;
	[HideInInspector] public float shootTimer;

	private Animator animator;
	private PlayerCameraBehavior cam;
	private WeaponSwitch weaponSwitch;
	private PauseMenu menu;

	private Vector2 spreadRadius;
	private bool isReloading;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		cam = FindObjectOfType<PlayerCameraBehavior>();
		weaponSwitch = FindObjectOfType<WeaponSwitch>();
		ammoGroup = ammoText.transform.GetComponent<CanvasGroup>();
		menu = FindObjectOfType<PauseMenu>();

		shootTimer = Time.time + 0.385f;
		weaponSwitch.actionTimer = shootTimer;
	}

	private void OnEnable()
	{
		if(ammoGroup != null) ammoGroup.alpha = 1f;
	}

	private void OnDisable()
	{
		if(ammoGroup != null) ammoGroup.alpha = 0f;
		if(cam != null) cam.playerCamera.fieldOfView = 65;
	}

	private void Update()
	{
		if(!menu.isPaused)
		{
			if(Input.GetMouseButtonDown(0) && Time.time > shootTimer && !isReloading && ammoInClip > 0)
			{
				Shoot();
			}

			if(Input.GetKeyDown(KeyCode.R) && Time.time > shootTimer && !isReloading && ammoInClip < 7 && weaponSwitch.pistolAmmo > 0)
			{
				StartCoroutine(Reload());
			}
		}

		if(Input.GetMouseButton(1) && !isReloading && !menu.isPaused)
		{
			isAiming = true;
			cam.playerCamera.fieldOfView = Mathf.Lerp(cam.playerCamera.fieldOfView, 50, Time.deltaTime * 7);
		}

		else
		{
			isAiming = false;
			cam.playerCamera.fieldOfView = Mathf.Lerp(cam.playerCamera.fieldOfView, 65, Time.deltaTime * 7);
		}
	}

	private void Shoot()
	{
		cam.RecoilCamera(Random.Range(-1.4f, 1.4f), -2.7f, 0.13f);
		animator.Play("deagle_shoot");

		cartridgeParticle.Play();

		ammoInClip--;
		ammoText.text = ammoInClip + "/" + weaponSwitch.pistolAmmo;

		spreadRadius = new Vector2(Random.Range(-spread, spread), Random.Range(-spread, spread));

		Ray ray = cam.playerCamera.ScreenPointToRay(new Vector3(cam.playerCamera.pixelWidth/2, cam.playerCamera.pixelHeight/2, 0) + new Vector3(spreadRadius.x, spreadRadius.y, 0));
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100f))
		{
			DrawTrail(hit.point);

			if(hit.transform.CompareTag("Enemy_Head"))
			{
				hit.transform.root.SendMessage("ApplyDamage", damage * 2);
			}

			else if(hit.transform.CompareTag("Enemy"))
			{
				hit.transform.root.SendMessage("ApplyDamage", damage);
			}

			else if(hit.transform.CompareTag("Breakable"))
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

		else DrawTrail(cam.playerCamera.transform.position + cam.playerCamera.transform.forward * 40);

		shootTimer = Time.time + 0.2f;
		weaponSwitch.actionTimer = shootTimer;
	}

	private IEnumerator Reload()
	{
		isReloading = true;
		animator.CrossFade("deagle_reload", 0.05f);
		weaponSwitch.actionTimer = Time.time + reloadTime;
		yield return new WaitForSeconds(reloadTime);

		int ammoToLoad = 7 - ammoInClip;
		if(weaponSwitch.pistolAmmo <= ammoToLoad)
		{
			ammoInClip += weaponSwitch.pistolAmmo;
			weaponSwitch.pistolAmmo = 0;
		}

		else
		{
			ammoInClip += ammoToLoad;
			weaponSwitch.pistolAmmo -= ammoToLoad;
		}

		ammoText.text = ammoInClip + "/" + weaponSwitch.pistolAmmo;
		isReloading = false;
	}

	private void DrawTrail(Vector3 targetPos)
	{
		Transform trail = Instantiate(trailPrefab, trailStartPos.position, Quaternion.identity) as Transform;
		trail.GetComponent<WeaponTrail>().targetPos = targetPos;

		int randomSound = Random.Range(0, shootSounds.Length);
		trail.GetComponent<AudioSource>().PlayOneShot(shootSounds[randomSound]);
	}
}
