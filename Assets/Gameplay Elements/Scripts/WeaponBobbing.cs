using UnityEngine;
using System.Collections;

public class WeaponBobbing : MonoBehaviour
{
	public float walkBobbingSpeed = 6f;
	public float runBobbingSpeed = 8.5f;
	public float crouchBobbingSpeed = 3.5f;
	public Vector2 walkBobAmount = new Vector2(0.05f, 0.1f);
	public Vector2 runBobAmount = new Vector2(0.12f, 0.18f);
	public Vector2 crouchBobAmount = new Vector2(0.15f, 0.15f);

	private float translateX;
	private float translateY;
	private float dropTarget;
	private float dropProgress;
	private float dropVelocity;
	private float bobStrengthFactor;
	private float bobTimer;
	private float bobCycleSpeed;
	private float currentHeight;
	private Vector2 currentBobAmount;

	private PlayerMotorBehavior motor;
	private CharacterController controller;
	private Vector3 startPoint;

	private void Start()
	{
		//Initialization.
		motor = FindObjectOfType<PlayerMotorBehavior>();
		controller = FindObjectOfType<CharacterController>();
		startPoint = transform.localPosition;
	}

	private void Update()
	{
		//Calculate the delta of the head bobbing.
		float delta = Mathf.Clamp01(Time.deltaTime * 6f);

		//Check if we are grounded and moving.
		if(motor.isGrounded && motor.isMoving)
		{
			//Switch to running cycle speed.
			if(motor.isRunning)
			{
				bobCycleSpeed = runBobbingSpeed;
				currentBobAmount = Vector2.LerpUnclamped(currentBobAmount, runBobAmount, delta);
			}

			//Switch to crouching cycle speed.
			else if(motor.isCrouching)
			{
				bobCycleSpeed = crouchBobbingSpeed;
				currentBobAmount = Vector2.LerpUnclamped(currentBobAmount, crouchBobAmount, delta);
			}

			//Switch to walking cycle speed.
			else
			{
				bobCycleSpeed = walkBobbingSpeed;
				currentBobAmount = Vector2.LerpUnclamped(currentBobAmount, walkBobAmount, delta);
			}

			//Update the timer of the head bobbing.
			bobTimer += bobCycleSpeed * Time.deltaTime;
			if(bobTimer >= Mathf.PI * 2f) bobTimer -= Mathf.PI * 2f;

			//Calculate the head bobbing motion.
			bobStrengthFactor = Mathf.Clamp01(controller.velocity.magnitude / 2.5f);
			translateX = Mathf.Sin(bobTimer) * currentBobAmount.x * bobStrengthFactor;
			translateY = Mathf.Cos(bobTimer * 2f) * currentBobAmount.y * bobStrengthFactor;
		}

		//We are not grounded or not moving, reset the head bobbing.
		else
		{
			//Reset the head bobbing motion.
			bobTimer = Mathf.LerpUnclamped(bobTimer, 0f, delta);
			bobStrengthFactor = Mathf.LerpUnclamped(bobStrengthFactor, 0f, delta);
			translateX = Mathf.LerpUnclamped(translateX, 0f, delta);
			translateY = Mathf.LerpUnclamped(translateY, 0f, delta);
		}

		float targetHeight = motor.isGrounded ? 0f : 0.02f;
		currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 7f);

		//Apply the new position and rotation on the head.
		transform.localPosition = new Vector3(startPoint.x + translateX * 0.04f, startPoint.y - currentHeight + translateY * 0.1f, startPoint.z);
	}
}
