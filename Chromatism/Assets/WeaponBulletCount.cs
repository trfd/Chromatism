//
// WeaponBulletCount.cs
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

[RequireComponent(typeof(TextMesh))]
public class WeaponBulletCount : MonoBehaviour
{
	#region Private Members

	private Weapon m_weapon;

	private TextMesh m_mesh;

	#endregion

	#region MonoBehaviour

	void Start()
	{
		m_weapon = GetComponentInParent<Weapon>();

		m_mesh = GetComponent<TextMesh>();
	}

	void Update()
	{
		m_mesh.text = m_weapon.RemainingBullets.ToString();
	}

	#endregion
}
