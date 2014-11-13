﻿using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	#region members

	public GameObject _enemyPrefab;
	public Patrol _patrol;
	public int _numberOfEntitiesByWave;
	public float _cooldown;

	private int m_numberToSpawn;
	private Timer m_spawnTimer;

	#endregion

	#region MonoBehaviours

	void Start()
	{
		if(_cooldown == 0f || _numberOfEntitiesByWave == 0)
		{
			Debug.LogError("Set the fucking values you dumbass !");
			return;
		}

		StartSpawn();
	}

	void Update()
	{
		if(m_spawnTimer.IsElapsedLoop && m_numberToSpawn > 0)
		{
			SpawnOneEnemy();
			if(m_numberToSpawn > 0)
			{
				m_spawnTimer.Reset(_cooldown);
			}
		}
	}

	#endregion

	#region Spawn Functions

	public void StartSpawn()
	{
		m_numberToSpawn = _numberOfEntitiesByWave;
		m_spawnTimer = new Timer(_cooldown);
	}

	void SpawnOneEnemy()
	{
		if(_patrol == null || _enemyPrefab == null)
		{
			Debug.LogError("Ref should be assign motha fucka !");
			return;
		}

		var enemySpawned = GameObject.Instantiate(_enemyPrefab, transform.position, Quaternion.identity) as GameObject;
		var bEnemyB = enemySpawned.GetComponent<BasicEnemyBehaviour>();
		if(bEnemyB == null)
		{
			Debug.LogError("Enemy should have BasicEnemyBehaviours class !");
			return;
		}

		bEnemyB._patrolState._patrol = _patrol;

		var enemyB = enemySpawned.GetComponent<EnemyBehaviour>();
		if(enemyB == null)
		{
			Debug.LogError("Enemy should have EnemyBehaviours class !");
			return;
		}
		DifficultyManager.Instance.AdjustPropertiesToDifficulty(ref enemyB);

	}

	#endregion
}