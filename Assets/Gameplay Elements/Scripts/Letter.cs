using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour
{
	public CanvasGroup letterUI;
	public float waitTimeBeforeSlide = 3;
	public bool isReading;

	private PlayerMotorBehavior motor;
	private PlayerCameraBehavior cam;
	private Vector3 slideInPosition;

	private void Start()
	{
		motor = FindObjectOfType<PlayerMotorBehavior>();
		cam = FindObjectOfType<PlayerCameraBehavior>();
		letterUI.alpha = 0;

		slideInPosition = transform.position;
		StartCoroutine(SlideToPos());
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) && isReading) CloseLetter();
	}

	private IEnumerator SlideToPos()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, 5.5f);
		yield return new WaitForSeconds(waitTimeBeforeSlide);

		while(transform.position != slideInPosition)
		{
			transform.position = Vector3.Lerp(transform.position, slideInPosition, Time.deltaTime * 5);
			yield return null;
		}
	}

	public void OpenLetter()
	{
		letterUI.alpha = 1;
		Time.timeScale = 0;

		motor.canMove = false;
		cam.allowRotation = false;
		isReading = true;
	}

	public void CloseLetter()
	{
		letterUI.alpha = 0;
		Time.timeScale = 1;

		motor.canMove = true;
		cam.allowRotation = true;
		isReading = false;
		FindObjectOfType<SmokeManager>().ReleaseSmoke();

		Destroy(gameObject);
	}
}
