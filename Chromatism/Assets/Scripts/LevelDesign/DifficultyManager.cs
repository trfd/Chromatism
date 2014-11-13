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

	public int Kills
	{
		get{return m_currentKills;}
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
		if(m_currentKills >= ((m_killForLevelUp + m_levelUpCoef) * m_level) && m_level <= m_maxLevel)
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


	private float m_defaultBulletDammage = 0.1f;
	private float m_defaultBulletRange = 50f;
	private float m_defaultBulletSize = 0.1f;
	private float m_defaultBulletVelocity = 4f;
	private float m_defaultFireRate = 10f;
	private float m_defaultMagazineSize = 1f;

	private float m_bulletdammageMaxValue = 1f;
	private float m_rangeMaxValue = 1000f;
	private float m_bulletSizeMaxValue = 3f;
	private float m_bulletVelocityMaxValue = 30f;
	private float m_fireRateMaxValue = 200f;
	private float m_magazineMaxValue = 100f;

	public void AdjustPropertiesToDifficulty( ref EnemyBehaviour enemy)
	{
		if(enemy != null)
		{
			enemy.Properties.WeaponBulletDamages = m_defaultBulletDammage + (m_bulletdammageMaxValue - m_defaultBulletDammage) * (m_level / m_maxLevel);
			enemy.Properties.WeaponBulletRange = m_defaultBulletRange + (m_rangeMaxValue - m_defaultBulletRange) * (m_level / m_maxLevel);
			enemy.Properties.WeaponBulletSize = m_defaultBulletSize + (m_bulletSizeMaxValue - m_defaultBulletSize) * (m_level / m_maxLevel);
			enemy.Properties.WeaponBulletVelocity = m_defaultBulletVelocity + (m_bulletVelocityMaxValue - m_defaultBulletVelocity) * (m_level / m_maxLevel);
			enemy.Properties.WeaponFireRate = m_defaultFireRate + (m_fireRateMaxValue - m_defaultFireRate) * (m_level / m_maxLevel);
			enemy.Properties.WeaponMagazineSize = m_defaultMagazineSize + (m_magazineMaxValue - m_defaultMagazineSize) * (m_level / m_maxLevel);
		}
	} 
	
	#endregion
}
