using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{
	public int loadIndex;
	public float fadeSpeed = 0.5f;
	public CanvasGroup fadeUI;

	private void Start()
	{
		fadeUI.alpha = 1;
		StartCoroutine(StartFadingOut());
	}

	public void EndLevel()
	{
		StartCoroutine(StartFadingIn());
	}

	IEnumerator StartFadingOut()
	{
		while(fadeUI.alpha > 0)
		{
			fadeUI.alpha = Mathf.MoveTowards(fadeUI.alpha, 0, Time.deltaTime * fadeSpeed);
			yield return null;
		}
	}

	IEnumerator StartFadingIn()
	{
		fadeUI.GetComponent<RawImage>().color = Color.white;

		while(fadeUI.alpha < 1)
		{
			fadeUI.alpha = Mathf.MoveTowards(fadeUI.alpha, 1, Time.deltaTime * fadeSpeed);
			yield return null;
		}

		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(loadIndex);
	}
}
