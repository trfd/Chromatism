using UnityEngine;
using System.Collections;

public class DifficultyManager : MonoBehaviour {


	#region members

	private int m_killForLevelUp;
	private int m_levelUpCoef;
	private int m_currentKills;
	private int m_maxLevel;
	private int m_level;
	
	private static DifficultyManager m_instance;

	#endregion

	#region Properties

	public static DifficultyManager Instance
	{
		get{
			if(m_instance == null) 
			{
				GameObject singleton = new GameObject();
				m_instance = singleton.AddComponent<DifficultyManager>();
				singleton.name = "SpawnManager";
				DontDestroyOnLoad(singleton);
				m_instance.Reset();
			}
			
			return m_instance;
		}
	}

	public int Level
	{
		get{ return m_level; }
		set{ m_level = value; }
	}

	#endregion


	#region MonoBehaviours

	void Start()
	{
		GPEventManager.Instance.Register("EnemyDied", OnKill);
	}

	#endregion

	#region Functions

	void OnKill( string evtName, GPEvent gpEvent)
	{
		m_currentKills++;
		if(m_currentKills >= (m_killForLevelUp + m_levelUpCoef * m_level) && m_level <= m_maxLevel)
		{
			m_level++;
		}
	}

	public void Reset()
	{
		m_killForLevelUp = 5;
		m_levelUpCoef = 3;
		m_currentKills = 0;
		m_maxLevel = 666;
		m_level = 0;
	}
	
	#endregion
}
