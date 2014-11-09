using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	#region Private Members

	/// <summary>
	/// Pawn holding the weapon.
	/// </summary>
	private Pawn m_owner;

	/// <summary>
	/// Properties of Owner
	/// </summary>
	private EntityProperties m_properties;

	/// <summary>
	/// Holds whether or not the shoot input button is down
	/// </summary>
	private bool  m_isInputShooting = false;

	private bool m_isReloading = false;

	/// <summary>
	/// Timer for cooldown shoots.
	/// </summary>
	private Timer m_shootTimer;

	/// <summary>
	/// Timer for reloading.
	/// </summary>
	private Timer m_reloadTimer;

	/// <summary>
	/// Number of bullet remaining in the magazine
	/// </summary>
	private int m_remainingBullet;

	#endregion


	#region Properties

	public Pawn Owner
	{
		get{ return m_owner; }
	}

	public EntityProperties OwnerProperties
	{
		get{ return m_properties; }
	}

	public float FireRate
	{
		get{ return m_properties.WeaponFireRate; }
	}

	public float ReloadDuration
	{
		get{return m_properties.WeaponReloadDuration;}
	}

	#endregion


	#region MonoBehaviour

	void Start()
	{
		m_shootTimer = new Timer();
		m_reloadTimer = new Timer();

		m_owner = GetComponentInParent<Pawn>();

		if(m_owner == null)
		{
			Debug.LogError("No Pawn found for Weapon");
			return;
		}

		m_properties = m_owner.Properties;

		m_shootTimer.Reset(1f/FireRate);
		m_remainingBullet = (int) m_properties.WeaponMagazineSize;
	}

	void Update()
	{
		if(m_isReloading && m_reloadTimer.IsElapsedLoop)
			EndReload();

		if(CanShoot())
		{
			m_shootTimer.Reset(1f/FireRate);
			Shoot();
		}
	}
	
	#endregion

	#region Weapon Public Interface

	/// <summary>
	/// Sets the shoot input.
	/// True for shoot, false otherwise
	/// </summary>
	/// <param name="input">If set to <c>true</c> input.</param>
	public void ShootInput(bool input)
	{
		m_isInputShooting = input;
	}

	[InspectorButton("Shoot")]
	public void StartShooting()
	{
		m_isInputShooting = true;
	}

	[InspectorButton("Stop")]
	public void StopShooting()
	{
		m_isInputShooting = false;
	}

	#endregion

	#region Weapon functions

	private void StartReload()
	{
		m_isReloading = true;
		m_reloadTimer.Reset(ReloadDuration);

		Debug.Log("Start Reload");
	}

	private void EndReload()
	{
		m_isReloading = false;
		m_remainingBullet = (int) m_properties.WeaponMagazineSize;

		Debug.Log("End Reload");
	}

	private bool CanShoot()
	{
		return (!m_isReloading && m_isInputShooting && m_shootTimer.IsElapsedLoop && m_remainingBullet > 0);
	}

	private void Shoot()
	{
		// Warning: No double checks
		// Shoot assumes this.CanShoot()

		m_remainingBullet--;

		ShootBullet();

		if(m_remainingBullet == 0)
			StartReload();
	}

	private void ShootBullet()
	{
		Debug.Log("PIOU");
	}

	#endregion
	
}
