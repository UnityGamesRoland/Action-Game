using UnityEngine;

public class WeaponSway : MonoBehaviour
{
	public float moveAmount = 0.05f;
	public float moveSpeed = 1f;
	public float tiltAmount = 3f;
	public float tiltSpeed = 4.5f;
	public Vector3 aimPos;

	private WeaponPistol pistol;
	private Vector3 defaultPosition;
	private Vector3 defaultRotation;

	private void Start()
	{
		if(aimPos != Vector3.zero) pistol = transform.parent.GetComponent<WeaponPistol>();

		defaultPosition = transform.localPosition;
		defaultRotation = transform.localEulerAngles;
	}

	private void Update()
	{
		float tiltX = Input.GetAxis("Mouse X") * tiltAmount;
		float tiltY = Input.GetAxis("Mouse Y") * tiltAmount;

		float moveX = -Input.GetAxis("Mouse X") * moveAmount;
		float moveY = -Input.GetAxis("Mouse Y") * moveAmount;

		moveX = Mathf.Clamp(moveX, -0.02f, 0.02f);
		moveY = Mathf.Clamp(moveY, -0.025f, 0.025f);

		Quaternion targetRotation = Quaternion.Euler(defaultRotation.x + tiltX, defaultRotation.y, defaultRotation.z + tiltY);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);

		Vector3 activePosition = GetTargetPosition();

		if(activePosition == defaultPosition)
		{
			Vector3 targetPosition = new Vector3(activePosition.x + moveX, activePosition.y + moveY, activePosition.z);
			transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
		}

		else
		{
			Vector3 targetPosition = new Vector3(activePosition.x + moveX/3, activePosition.y + moveY/3, activePosition.z);
			transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
		}
	}

	private Vector3 GetTargetPosition()
	{
		if(pistol != null && pistol.isAiming) return aimPos;
		else return defaultPosition;
	}
}
