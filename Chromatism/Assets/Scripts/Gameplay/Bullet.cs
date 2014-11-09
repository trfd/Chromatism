//
// Bullet.cs
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

public class Bullet : PoolableObject
{
	#region Private Members

	/// <summary>
	/// Starting Point of bullet.
	/// </summary>
	private Vector3 m_spawnPoint;

	/// <summary>
	/// Holds whether or not the bullet has hit something.
	/// </summary>
	private bool m_isUsed = false;

	#endregion

	#region Properties

	public GameObject Owner
	{
		get; set;
	}

	public Weapon ParentWeapon
	{
		get; set;
	}

	public float Damages
	{
		get; set;
	}

	public float Range
	{
		get; set;
	}

	[InspectorLabel]
	public Vector3 Velocity
	{
		get; set;
	}

	public bool IsUsed
	{
		get{ return m_isUsed;  }
	}

	#endregion

	#region MonoBehaviour

	void Start()
	{
	}

	void Update()
	{
		if(m_isUsed)
			return;

		// Check still in range

		if(Vector3.Distance(m_spawnPoint,transform.position) > Range)
			OutOfRange();
		else
			transform.position += Time.deltaTime * Velocity;
	}

	void OnCollisionEnter(Collision coll)
	{
		TreatCollision(coll);
	}

	void OnCollisionStay(Collision coll)
	{
		TreatCollision(coll);
	}

	void OnCollisionExit(Collision coll)
	{
		TreatCollision(coll);
	}

	#endregion

	#region Accessors

	#endregion

	#region PoolableObject Override

	/// <summary>
	/// Called whenever the object is picked up in the pool.
	/// This should be used for activating stuff.
	/// </summary>
	protected override void OnPoolInit()
	{
		collider.enabled = true;
		renderer.enabled = true;
		this.enabled = true;

		m_isUsed = false;
	}

	/// <summary>
	/// Called whenever the object is 
	/// This should be used for deactivating stuff.
	/// </summary>
	protected override void OnPoolClear()
	{
		m_isUsed = true;
		m_spawnPoint = Vector3.zero;

		Damages  = 0f;
		Range    = 0f;
		Owner    = null;
		Velocity = Vector3.zero;

		collider.enabled = false;
		renderer.enabled = false;
		this.enabled = false;
		rigidbody.Sleep();
	}

	#endregion

	#region Bullet Methods

	public void SpawnAt(Vector3 position)
	{
		m_spawnPoint = position;
		transform.position = position;
	}

	/// <summary>
	/// Called when bullet goes out of its range.
	/// </summary>
	private void OutOfRange()
	{
		//...

		IsPoolable = true;
	}

	private void SetUsed()
	{
		m_isUsed = true;

		// Do something ...

		IsPoolable = true;
	}

	private void TreatCollision(Collision collision)
	{
		if(m_isUsed)
			return;
		
		Pawn pawn = collision.gameObject.GetComponentInParent<Pawn>();
		
		if(pawn != null)
		{
			// Avoid collision with self
			if(pawn == ParentWeapon.Owner)
				return;

			pawn.HitByBullet(this);
		}
		
		SetUsed();
	}

	#endregion
}
