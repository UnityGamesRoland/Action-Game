using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeManager : MonoBehaviour
{
	public float targetFogDensity;
	public ParticleSystem[] smokes;

	private void Start()
	{
		foreach(ParticleSystem smoke in smokes)
		{
			smoke.Stop();
		}
	}

	public void ReleaseSmoke()
	{
		foreach(ParticleSystem smoke in smokes)
		{
			smoke.Play();
		}

		StartCoroutine(FillRoomWithSmoke());
	}

	private IEnumerator FillRoomWithSmoke()
	{
		yield return new WaitForSeconds(4);

		while(RenderSettings.fogDensity < targetFogDensity)
		{
			RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime * 0.01f);
			yield return null;
		}

		FindObjectOfType<LevelSetup>().EndLevel();
	}
}
