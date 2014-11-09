using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	#region Private Members
	
	private float  m_fireRate;
	private float m_reloadDuration;
	private bool m_isShooting = false;
	private Timer m_shootTimer;
	
	#endregion


	#region Properties

	public float FireRate
	{
		get{return m_fireRate;}
		set{m_fireRate = value;}
	}

	public float ReloadDuration
	{
		get{return m_reloadDuration;}
		set{m_reloadDuration = value;}
	}

	#endregion


	#region MonoBehaviour

	void Start()
	{
		m_shootTimer = new Timer();
	}

	void Update()
	{
		if(m_isShooting && m_shootTimer.IsElapsedLoop)
		{
			m_shootTimer.Reset(m_reloadDuration);
			Shoot();
		}
	}
	
	#endregion

	#region Weapon functions

	void Shoot()
	{
		//TODO
	}

	#endregion
	
}
