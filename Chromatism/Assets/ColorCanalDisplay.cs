//
// ColorCanalDisplay.cs
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

[RequireComponent(typeof(MeshFilter))]
public class ColorCanalDisplay : MonoBehaviour
{
	private Timer m_updateTimer;

	private PlayerBehaviour m_player;

	private MeshFilter m_mesh;

	private float[] m_data;
	private Vector3[] m_vertices;
	private Vector3[] m_normals;
	private int[] m_triangles;
	private Vector2[] m_uv;

	private int m_currIdx;

	public TextMesh _textMesh;

	public float _xScale = 1.0f;
	public float _yScale = 1.0f;
	public float _updateFrequency;
	public int _channel;
	public int _steps = 12;

	void Start()
	{
		m_updateTimer = new Timer();

		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		m_mesh = GetComponent<MeshFilter>();

		m_data = new float[_steps];

		m_currIdx = 0;

		AllocMesh();

		GPEventManager.Instance.Register("PlayerTouched",OnPlayerTouched);
		GPEventManager.Instance.Register("PlayerPickOrb",OnPlayerPickOrb);
	}

	void Update()
	{
		if(m_updateTimer != null && m_updateTimer.IsElapsedLoop && m_currIdx<_steps-1)
		{
			AddData();
			m_updateTimer = null;
		}

		_textMesh.text = ((int)(100f*m_player.Properties.ColorValueChannel(_channel))).ToString();
	}

	#region Events

	void OnPlayerTouched(string evtName, GPEvent evt)
	{
		AddData();
	}

	void OnPlayerPickOrb(string evtName, GPEvent evt)
	{
		AddData();
	}

	#endregion

	void AddData()
	{
		m_data[m_currIdx] = m_player.Properties.ColorValueChannel(_channel);
		m_currIdx = (m_currIdx+1)%_steps;

		ComputeMesh();
	}

	void AllocMesh()
	{
		m_vertices = new Vector3[_steps*2];
		m_uv = new Vector2[_steps*2];
		m_triangles = new int[(_steps-1)*6];
		m_normals = new Vector3[_steps*2];

		for(int i = 0 ; i < m_data.Length ; i++)
		{
			// Triangle
			
			if(i == _steps-1)
				continue;

			m_triangles[6*i]   = i+1;
			m_triangles[6*i+1] = i+1+_steps;
			m_triangles[6*i+2] = i+_steps;
		
			m_triangles[6*i+3] = i;
			m_triangles[6*i+4] = i+1;
			m_triangles[6*i+5] = i+_steps;
		}
	
		for(int i=0 ; i<2*_steps ; i++)
			m_normals[i] = Vector3.forward;

	}

	void ComputeMesh()
	{
		Vector3 tmp = Vector3.zero;

		for(int i = 0 ; i < m_data.Length ; i++)
		{
			int idx = (m_currIdx+i)%_steps;

			// Value vertices

			tmp.x = _xScale * i;
			tmp.y = _yScale * m_data[idx];

			m_vertices[i] = tmp;

			// Base vertices

			tmp.x = _xScale * i;
			tmp.y = 0f;

			m_vertices[i+_steps] = tmp;
		}

		// Reset mesh:

		m_mesh.mesh.Clear();

		m_mesh.mesh.vertices  = m_vertices;
		m_mesh.mesh.normals   = m_normals;
		m_mesh.mesh.uv        = m_uv;
		m_mesh.mesh.triangles = m_triangles;
	}
}
