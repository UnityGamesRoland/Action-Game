using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	[HideInInspector] public float health = 100f;
	public CanvasGroup hurtUI;

	public void TakeDamage(int amount)
	{
		health -= amount;

		hurtUI.alpha = 1f;

		if(health <= 0) Debug.Log("Player died!");
	}

	private void Update()
	{
		if(hurtUI.alpha != 0)
		{
			hurtUI.alpha = Mathf.MoveTowards(hurtUI.alpha, 0, Time.deltaTime * 2);
		}
	}
}
