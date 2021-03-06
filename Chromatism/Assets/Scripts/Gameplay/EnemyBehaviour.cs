﻿//
// EnemyBehaviour.cs
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

[AddComponentMenu("Gameplay/EnemyBehaviour")]
[RequireComponent(typeof(Pawn))]
[RequireComponent(typeof(EntityProperties))]
public class EnemyBehaviour : MonoBehaviour
{
	#region Private Members

	private EntityProperties m_properties;

	private Pawn m_pawn;

	#endregion

	#region Public Members

	public ColorOrb _orbPrefab;

	/// <summary>
	/// Value of initialization for channel 0.
	/// </summary>
	[Range(0f,1f)]
	public float _initChannel0;

	/// <summary>
	/// Value of initialization for channel 1.
	/// </summary>
	[Range(0f,1f)]
	public float _initChannel1;

	/// <summary>
	/// Value of initialization for channel 2.
	/// </summary>
	[Range(0f,1f)]
	public float _initChannel2;

	#endregion

	#region Properties

	public EntityProperties Properties
	{
		get{ return m_properties; }
	}

	public Pawn Pawn
	{
		get{ return m_pawn; }
	}

	#endregion

	#region MonoBehaviour

	void Start()
	{
		Init();
	}

	public void Init()
	{
		m_properties = GetComponent<EntityProperties>();
		m_pawn = GetComponent<Pawn>();

		Weapon weapon = GetComponent<Weapon>();

		// Register Delegates

		m_pawn.OnPawnDie += OnEnemyDie;
		m_pawn.OnPawnHit += OnEnemyHit;

		if(weapon != null)
		{
			weapon.OnWeaponShoot       += OnEnemyWeaponShoot;
			weapon.OnWeaponStartReload += OnEnemyWeaponStartReload;
			weapon.OnWeaponStartReload += OnEnemyWeaponStopReload;
		}

		m_properties.ColorChannel0 = _initChannel0;
		m_properties.ColorChannel1 = _initChannel1;
		m_properties.ColorChannel2 = _initChannel2;
	}

	void Update()
	{
	}

	void Clear()
	{
		if(rigidbody != null)
		{
			rigidbody.isKinematic = true;
			rigidbody.Sleep();
		}

		if(collider != null)
			collider.enabled = false;

		Renderer[] renderers = GetComponentsInChildren<Renderer>();

		foreach(Renderer renderer in renderers)
			renderer.enabled = false;
	}

	#endregion

	#region Events

	private void OnEnemyHit(Pawn pawn)
	{
		GPEventManager.Instance.Raise("EnemyTouched",new GPEvent());
	}

	#endregion

	#region Enemy Death


	private void SpawnOrbs()
	{
		if(_initChannel0 >= 0.1f)
			SpawnOrb(Channel.CHANNEL_0 , GameManager.Instance._enemyOrbLossChannel0 * _initChannel0);

		if(_initChannel1 >= 0.1f)
			SpawnOrb(Channel.CHANNEL_1 , GameManager.Instance._enemyOrbLossChannel1 * _initChannel1);

		if(_initChannel2 >= 0.1f)
			SpawnOrb(Channel.CHANNEL_2 , GameManager.Instance._enemyOrbLossChannel2 * _initChannel2);
	}

	private void SpawnOrb(Channel channel, float value)
	{
		ColorOrb orb = (ColorOrb) GameObject.Instantiate(_orbPrefab,transform.position,transform.rotation);

		orb.StationnaryLocation = transform.position + 
			new Vector3(Random.Range(-1f,1f),Random.Range(-0.5f,0.5f),Random.Range(-1f,1f));

		orb.ColorValue = value;
		orb._channel = channel;
	}

	#endregion

	#region Delegate

	private void OnEnemyDie(Pawn pawn)
	{
		GPEventManager.Instance.Raise("EnemyDied",new GameObjectEvent(this.gameObject));

        Fabric.EventManager.Instance.PostEvent("enemydeath", gameObject);
		
		SpawnOrbs();

		Clear();

		Destroy(this.gameObject, 10f);
	}


	private void OnEnemyWeaponShoot(Weapon weapon)
	{
		GPEventManager.Instance.Raise("EnemyWeaponShoot",new GPEvent());
	}

	private void OnEnemyWeaponStartReload(Weapon weapon)
	{
		GPEventManager.Instance.Raise("EnemyWeaponStartReload",new GPEvent());
	}

	private void OnEnemyWeaponStopReload(Weapon weapon)
	{
		GPEventManager.Instance.Raise("EnemyWeaponStopReload",new GPEvent());
	}

	#endregion
}
