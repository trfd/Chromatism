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
		get
		{
			if(m_instance == null) 
			{
				GameObject singleton = new GameObject();
				m_instance = singleton.AddComponent<DifficultyManager>();
				singleton.name = "DifficultyManager";
				m_instance.Reset();
			}
			
			return m_instance;
		}

		set
		{
			if(m_instance == null)
			{
				m_instance = value;
			}
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
		m_instance = this;
		m_instance.Reset();
	}

	#endregion

	#region Functions

	void OnKill( string evtName, GPEvent gpEvent)
	{
		Debug.Log("Kill detected !");
		m_currentKills++;
		if(m_currentKills >= ((m_killForLevelUp + m_levelUpCoef) * m_level) && m_level <= m_maxLevel)
		{
			Debug.Log("level up !");
			m_level++;
		}
	}

	public void Reset()
	{
		m_killForLevelUp = 5;
		m_levelUpCoef = 3;
		m_currentKills = 0;
		m_maxLevel = 666;
		m_level = 1;
		DontDestroyOnLoad(this);
		Debug.Log("reset");
		GPEventManager.Instance.Register("EnemyDied", OnKill);
	}

	public float _defaultBulletDammage = 0.1f;
	public float _defaultBulletRange = 50f;
	public float _defaultBulletSize = 0.1f;
	public float _defaultBulletVelocity = 4f;
	public float _defaultFireRate = 10f;
	public float _defaultMagazineSize = 1f;

	public float _bulletDammageMaxValue = 1f;
	public float _rangeMaxValue = 1000f;
	public float _bulletSizeMaxValue = 3f;
	public float _bulletVelocityMaxValue = 30f;
	public float _fireRateMaxValue = 200f;
	public float _magazineMaxValue = 100f;

	public void AdjustPropertiesToDifficulty( ref EnemyBehaviour enemy)
	{
		if(enemy.Properties == null)
		{
			enemy.Init();
		}

		enemy.Properties.WeaponBulletDamages = _defaultBulletDammage + (_bulletDammageMaxValue - _defaultBulletDammage) * (m_level / m_maxLevel);
		enemy.Properties.WeaponBulletRange = _defaultBulletRange + (_rangeMaxValue - _defaultBulletRange) * (m_level / m_maxLevel);
		enemy.Properties.WeaponBulletSize = _defaultBulletSize + (_bulletSizeMaxValue - _defaultBulletSize) * (m_level / m_maxLevel);
		enemy.Properties.WeaponBulletVelocity = _defaultBulletVelocity + (_bulletVelocityMaxValue - _defaultBulletVelocity) * (m_level / m_maxLevel);
		enemy.Properties.WeaponFireRate = _defaultFireRate + (_fireRateMaxValue - _defaultFireRate) * (m_level / m_maxLevel);
		enemy.Properties.WeaponMagazineSize = _defaultMagazineSize + (_magazineMaxValue - _defaultMagazineSize) * (m_level / m_maxLevel);
	}
	
	#endregion
}
