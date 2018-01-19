using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour
{
	public LayerMask interactLayer;
	public float interactRange;
	public Text pickupText;

	public void Update()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, interactRange, interactLayer))
		{
			Item item = hit.transform.GetComponent<Item>();
			StartCoroutine(WriteText(item.pickupText));

			if(Input.GetKeyDown(KeyCode.E))
			{
				item.PickupItem();
			}
		}

		else
		{
			StopAllCoroutines();
			pickupText.text = "";
		}
	}

	IEnumerator WriteText(string finalText)
	{
		pickupText.text = "";
		int loopProgress = -1;

		foreach(char letter in finalText.ToCharArray())
		{
			loopProgress++;
			if(loopProgress < 3) pickupText.text += "<color=orange>" + letter + "</color>";
			else pickupText.text += letter;
			yield return null;
		}
	}
}
