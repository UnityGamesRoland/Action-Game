using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloser : MonoBehaviour
{
	public bool affectTime;
	public bool affectCursor;
	private CanvasGroup canvasGroup;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}

	private void Update()
	{
		if(canvasGroup.alpha > 0)
		{
			if(affectTime)
			{
				Time.timeScale = 0;
			}

			if(affectCursor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}

			if(Input.GetKeyDown(KeyCode.Space)) canvasGroup.alpha = 0;
		}

		if(canvasGroup.alpha == 0)
		{
			if(affectTime)
			{
				Time.timeScale = 1;
			}

			if(affectCursor)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}
}
