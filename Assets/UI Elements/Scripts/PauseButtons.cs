using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtons : MonoBehaviour
{
	public GameObject settingsUI;
	public CanvasGroup darkener;
	private PauseMenu pause;

	private void Start()
	{
		pause = GetComponent<PauseMenu>();
	}

	public void Continue()
	{
		pause.ContinueGame();
	}

	public void OpenSettings()
	{
		settingsUI.SetActive(true);
		StartCoroutine(DarkenBackground());
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private IEnumerator DarkenBackground()
	{
		while(darkener.alpha < 0.9f)
		{
			darkener.alpha = Mathf.MoveTowards(darkener.alpha, 0.9f, Time.fixedDeltaTime * 5);
			yield return null;
		}
	}
}
