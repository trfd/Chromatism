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
	public MeshFilter m_meshFilter;

#if UNITY_EDITOR

	[InspectorButton("PrecomputeExplosion")]
	void PrecomputeExplodedMesh()
	{
		if(m_meshFilter == null)
			return;

		Mesh originalMesh = m_meshFilter.mesh;

		Mesh explMesh = new Mesh();

		explMesh.Clear();

		int[] triangles = new int[originalMesh.triangles.Length];

		Vector3[] vertices = new Vector3[triangles.Length];
		Vector3[] normals  = new Vector3[triangles.Length];

		Vector2[] uvs = new Vector2[triangles.Length];

		int triangleCount = triangles.Length / 3;

		for(int i=0 ; i<triangleCount ; i++)
		{
			triangles[3*i]   = 3*i;
			triangles[3*i+1] = 3*i+1;
			triangles[3*i+2] = 3*i+2;

			vertices[3*i]   = originalMesh.vertices[originalMesh.triangles[3*i]];
			vertices[3*i+1] = originalMesh.vertices[originalMesh.triangles[3*i+1]];
			vertices[3*i+2] = originalMesh.vertices[originalMesh.triangles[3*i+2]];

			Vector3 normal = 0.333f * originalMesh.normals[originalMesh.triangles[3*i]]   +
							 0.333f * originalMesh.normals[originalMesh.triangles[3*i+1]] + 
							 0.333f * originalMesh.normals[originalMesh.triangles[3*i+2]];

			normals[3*i]   = normal;
			normals[3*i+1] = normal;
			normals[3*i+2] = normal;

			uvs[3*i]   = originalMesh.uv[originalMesh.triangles[3*i]];
			uvs[3*i+1] = originalMesh.uv[originalMesh.triangles[3*i+1]];
			uvs[3*i+2] = originalMesh.uv[originalMesh.triangles[3*i+2]];
		}

		explMesh.vertices  = vertices;
		explMesh.triangles = triangles;
		explMesh.normals   = normals;
		explMesh.uv        = uvs;

		UnityEditor.AssetDatabase.CreateAsset(explMesh,"Assets/Mesh_ExplodedEnemy.asset");
		UnityEditor.AssetDatabase.SaveAssets();
	}

#endif
}
