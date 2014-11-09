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
	private int m_remainingBullets;
	
	#endregion

	#region Public Member

	/// <summary>
	/// Bullet pool used to get bullets.
	/// </summary>
	public Pool _bulletPool;

	/// <summary>
	/// Location used to spawn bullets.
	/// </summary>
	public Transform _bulletSpawnTransform;

	#endregion

	#region Properties

	/// <summary>
	/// Pawn holding the weapon.
	/// </summary>
	/// <value>The owner.</value>
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

	/// <summary>
	/// Gets the remaining bullets in weapon's magazine.
	/// </summary>
	/// <value>The remaining bullets.</value>
	public int RemainingBullets
	{
		get{ return m_remainingBullets; }
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
		m_remainingBullets = (int) m_properties.WeaponMagazineSize;
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

	//[InspectorButton("Shoot")]
	public void StartShooting()
	{
		m_isInputShooting = true;
	}

	//[InspectorButton("Stop")]
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
		m_remainingBullets = (int) m_properties.WeaponMagazineSize;

		Debug.Log("End Reload");
	}

	private bool CanShoot()
	{
		return (!m_isReloading && m_isInputShooting && 
		        m_shootTimer.IsElapsedLoop && m_remainingBullets > 0);
	}

	private void Shoot()
	{
		// Warning: No double checks
		// Shoot assumes this.CanShoot()

		m_remainingBullets--;

		ShootBullet();

		if(m_remainingBullets == 0)
			StartReload();
	}

	private void ShootBullet()
	{
		Bullet newBullet = (Bullet) _bulletPool.GetUnusedObject();

		if(newBullet == null)
		{
			Debug.LogError("Null bullet, skip shoot");
			return;
		}

		newBullet.Owner   = Owner.gameObject;
		newBullet.Damages = m_properties.WeaponBulletDamages;
		newBullet.Range   = m_properties.WeaponBulletRange;

		if(m_owner.AimingPoint == Vector3.zero)
			newBullet.Velocity = m_properties.WeaponBulletVelocity * _bulletSpawnTransform.forward;
		else
		{
			Vector3 dir = m_owner.AimingPoint - _bulletSpawnTransform.position;	
			newBullet.Velocity = m_properties.WeaponBulletVelocity * dir.normalized;
		}

		newBullet.transform.localScale = m_properties.WeaponBulletSize * Vector3.one;

		newBullet.SpawnAt(_bulletSpawnTransform.position);
	}

	#endregion
	
}
