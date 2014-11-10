//
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

	void Start()
	{
		m_properties = GetComponent<EntityProperties>();
		m_pawn = GetComponent<Pawn>();

		m_pawn.OnPawnDie += OnEnemyDie;

		m_properties.ColorChannel0 = _initChannel0;
		m_properties.ColorChannel1 = _initChannel1;
		m_properties.ColorChannel2 = _initChannel2;
	}

	void Update()
	{
	}

	#region Enemy Death

	private void OnEnemyDie(Pawn pawn)
	{
		GPEventManager.Instance.Raise("EnemyDied",new GameObjectEvent(this.gameObject));

		SpawnOrbs();
	}

	private void SpawnOrbs()
	{
		ColorOrb channel0Orb = (ColorOrb) GameObject.Instantiate(_orbPrefab,transform.position,transform.rotation);
		ColorOrb channel1Orb = (ColorOrb) GameObject.Instantiate(_orbPrefab,transform.position,transform.rotation);
		ColorOrb channel2Orb = (ColorOrb) GameObject.Instantiate(_orbPrefab,transform.position,transform.rotation);

		channel0Orb._channel = Channel.CHANNEL_0;
		channel1Orb._channel = Channel.CHANNEL_1;
		channel2Orb._channel = Channel.CHANNEL_2;

		channel0Orb.StationnaryLocation = transform.position + 
			new Vector3(Random.Range(-1f,1f),Random.Range(-0.5f,0.5f),Random.Range(-1f,1f));

		channel1Orb.StationnaryLocation = transform.position + 
			new Vector3(Random.Range(-1f,1f),Random.Range(-0.5f,0.5f),Random.Range(-1f,1f));

		channel2Orb.StationnaryLocation = transform.position +
			new Vector3(Random.Range(-1f,1f),Random.Range(-0.5f,0.5f),Random.Range(-1f,1f));

		channel0Orb.ColorValue = GameManager.Instance._enemyOrbLossChannel0 * _initChannel0;
		channel1Orb.ColorValue = GameManager.Instance._enemyOrbLossChannel0 * _initChannel1;
		channel2Orb.ColorValue = GameManager.Instance._enemyOrbLossChannel0 * _initChannel2;
	}

	#endregion
}
