//
// Patrol.cs
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
using System.Collections.Generic;

[System.Serializable]
public class PatrolState
{
	/// <summary>
	/// The patrol used.
	/// </summary>
	public Patrol _patrol;

	public float _velocity;

	/// <summary>
	/// The index of patrol point user of patrol is heading to.
	/// </summary>
	public int _targetPatrolPointIndex;

	/// <summary>
	/// The last position registered last time Patrol.Move was used.
	/// </summary>
	internal Vector3 m_previousPosition;
}

public class Patrol : MonoBehaviour
{
	public enum Type
	{
		ONE_WAY,
		//REVERSE,
		LOOP
	}

	#region Static/Const

	private const float c_patrolPointCloseness = 0.3f;

	#endregion

	#region Private Members

	#endregion

	#region Public Members

	public Type _patrolType;

	public bool _ignoreY;

	public List<PatrolPoint> _patrolPoints;

	#endregion

	#region MonoBehaviour

	void Start()
	{
	}

	void Update()
	{}

	void OnDrawGizmos() 
	{
		DrawGizmos(false);
	}
	
	void OnDrawGizmosSelected()
	{
		DrawGizmos(true);
	}

	#endregion

	#region Patrol Interface

	/// <summary>
	/// Update the specified patrol state and call the associated patrol point behaviour,
	/// depending on the position of specified object.
	/// The position of next patrol point to target is returned.
	/// </summary>
	/// <param name="gameObject">Game object.</param>
	/// <param name="state">State.</param>
	public Vector3 Move(GameObject gameObject ,ref PatrolState state)
	{
		if(state._targetPatrolPointIndex >= _patrolPoints.Count)
		{
			Debug.LogError("Invalid patrol state");
			return gameObject.transform.position;
		}

		PatrolPoint targetPatrolPoint = _patrolPoints[state._targetPatrolPointIndex];

		Vector3 rel = gameObject.transform.position - targetPatrolPoint.transform.position;

		if(_ignoreY) rel.y = 0f;

		Vector3 prevRel = state.m_previousPosition - targetPatrolPoint.transform.position;

		if(_ignoreY) prevRel.y = 0f;

		bool isCloseOfNext  = rel.magnitude <= c_patrolPointCloseness;
		bool wasCloseOfNext = prevRel.magnitude <= c_patrolPointCloseness;

		// Patrol point delegate
		
		if(isCloseOfNext && !wasCloseOfNext)
			targetPatrolPoint.OnObjectEnterPatrolPoint(gameObject);	
		else if(!isCloseOfNext && wasCloseOfNext)
			targetPatrolPoint.OnObjectExitPatrolPoint(gameObject);
		else if(isCloseOfNext)
			targetPatrolPoint.OnObjectStayOnPatrolPoint(gameObject);

		// Change the target patrol point if needed

		if(isCloseOfNext)
		{
			int newIdx = NextPatrolPointIndex(state._targetPatrolPointIndex);

			PatrolPoint newPatrolPoint = _patrolPoints[newIdx];

			if(newIdx != state._targetPatrolPointIndex)
				newPatrolPoint.OnObjectStartHeading(gameObject);
			else
				newPatrolPoint.OnObjectHeading(gameObject);

			state._targetPatrolPointIndex = newIdx;
		}
	
		state.m_previousPosition = gameObject.transform.position;

		MoveGameObject(gameObject,state);

		RotateGameObject(gameObject,state);

		return _patrolPoints[state._targetPatrolPointIndex].transform.position;
	}

	private int NextPatrolPointIndex(int idx)
	{
		if(idx < _patrolPoints.Count-1)
			return idx+1;

		switch(_patrolType)
		{
		case Type.ONE_WAY: return idx;
		//case Type.REVERSE: return idx-1;
		case Type.LOOP   : return 0;
		default: return 0;
		}
	}

	private void MoveGameObject(GameObject obj,PatrolState state)
	{
		Vector3 target = _patrolPoints[state._targetPatrolPointIndex].transform.position;

		Vector3 vel = (target - obj.transform.position).normalized * state._velocity;

		if(_ignoreY) vel.y = obj.rigidbody.velocity.y;

		obj.rigidbody.velocity = vel;
	}

	private void RotateGameObject(GameObject obj , PatrolState state)
	{
		Vector3 pos = _patrolPoints[state._targetPatrolPointIndex].transform.position;

		pos.y = obj.transform.position.y;

		obj.transform.LookAt(pos);
	}

	#endregion

	#region GUI

	void DrawGizmos(bool selected)
	{
		Gizmos.color = Color.blue;
		for(int i= 0 ; i< _patrolPoints.Count-1 ; i++)
		{
			Gizmos.DrawLine(_patrolPoints[i].transform.position,
			                _patrolPoints[i+1].transform.position);
		}
	}

	[InspectorButton("Add Point")]
	private void AddPatrolPoint()
	{
		GameObject go = new GameObject("PatrolPoint_"+_patrolPoints.Count, new System.Type[]{typeof(PatrolPoint)});

		go.transform.parent = this.transform;

		if(_patrolPoints.Count > 0)
			go.transform.position = _patrolPoints[_patrolPoints.Count-1].transform.position + Vector3.right;
		else 
			go.transform.localPosition = Vector3.zero;

		_patrolPoints.Add(go.GetComponent<PatrolPoint>());
	}

	#endregion
}
