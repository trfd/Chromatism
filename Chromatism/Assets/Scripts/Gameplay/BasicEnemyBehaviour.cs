//
// BasicEnemyBehaviour.cs
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

public class BasicEnemyBehaviour : MonoBehaviour
{
	public enum State
	{
		REST,
		PATROL,
		ATTACK
	}

	#region Private Members

	private PlayerBehaviour m_player; 

	/// <summary>
	/// Timer used to track ai updates
	/// </summary>
	private Timer m_aiTimer;

	/// <summary>
	/// Current AI State
	/// </summary>
	[InspectorLabel()]
	private State m_currState;

	#endregion

	#region Public Members

	/// <summary>
	/// The update frequency of AI.
	/// </summary>
	public float _aiUpdateFrequency;

	public float _coneViewAngle;

	public float _coneViewRange;

	public float _maxRotateSpeed;

	#endregion

	#region Properties

	#endregion

	#region Constructors

	#endregion

	#region MonoBehaviour

	void Start()
	{
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		m_aiTimer = new Timer(1f / _aiUpdateFrequency);
	}

	void Update()
	{
		if(m_aiTimer.IsElapsedLoop)
		{
			UpdateAI();
			m_aiTimer.Reset(1f / _aiUpdateFrequency);
		}

		UpdateState();
	}

	#endregion

	#region AI

	private void UpdateAI()
	{
		if(IsSeeingPlayer())
			m_currState = State.ATTACK;
		else
			m_currState = State.REST;
	}

	#endregion

	#region State

	private void UpdateState()
	{
		switch(m_currState)
		{
		case State.REST:
			UpdateRest();
			break;
		case State.PATROL:
			UpdatePatrol();
			break;
		case State.ATTACK:
			UpdateAttack();
			break;
		default:
			Debug.LogError("Invalid AI state");
			break;
		}
	}

	private void UpdateRest()
	{

	}

	private void UpdatePatrol()
	{

	}

	private void UpdateAttack()
	{
		LookAt(m_player.transform.position);
	}

	private void LookAt(Vector3 position)
	{
		Vector3 localRel = transform.InverseTransformPoint(position);

		float angle = Mathf.Atan2(localRel.x,localRel.z);

		transform.Rotate(Vector3.up, Mathf.Min(Time.deltaTime * _maxRotateSpeed, angle * Mathf.Rad2Deg));
	}

	#endregion

	#region Awarness

	private bool IsSeeingPlayer()
	{
		Vector3 rel = m_player.transform.position - transform.position;

		rel.y = 0;

		if(rel.magnitude >= _coneViewRange)
			return false;

		Vector3 localRel = transform.InverseTransformPoint(m_player.transform.position);
		
		float angle = Mathf.Atan2(localRel.x,localRel.z);

		return (Mathf.Abs(angle) < _coneViewAngle * Mathf.Deg2Rad);
	}

	#endregion

	#region Debug

	[InspectorButton("Generate Vision Cone")]
	void GenerateVisionCone()
	{
		VisionConeMesh mesh = GetComponentInChildren<VisionConeMesh>();

		if(mesh == null)
			return;

		mesh.GenerateMesh();
	}

	#endregion
}
