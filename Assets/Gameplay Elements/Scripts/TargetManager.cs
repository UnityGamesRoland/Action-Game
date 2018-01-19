using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
	public int targetsToKill = 5;
	public int targetsKilled;

	public void CountKill()
	{
		targetsKilled++;

		if(targetsKilled == targetsToKill)
		{
			GetComponent<LevelSetup>().EndLevel();
		}
	}
}
