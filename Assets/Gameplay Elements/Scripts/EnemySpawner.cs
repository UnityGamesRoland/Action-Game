using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
	public string waveName;
	public int spawnAmount;
	public Vector3[] spawnPositions;
}

public class EnemySpawner : MonoBehaviour
{
	public Transform runnerPrefab;
	public EnemyWave[] waves;

	private void Start()
	{
		StartCoroutine(SpawnWaves());
	}

	public void StartWave(int waveIndex)
	{
		EnemyWave targetWave = waves[waveIndex];

		for(int i = 0; i < targetWave.spawnAmount; i++)
		{
			Instantiate(runnerPrefab, targetWave.spawnPositions[i], Quaternion.identity);
		}
	}

	private IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(2);
		StartWave(0);
		yield return new WaitForSeconds(0.5f);
		StartWave(1);
		yield return new WaitForSeconds(1.5f);
		StartWave(2);
		yield return new WaitForSeconds(2f);
		StartWave(3);
	}

	private void OnDrawGizmos()
	{
		if(waves.Length > 0)
		{
			foreach (EnemyWave wave in waves)
			{
				Gizmos.color = Color.blue;

				if(wave.spawnPositions.Length == wave.spawnAmount)
				{
					for(int i = 0; i < wave.spawnAmount; i++)
					{
						Gizmos.DrawSphere (wave.spawnPositions[i], 0.2f);
						Gizmos.DrawRay(wave.spawnPositions[i], Vector3.up * 2);
					}
				}
			}
		}
	}
}
