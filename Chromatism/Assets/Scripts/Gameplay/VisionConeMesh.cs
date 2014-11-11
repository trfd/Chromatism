//
// VisionConeMesh.cs
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

[RequireComponent(typeof(MeshFilter))]
public class VisionConeMesh : MonoBehaviour
{
	#region Private Members

	private MeshFilter m_meshFilter;

	private BasicEnemyBehaviour m_enemyBehaviour;

	private int m_meshPrecision = 10;

	#endregion

	public void GenerateMesh()
	{
		if(m_meshFilter == null)
			m_meshFilter = GetComponent<MeshFilter>();

		if(m_enemyBehaviour == null)
			m_enemyBehaviour = GetComponentInParent<BasicEnemyBehaviour>();

		if(m_enemyBehaviour == null)
		{
			Debug.Log("VisionConeMesh requires BasicEnemyBehaviour component in parent");
			return;
		}

		float deltaAngle = 2f*m_enemyBehaviour._coneViewAngle / m_meshPrecision;

		Mesh newMesh = new Mesh();

		// Create vertices

		List<Vector3> vertices = new List<Vector3>();

		vertices.Add(Vector3.zero);

		Vector3 tmp1 = Vector3.zero;

		for(float angle = -m_enemyBehaviour._coneViewAngle ; 
		    angle<m_enemyBehaviour._coneViewAngle+deltaAngle ;
		    angle += deltaAngle)
		{
			tmp1.x = m_enemyBehaviour._coneViewRange * Mathf.Sin(angle*Mathf.Deg2Rad);
			tmp1.z = m_enemyBehaviour._coneViewRange * Mathf.Cos(angle*Mathf.Deg2Rad);

			vertices.Add(tmp1);
		}

		// Create triangles and normals

		List<int> triangles = new List<int>();

		for(int idx= 0 ; idx <= m_meshPrecision ; idx++)
		{
			triangles.Add(0);
			triangles.Add(idx);
			triangles.Add(idx+1);
		}

		// Assign to mesh

		newMesh.vertices = vertices.ToArray();
		newMesh.triangles = triangles.ToArray();

		newMesh.RecalculateNormals();

		m_meshFilter.mesh = newMesh;

		transform.localScale = new Vector3(1f/m_enemyBehaviour.transform.localScale.x,
		                                   1f/m_enemyBehaviour.transform.localScale.y,
		                                   1f/m_enemyBehaviour.transform.localScale.z);
	}
}
