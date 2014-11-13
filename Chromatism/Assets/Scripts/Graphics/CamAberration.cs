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

	private int m_coefID;
	private int m_redCoefID;
	private int m_greenCoefID;
	private int m_blueCoefID;
	
	public float _animationDuration;

	public AnimationCurve _hitCoefAnimation;

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
	}

	void Update()
	{
		if(m_isAnimating && !m_animTimer.IsElapsedLoop)
		{
			float value = _hitCoefAnimation.Evaluate(1.0f-m_animTimer.CurrentNormalized);
			Debug.Log(m_animTimer.CurrentNormalized);
			_material.SetFloat(m_coefID,value);
		}

		if(m_isAnimating && m_animTimer.IsElapsedLoop)
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
	}

	[InspectorButton("Test Anim")]
	void StartAnim()
	{
		m_animTimer.Reset(_animationDuration);

		m_isAnimating = true;
	}

}
