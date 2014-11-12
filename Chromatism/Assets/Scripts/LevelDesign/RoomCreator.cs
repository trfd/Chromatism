//
// RoomCreator.cs
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

public class RoomCreator : MonoBehaviour
{
	public const float CellSize = 1.0f;

	#region Private Member

	private GameObject m_originalPrefabSelected;

	string[] m_prefabNames;

	#endregion

	#region Public Members

	public GameObject _editingPlane;

	public Transform _rootNode;
	public Transform _setNode;

	public int _width;
	public int _height;
	public int _depth;

	public List<GameObject> _prefabs;

	#endregion

	#region Properties

	public int EditingHeight
	{
		get; set;
	}

	public string[] Prefabs
	{
		get
		{ 
			if(m_prefabNames == null)
				CreatePrefabList();
			return m_prefabNames; 
		}
	}

	public GameObject CurrentSelectedPrefab
	{
		get; set;
	}

	#endregion

	#region MonoBehaviour

	void Start()
	{
		Destroy(this.gameObject);
	}

	void Update()
	{
	}

	#endregion

	#region Interface

	public void CreatePrefabList()
	{
		List<string> prefabName = new List<string>();

		foreach(GameObject obj in _prefabs)
		{
			prefabName.Add(obj.name);
		}

		m_prefabNames = prefabName.ToArray();
	}


	public void StartCreatePrefabAtIndex(int idx)
	{
		m_originalPrefabSelected = _prefabs[idx];

		DuplicateCurrentPrefab();
	}

	public void DuplicateCurrentPrefab()
	{
		CurrentSelectedPrefab = (GameObject) GameObject.Instantiate(m_originalPrefabSelected);

		CurrentSelectedPrefab.transform.parent = _setNode;

		CurrentSelectedPrefab.name = m_originalPrefabSelected.name;
	}

	#endregion
}
