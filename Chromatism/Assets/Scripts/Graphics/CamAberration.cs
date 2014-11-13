//
// CamAberration.cs
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

public class CamAberration : MonoBehaviour
{
	private bool m_isAnimating;
	private PlayerBehaviour m_player;
	private Timer m_animTimer;

	private Timer m_deathTimer;

	private int m_coefID;
	private int m_redCoefID;
	private int m_greenCoefID;
	private int m_blueCoefID;
	
	public float _animationDuration;
	public float _deathDuration;


	public AnimationCurve _hitCoefAnimation;
	public AnimationCurve _deathCoefAnimation;

	public Material _material;

	void Start()
	{
		m_animTimer = new Timer();
		m_player = ((GameObject) GameObject.FindGameObjectWithTag("Player")).GetComponent<PlayerBehaviour>();

		m_coefID      = Shader.PropertyToID("_Coef");
		m_redCoefID   = Shader.PropertyToID("_RedCoef");
		m_greenCoefID = Shader.PropertyToID("_GreenCoef");
		m_blueCoefID  = Shader.PropertyToID("_BlueCoef");

		GPEventManager.Instance.Register("PlayerTouched",OnPlayerTouched);
		GPEventManager.Instance.Register("PlayerDied",OnPlayerDied);

		_material.SetFloat(m_coefID,      0);
		_material.SetFloat(m_redCoefID,   0);
		_material.SetFloat(m_greenCoefID, 0);
		_material.SetFloat(m_blueCoefID,  0);
	}

	void Update()
	{
		_material.SetFloat(m_redCoefID, Mathf.Lerp(0f,0.01f,m_player.Properties.ColorChannel0));
		_material.SetFloat(m_greenCoefID, Mathf.Lerp(0f,0.01f,m_player.Properties.ColorChannel1));
		_material.SetFloat(m_blueCoefID, Mathf.Lerp(0f,0.01f,m_player.Properties.ColorChannel2));

		if(m_deathTimer != null && !m_deathTimer.IsElapsedLoop)
		{
			float value = _deathCoefAnimation.Evaluate(1.0f-m_deathTimer.CurrentNormalized);
			_material.SetFloat(m_coefID,value);
		}
		else if (m_deathTimer != null && m_deathTimer.IsElapsedLoop)
		{
			GPEventManager.Instance.Raise("RestartLevel",new GPEvent());
		}

		if(m_isAnimating && !m_animTimer.IsElapsedLoop)
		{
			float value = _hitCoefAnimation.Evaluate(1.0f-m_animTimer.CurrentNormalized);
			_material.SetFloat(m_coefID,value);
		}
		else if(m_isAnimating && m_animTimer.IsElapsedLoop)
		{
			m_isAnimating = false;
			_material.SetFloat(m_coefID,0);
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, _material);
	}

	void OnPlayerTouched(string str, GPEvent evt)
	{
		StartAnim();

		_material.SetFloat(m_redCoefID, Mathf.Lerp(0f,0.01f,m_player.Properties.ColorChannel0));
		_material.SetFloat(m_greenCoefID, Mathf.Lerp(0f,0.01f,m_player.Properties.ColorChannel1));
		_material.SetFloat(m_blueCoefID, Mathf.Lerp(0f,0.01f,m_player.Properties.ColorChannel2));
	}

	void OnPlayerDied(string str, GPEvent evt)
	{
		m_deathTimer = new Timer(_deathDuration);
	}

	[InspectorButton("Test Anim")]
	void StartAnim()
	{
		m_animTimer.Reset(_animationDuration);

		m_isAnimating = true;
	}

	[InspectorButton("Test Death Anim")]
	void StartDeathAnim()
	{
		m_deathTimer = new Timer(_deathDuration);
	}
}
