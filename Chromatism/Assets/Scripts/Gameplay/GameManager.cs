//
// GameManager.cs
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

public class GameManager : MonoBehaviour
{
	#region Singleton
	
	private static GameManager m_instance;
	
	public static GameManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<GameManager>();
				
				DontDestroyOnLoad(m_instance.gameObject);
			}
			
			return m_instance;
		}
	}
	
	void Awake() 
	{
		if(m_instance == null)
		{
			m_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}

	#endregion 

	#region Public Member

	public float _enemyOrbLossChannel0;
	public float _enemyOrbLossChannel1;
	public float _enemyOrbLossChannel2;

	#endregion

	#region MonoBehaviour

	void Start()
	{
		Screen.lockCursor = true;
	}

	#endregion		
}
