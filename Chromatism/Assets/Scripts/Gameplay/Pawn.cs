//
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

[RequireComponent(typeof(EntityProperties))]
public class Pawn : MonoBehaviour
{
	#region Private Members

	private EntityProperties m_properties;

	#endregion

	#region Public Members

	/// <summary>
	/// The coeficient of color loss when hit by a bullet.
	/// </summary>
	[Tooltip("The coeficient of color loss when hit by a bullet")]
	public float _colorLossCoef = 1f;

	/// <summary>
	/// The coeficient of color gain when hit by a bullet
	/// </summary>
	[Tooltip("The coeficient of color gain when hit by a bullet")]
	public float _colorGainCoef = 1f;

	#endregion

	#region Properties

	public EntityProperties Properties
	{
		get{ return m_properties; }
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

	public void HitByBullet(Bullet bullet)
	{
		if(bullet.IsUsed)
			return;

		EntityProperties otherProperties = bullet.Owner.GetComponent<EntityProperties>();

		m_properties.ColorChannel0 -= _colorLossCoef * bullet.Damages * otherProperties.ColorChannel0;
		m_properties.ColorChannel1 -= _colorLossCoef * bullet.Damages * otherProperties.ColorChannel1;
		m_properties.ColorChannel2 -= _colorLossCoef * bullet.Damages * otherProperties.ColorChannel2;
	}

	public void HitPawn(Pawn other, Bullet bullet)
	{
		if(bullet.IsUsed)
			return;

		m_properties.ColorChannel0 += _colorGainCoef * bullet.Damages * other.Properties.ColorChannel0;
		m_properties.ColorChannel1 += _colorGainCoef * bullet.Damages * other.Properties.ColorChannel1;
		m_properties.ColorChannel2 += _colorGainCoef * bullet.Damages * other.Properties.ColorChannel2;
	}

	#endregion
}
