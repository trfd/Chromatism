//
// ExplosionMesh.cs
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

public class ExplosionMesh : MonoBehaviour
{
	private Pawn m_pawn;
	private Timer m_timer;

	private static int m_sizeID;
	private static int m_randomnessID;

	public MeshRenderer m_renderer;

	public float _explosionDuration;
	public AnimationCurve _explosionCurve;
	public AnimationCurve _randomnessCurve;

	void Start()
	{
		m_pawn = GetComponent<Pawn>();

		m_pawn.OnPawnDie += OnEnemyDie;

		m_sizeID = Shader.PropertyToID("_Size");
		m_randomnessID = Shader.PropertyToID("_Randomness");
	
		m_renderer.material.SetFloat(m_randomnessID,0f);
		m_renderer.material.SetFloat(m_sizeID,0f);
	}
	
	void Update()
	{
		if(m_timer != null && !m_timer.IsElapsedLoop)
		{	
			if(!m_renderer.enabled)
				m_renderer.enabled = true;

			m_renderer.material.SetFloat(m_randomnessID,_randomnessCurve.Evaluate(1f-m_timer.CurrentNormalized));
			m_renderer.material.SetFloat(m_sizeID,_explosionCurve.Evaluate(1f-m_timer.CurrentNormalized));
		}
	}

	void OnEnemyDie(Pawn pawn)
	{
		m_renderer.enabled = true;
		m_renderer.material.SetFloat(m_randomnessID,0);
		m_renderer.material.SetFloat(m_sizeID,0);

		m_timer = new Timer(_explosionDuration);
	}

	[InspectorButton("Explosion")]
	void TestAnim()
	{
		m_timer = new Timer(_explosionDuration);
	}

}
