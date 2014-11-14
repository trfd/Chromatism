//
// EnemyDeathRendering.cs
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

[RequireComponent(typeof(EnemyBehaviour))]
public class EnemyDeathRendering : MonoBehaviour
{
	private EnemyBehaviour m_enemy;

	private Timer m_timer;

	private bool m_isPlayingHit;
	private bool m_isPlayingDie;

	private int m_coefID;

	public Renderer _renderer;
	
	public float _hitDuration;
	public float _dieDuration;

	public AnimationCurve _hitAnimation;
	public AnimationCurve _dieAnimation;

	void Start()
	{
		m_timer = new Timer();

		Pawn pawn = GetComponent<Pawn>();

		pawn.OnPawnHit += OnEnemyHit;
		pawn.OnPawnDie += OnEnemyDie;

		m_coefID = Shader.PropertyToID("_AberrationCoef");
	}

	void Update()
	{
		if(!m_timer.IsElapsedLoop)
		{
			if(m_isPlayingDie)
				_renderer.material.SetFloat(m_coefID,_dieAnimation.Evaluate(m_timer.CurrentNormalized));
			else if(m_isPlayingHit)
				_renderer.material.SetFloat(m_coefID,_hitAnimation.Evaluate(m_timer.CurrentNormalized));
		}

		if(m_timer.IsElapsedLoop)
		{
			if(m_isPlayingHit)
				_renderer.material.SetFloat(m_coefID,0f);
			
			m_isPlayingDie = false;
			m_isPlayingHit = false;
		}
	}

	void OnEnemyHit(Pawn pawn)
	{
		if(pawn.IsDead)
			return;

		m_timer.Reset(_hitDuration);

		m_isPlayingDie = false;
		m_isPlayingHit = true;
	}

	void OnEnemyDie(Pawn pawn)
	{
		m_timer.Reset(_dieDuration);

		m_isPlayingDie = true;
		m_isPlayingHit = false;
	}

	[InspectorButton("Hit")]
	void TestHit()
	{
		m_timer.Reset(_hitDuration);
		
		m_isPlayingDie = false;
		m_isPlayingHit = true;
	}

	[InspectorButton("Death")]
	void TestDeath()
	{
		m_timer.Reset(_dieDuration);
		
		m_isPlayingDie = true;
		m_isPlayingHit = false;
	}
}
