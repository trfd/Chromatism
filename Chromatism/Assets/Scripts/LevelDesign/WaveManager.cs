//
// WaveManager.cs
//
// Author(s):
//       Baptiste Dupy <baptiste.dupy@gmail.com>
//
// Copyright (c) 2014
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
	#region Singleton
	
	private static WaveManager m_instance;
	
	public static WaveManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<WaveManager>();
				//DontDestroyOnLoad(m_instance.gameObject);
			}
			
			return m_instance;
		}
	}
	
	void Awake() 
	{
		if(m_instance == null)
		{
			m_instance = this;
			m_instance.Init();
			//DontDestroyOnLoad(this);
		}
		else
		{
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	
	#endregion 

	private List<GameObject> m_enemies;

	private int m_currWaveIdx;

	public SpawnWave[] _waves;

	void Init()
	{
	}

	void Start()
	{
		m_enemies = new List<GameObject>();

		m_currWaveIdx = 0;

		GPEventManager.Instance.Register("EnemySpawned",OnEnemySpawned);
		GPEventManager.Instance.Register("EnemyDied",OnEnemyDied);


		_waves[m_currWaveIdx].StartWave();
	}

	void Update()
	{
		if(m_enemies.Count == 0 && _waves[m_currWaveIdx].HasSpawned)
		{
			_waves[m_currWaveIdx].Clear();
			m_currWaveIdx = (m_currWaveIdx+1)%_waves.Length;
			Debug.Log("Start Wave "+m_currWaveIdx);
			_waves[m_currWaveIdx].StartWave();
		}

	}

	void OnEnemySpawned(string evtName, GPEvent evt)
	{
		GameObjectEvent goEvt = (GameObjectEvent) evt;

		m_enemies.Add(goEvt._object);
	}

	void OnEnemyDied(string evtName, GPEvent evt)
	{
		GameObjectEvent goEvt = (GameObjectEvent) evt;

		m_enemies.Remove(goEvt._object);

		//UpdateWaves();
	}
}
