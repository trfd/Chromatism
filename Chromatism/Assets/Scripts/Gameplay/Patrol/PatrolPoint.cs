//
// PatrolPoint.cs
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

public class PatrolPoint : MonoBehaviour
{
	#region Private Members

	PatrolPointBehaviour[] m_behaviours;

	#endregion

	#region MonoBehaviour

	void Start()
	{
		m_behaviours = GetComponents<PatrolPointBehaviour>();
	}

	void OnDrawGizmos() 
	{
		DrawGizmos(false);
	}

	void OnDrawGizmosSelected()
	{
		DrawGizmos(true);
	}

	#endregion

	#region Gizmos

	void DrawGizmos(bool selected)
	{
		if(selected)
			Gizmos.color = Color.blue;
		else
			Gizmos.color = Color.grey;

		Gizmos.DrawSphere(transform.position, 0.15f);
	}

	#endregion

	#region Interface

	/// <summary>
	/// Raises when a start heading toward the patrol point
	/// </summary>
	/// <param name="gameObject">Object.</param>
	public virtual void OnObjectStartHeading(GameObject gameObject)
	{
		foreach(PatrolPointBehaviour ppbh in m_behaviours)
			ppbh.OnObjectStartHeading(gameObject);
	}
	
	/// <summary>
	/// Raises each time the gameObject is heading toward the patrol point.
	/// </summary>
	/// <param name="gameObject">Object.</param>
	public virtual void OnObjectHeading(GameObject gameObject)
	{
		foreach(PatrolPointBehaviour ppbh in m_behaviours)
			ppbh.OnObjectHeading(gameObject);
	}
	
	/// <summary>
	/// Raises a gameObject enters patrol point.
	/// </summary>
	/// <param name="gameObject">Object.</param>
	public virtual void OnObjectEnterPatrolPoint(GameObject gameObject)
	{
		foreach(PatrolPointBehaviour ppbh in m_behaviours)
			ppbh.OnObjectEnterPatrolPoint(gameObject);
	}
	
	/// <summary>
	/// Raises each fram a gameObject stays on patrol point.
	/// </summary>
	/// <param name="gameObject">Object.</param>
	public virtual void OnObjectStayOnPatrolPoint(GameObject gameObject)
	{
		foreach(PatrolPointBehaviour ppbh in m_behaviours)
			ppbh.OnObjectStayOnPatrolPoint(gameObject);
	}
	
	/// <summary>
	/// Raises when a gameObject exit patrol point.
	/// </summary>
	/// <param name="gameObject">Object.</param>
	public virtual void OnObjectExitPatrolPoint(GameObject gameObject)
	{
		foreach(PatrolPointBehaviour ppbh in m_behaviours)
			ppbh.OnObjectExitPatrolPoint(gameObject);
	}

	#endregion
}
