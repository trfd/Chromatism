using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnWave : MonoBehaviour 
{

	#region members

	public List<SpawnPoint> _spawnPoints;
	public float _waveTiming;

	private Timer m_spawnTimer;

	#endregion
	
	#region Properties
	
	#endregion
	
	
	#region MonoBehaviours
	
	void Start()
	{
		if(_waveTiming == 0f || _spawnPoints == null || _spawnPoints.Count == 0)
		{
			Debug.LogError("Set the fucking values you dumbass !");
			return;
		}
	}
	
	void Update()
	{
		if(m_spawnTimer != null && m_spawnTimer.IsElapsedLoop)
		{
			Spawn();
			m_spawnTimer = null;
		}
	}
	
	#endregion
	
	#region Functions

	public void StartWave()
	{
		m_spawnTimer = new Timer(_waveTiming);
	}
	
	void Spawn()
	{
		foreach(SpawnPoint sp in _spawnPoints)
		{
			sp.StartSpawn();
		}
	}
	
	#endregion
	
}
