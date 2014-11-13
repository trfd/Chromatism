//
// PlayerBehaviour.cs
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

[AddComponentMenu("Gameplay/PlayerBehaviour")]
[RequireComponent(typeof(Pawn))]
[RequireComponent(typeof(PlayerBehaviour))]
public class PlayerBehaviour : MonoBehaviour
{
	#region Private Members

	private Camera m_camera;

	private EntityProperties m_properties;

	private Weapon m_weapon;

	private Pawn m_pawn;

	/// <summary>
	/// The coordinate (in world space) of point player is aiming at. 
	/// </summary>
	private Vector3 m_aimingPoint;

	private Ray m_aimingRay;

	private int m_rayLayerMask;

	#endregion

	#region Public Properties

	public float _channel0Gain = 0.1f;
	public float _channel1Gain = 0.1f;
	public float _channel2Gain = 0.1f;

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

	public Weapon Weapon
	{
		get{ return m_weapon; }
	}
	
	#endregion

	#region MonoBehaviour

	void Start()
	{
		m_camera = GetComponentInChildren<Camera>();

		m_aimingRay = new Ray();

		m_rayLayerMask = ~(LayerMask.NameToLayer("Player"));

		// Get Components

		m_pawn = GetComponent<Pawn>();
		m_properties = GetComponent<EntityProperties>();
		m_weapon = GetComponentInChildren<Weapon>();

		// Register Delegates

		m_pawn.OnPawnHit += OnPlayerHit;
		m_weapon.OnWeaponShoot       += OnPlayerWeaponShoot;
		m_weapon.OnWeaponStartReload += OnPlayerWeaponStartReload;
		m_weapon.OnWeaponStopReload  += OnPlayerWeaponStopReload;

		// Key Binding

		KeyBinder.Instance.DefineActions("MouseLeftClick", 
		                                 new KeyActionConfig(KeyType.Action, 0,
		                    			() => { m_weapon.StartShooting(); }, 
										() => { m_weapon.StopShooting();  } ));

		// Event Registering

		GPEventManager.Instance.Register("EnemyDied",OnEnemyDie);
	}

	void Update()
	{
		m_aimingRay = new Ray(m_camera.transform.position,m_camera.transform.forward);

		RaycastHit hit;

		if(Physics.Raycast(m_aimingRay,out hit,Mathf.Infinity,m_rayLayerMask))
			m_aimingPoint = hit.point;
		else
			m_aimingPoint = Vector3.zero;

		m_pawn.AimingPoint = m_aimingPoint;
	}

	#endregion

	#region Event

	private void OnPlayerHit(Pawn pawn)
	{
		GPEventManager.Instance.Raise("PlayerTouched",new GPEvent());
        Fabric.EventManager.Instance.PostEvent("player_hurt", gameObject);
	}

	private void OnPlayerWeaponShoot(Weapon weapon)
	{
		GPEventManager.Instance.Raise("PlayerWeaponShoot", new GPEvent());
	}
	
	private void OnPlayerWeaponStartReload(Weapon weapon)
	{
		GPEventManager.Instance.Raise("PlayerWeaponStartReload", new GPEvent());
	}

	private void OnPlayerWeaponStopReload(Weapon weapon)
	{
		GPEventManager.Instance.Raise("PlayerWeaponStopReload", new GPEvent());
	}

	public void OnEnemyDie(string evtName,GPEvent evt)
	{
	}

	#endregion

	#region Public Interface

	public void PickUpOrb(ColorOrb orb)
	{
		switch(orb._channel)
		{
		case Channel.CHANNEL_0:
			Properties.ColorChannel0 += orb.ColorValue; 
			break;
		case Channel.CHANNEL_1:
			Properties.ColorChannel1 += orb.ColorValue;
			break;
		case Channel.CHANNEL_2:
			Properties.ColorChannel2 += orb.ColorValue;
			break;
		}

		GPEventManager.Instance.Raise("PlayerPickOrb",new GPEvent());
        Fabric.EventManager.Instance.PostEvent("orb_pickup", gameObject);
	}

	#endregion
}
