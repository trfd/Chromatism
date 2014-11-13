//
// GunShootEffect.cs
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

[RequireComponent(typeof(Light))]
public class GunShootEffect : MonoBehaviour
{
	#region Private Members

	private Timer m_timer;

	private Light m_light;

	private bool m_animatorDirty;

	#endregion

	public Animator m_animator;

	public float _maxLightIntensity = 6.0f;
	public float _lightDuration = 0.1f;

	void Start()
	{
		m_timer = new Timer();

		m_light = GetComponent<Light>();

		GPEventManager.Instance.Register("PlayerWeaponShoot",OnPlayerWeaponShoot);
	}

	void Update()
	{
		if(m_animatorDirty)
		{
			m_animator.SetBool("Shoot",false);
			m_animatorDirty = false;
		}

		if(!m_timer.IsElapsedLoop)
			m_light.intensity = Mathf.Lerp(0f,_maxLightIntensity,1.0f-m_timer.CurrentNormalized);
		else
			m_light.intensity = 0f;
	}

	void OnPlayerWeaponShoot(string evName, GPEvent evt)
	{
		m_timer.Reset(_lightDuration);
		m_animator.SetBool("Shoot",true);
		m_animatorDirty = true;
	}
}
