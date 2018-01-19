using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
	[HideInInspector] public Vector3 targetPos;

	private float distance;
	private float counter;
	private float velocity;

	private LineRenderer trail;

	private void Start()
	{
		trail = GetComponent<LineRenderer>();
		trail.SetPosition(0, transform.position);

		distance = Vector3.Distance(transform.position, targetPos);

		StartCoroutine(FadeTrail());
		Destroy(gameObject, 4);
	}

	private void Update()
	{
		if(counter < distance)
		{
			counter = Mathf.SmoothDamp(counter, distance, ref velocity, (distance <= 7) ? 0.1f : 0.2f);

			float x = Mathf.Lerp(0, distance, counter);
			Vector3 pointAlongLine = x * Vector3.Normalize(targetPos - transform.position) + transform.position;

			trail.SetPosition(1, pointAlongLine);
		}
	}

	private IEnumerator FadeTrail()
	{
		Gradient fadedGradient = trail.colorGradient;
		GradientAlphaKey[] alphaKeys = fadedGradient.alphaKeys;

		yield return new WaitForSeconds(1.2f);

		while(true)
		{
			for(int i = 0; i < alphaKeys.Length; i++)
			{
				alphaKeys[i].alpha = Mathf.Lerp(alphaKeys[i].alpha, 0, Time.deltaTime * 0.6f);
			}

			fadedGradient.SetKeys(fadedGradient.colorKeys, alphaKeys);
			trail.colorGradient = fadedGradient;

			yield return null;
		}
	}
}
