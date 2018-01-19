using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject settingsUI;
	public GameObject quitUI;
	public CanvasGroup pauseUI;
	public CanvasGroup darkener;
	public bool isPaused;

	private PlayerMotorBehavior motor;
	private PlayerCameraBehavior cam;

	private CursorLockMode cursorLock = CursorLockMode.None;
	private bool cursorVisibility = true;
	private bool moveState = true;
	private bool lookState = true;

	private void Start()
	{
		motor = FindObjectOfType<PlayerMotorBehavior>();
		cam = FindObjectOfType<PlayerCameraBehavior>();

		ContinueGame();

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(settingsUI.activeInHierarchy)
			{
				settingsUI.SetActive(false);
				return;
			}

			if(quitUI.activeInHierarchy)
			{
				quitUI.SetActive(false);
				return;
			}

			if(isPaused) ContinueGame();
			else PauseGame();
		}

		if(settingsUI.activeInHierarchy || quitUI.activeInHierarchy)
		{
			darkener.alpha = Mathf.MoveTowards(darkener.alpha, 0.9f, Time.unscaledDeltaTime * 3);
		}

		else darkener.alpha = Mathf.MoveTowards(darkener.alpha, 0.4f, Time.unscaledDeltaTime * 3);
	}

	public void PauseGame()
	{
		settingsUI.SetActive(false);
		quitUI.SetActive(false);

		pauseUI.interactable = true;

		cursorLock = Cursor.lockState;
		cursorVisibility = Cursor.visible;

		moveState = motor.canMove;
		lookState = cam.allowRotation;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		motor.canMove = false;
		cam.allowRotation = false;

		Time.timeScale = 0;
		pauseUI.alpha = 1;
		isPaused = true;
	}

	public void ContinueGame()
	{
		settingsUI.SetActive(false);
		quitUI.SetActive(false);

		pauseUI.interactable = false;

		Cursor.lockState = cursorLock;
		Cursor.visible = cursorVisibility;

		motor.canMove = moveState;
		cam.allowRotation = lookState;

		Time.timeScale = 1;
		pauseUI.alpha = 0;
		isPaused = false;
	}

	public void OpenSettings()
	{
		quitUI.SetActive(false);
		settingsUI.SetActive(true);
	}

	public void OpenQuitToDesktop()
	{
		quitUI.SetActive(true);
		settingsUI.SetActive(false);
	}

	public void CloseActiveMenu()
	{
		quitUI.SetActive(false);
		settingsUI.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
