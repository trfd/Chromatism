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

	[InspectorLabel]
	public int Level
	{
		get{ return m_level; }
		set{ m_level = value; }
	}

	[InspectorLabel]
	public int Kills
	{
		get{return m_currentKills;}
	}

	#endregion


	#region MonoBehaviours

	void Start()
	{
		if(m_instance != null && m_instance != this)
		{
			Destroy(m_instance.gameObject);
			m_instance = this;
			m_instance.Reset();
		}
		else if(m_instance == null)
		{
			m_instance = this;
			m_instance.Reset();
		}
	}

	#endregion

	#region Functions

	void OnKill(string evtName, GPEvent gpEvent)
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
		m_level = 1;

		DontDestroyOnLoad(this);

		Debug.Log("reset");

		GPEventManager.Instance.Register("EnemyDied", OnKill);
	}

	public AnimationCurve _channelDifficultyCurve;

	public float _defaultBulletDammage  = 0.1f;
	public float _defaultBulletRange    = 50f;
	public float _defaultBulletSize     = 0.1f;
	public float _defaultBulletVelocity = 4f;
	public float _defaultFireRate       = 10f;
	public float _defaultMagazineSize   = 1f;

	public float _bulletDammageMaxValue  = 1f;
	public float _rangeMaxValue          = 1000f;
	public float _bulletSizeMaxValue     = 3f;
	public float _bulletVelocityMaxValue = 30f;
	public float _fireRateMaxValue       = 200f;
	public float _magazineMaxValue       = 100f;

	public void AdjustPropertiesToDifficulty(ref EnemyBehaviour enemy , Vector3 rawColors)
	{
		float levelNorm = (1f * m_level) / m_maxLevel;

		if(enemy.Properties == null)
			enemy.Init();

		enemy._initChannel0 = rawColors.x + Mathf.Clamp01(_channelDifficultyCurve.Evaluate(levelNorm) * rawColors.x);
		enemy._initChannel1 = rawColors.y + Mathf.Clamp01(_channelDifficultyCurve.Evaluate(levelNorm) * rawColors.y);
		enemy._initChannel2 = rawColors.z + Mathf.Clamp01(_channelDifficultyCurve.Evaluate(levelNorm) * rawColors.z);
	
		enemy.Properties.WeaponBulletDamages  = _defaultBulletDammage  + (_bulletDammageMaxValue  - _defaultBulletDammage)  * levelNorm;
		enemy.Properties.WeaponBulletRange    = _defaultBulletRange    + (_rangeMaxValue          - _defaultBulletRange)    * levelNorm;
		enemy.Properties.WeaponBulletSize     = _defaultBulletSize     + (_bulletSizeMaxValue     - _defaultBulletSize)     * levelNorm;
		enemy.Properties.WeaponBulletVelocity = _defaultBulletVelocity + (_bulletVelocityMaxValue - _defaultBulletVelocity) * levelNorm;
		enemy.Properties.WeaponFireRate       = _defaultFireRate       + (_fireRateMaxValue       - _defaultFireRate)       * levelNorm;
		enemy.Properties.WeaponMagazineSize   = _defaultMagazineSize   + (_magazineMaxValue       - _defaultMagazineSize)   * levelNorm;
	}

	// Debug purpose

	[InspectorButton("LevelUp Test")]
	void LevelUpTest()
	{
		OnKill("",null);
	}

	#endregion
}
