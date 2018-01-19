using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class FocusTime : MonoBehaviour
{
	public AudioMixer mixer;
	[Range(0, 100)] public float focusAmount;
	public float regenRate;
	public float decreaseRate;
	public bool isFocusing;

	private bool canFocus = true;
	private bool canRegen = true;

	private PauseMenu menu;

	private void Start()
	{
		menu = FindObjectOfType<PauseMenu>();
	}

	private void Update()
	{
		if(focusAmount < 0.5f && canFocus)
		{
			StartCoroutine(Exhausted());
		}

		if(Input.GetKey(KeyCode.Q) && focusAmount > 0 && canFocus && !menu.isPaused)
		{
			isFocusing = true;
			Time.timeScale = Mathf.Lerp(Time.timeScale, 0.2f, Time.unscaledDeltaTime * 8);
			Time.fixedDeltaTime = Time.timeScale * 0.02f;

			focusAmount -= decreaseRate * Time.unscaledDeltaTime;
		}

		else
		{
			isFocusing = false;

			if(!menu.isPaused)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.unscaledDeltaTime * 8);
				Time.fixedDeltaTime = Time.timeScale * 0.02f;
			}

			if(canRegen && !menu.isPaused)
			{
				if(focusAmount < 100)
				{
					focusAmount += regenRate * Time.unscaledDeltaTime;
					focusAmount = Mathf.Clamp(focusAmount, 0, 100);
				}
			}
		}

		if(Input.GetKeyUp(KeyCode.Q))
		{
			StopCoroutine(DelayRegen());
			StartCoroutine(DelayRegen());
		}
	}

	private void LateUpdate()
	{
		mixer.SetFloat("sfxPitch", Time.timeScale);
	}

	private IEnumerator Exhausted()
	{
		canFocus = false;
		while(focusAmount < 15) yield return null;
		canFocus = true;
	}

	private IEnumerator DelayRegen()
	{
		canRegen = false;
		yield return new WaitForSecondsRealtime(2f);
		canRegen = true;
	}
}
