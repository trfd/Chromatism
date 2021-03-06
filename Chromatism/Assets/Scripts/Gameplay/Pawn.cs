﻿//
// Pawn.cs
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

[AddComponentMenu("Gameplay/Pawn")]
[RequireComponent(typeof(EntityProperties))]
public class Pawn : MonoBehaviour
{
	public enum DeathType
	{
		FIRST_CHANNEL_DOWN,
		ALL_CHANNELS_DOWN
	}

	public delegate void PawnDelegate(Pawn pawn);

	#region Private Members

	private EntityProperties m_properties;

	private bool m_isDead = false;

	#endregion

	#region Protected Members

	private Vector3 m_aimingPoint;

	#endregion

	#region Public Members

	public DeathType _death;

	/// <summary>
	/// The coeficient of color loss when hit by a bullet.
	/// </summary>
	[Tooltip("The coeficient of color loss when hit by a bullet")]
	public float _channel0LossCoef = 1f;

	[Tooltip("The coeficient of color loss when hit by a bullet")]
	public float _channel1LossCoef = 1f;

	[Tooltip("The coeficient of color loss when hit by a bullet")]
	public float _channel2LossCoef = 1f;

	#endregion

	#region Property Members

	public Vector3 AimingPoint
	{
		get{ return m_aimingPoint;  }
		set{ m_aimingPoint = value; }
	}

	#endregion

	#region Delegate
	
	public PawnDelegate OnPawnHit
	{
		get; set; 
	}

	public PawnDelegate OnPawnDie
	{
		get; set; 
	}

	#endregion

	#region Properties

	public EntityProperties Properties
	{
		get
		{ 
			if(m_properties == null)
				m_properties = GetComponent<EntityProperties>();
			return m_properties; 
		}
	}

	public bool IsDead
	{
		get{ return m_isDead; }
	}

	#endregion

	#region MonoBehaviour

	void Start()
	{
		m_properties = GetComponent<EntityProperties>();
	}

	void Update()
	{
	}

	#endregion

	#region Hit

	public virtual void HitByBullet(Bullet bullet)
	{
		if(bullet.IsUsed || m_isDead)
			return;

		EntityProperties otherProperties = bullet.Owner.GetComponent<EntityProperties>();

		m_properties.ColorChannel0 -= _channel0LossCoef * bullet.Damages * otherProperties.ColorChannel0;
		m_properties.ColorChannel1 -= _channel1LossCoef * bullet.Damages * otherProperties.ColorChannel1;
		m_properties.ColorChannel2 -= _channel2LossCoef * bullet.Damages * otherProperties.ColorChannel2;

		if(CheckDeath())
		{
			Die();
		}
		else if(OnPawnHit != null)
			OnPawnHit(this);
	}

	private bool CheckDeath()
	{
		switch(_death)
		{
		case DeathType.ALL_CHANNELS_DOWN:  return CheckDeathAllChannels();
		case DeathType.FIRST_CHANNEL_DOWN: return CheckDeathFirstChannel();
		default: Debug.LogError("Incorrect Death Type"); break;
		}

		return false;
	}

	private bool CheckDeathAllChannels()
	{
		return (m_properties.ColorChannel0 <= 0 && 
			    m_properties.ColorChannel1 <= 0 &&
		        m_properties.ColorChannel2 <= 0 );
	}

	private bool CheckDeathFirstChannel()
	{
		return (m_properties.ColorChannel0 <= 0 || 
		        m_properties.ColorChannel1 <= 0 ||
		        m_properties.ColorChannel2 <= 0 );
	}

	#endregion

	#region Life Management

	//[InspectorButton("Kill")]
	public virtual void Die()
	{
		m_isDead = true;

		if(OnPawnDie != null)
			OnPawnDie(this);
	}

	#endregion
}
