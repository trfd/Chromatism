//
// GPEventManager.cs
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

public class GPEvent
{
}

public class GPEventManager : MonoBehaviour
{
	#region Singleton
	
	private static GPEventManager m_instance;
	
	public static GPEventManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<GPEventManager>();
				
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
			m_instance.Init();
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	
	#endregion 

	#region Delegates

	public delegate void EventDelegate(string evtName, GPEvent evt);

	#endregion

	#region Private Members

	private Dictionary<string,EventDelegate> m_eventMap;

	#endregion

	private void Init()
	{
		m_eventMap = new Dictionary<string, EventDelegate>();
	}

	#region Registration

	public void Register(string evtName, EventDelegate del)
	{
		try
		{
			m_eventMap[evtName] += del;
		}
		catch(KeyNotFoundException)
		{
			m_eventMap.Add(evtName,del);
		}
	}

	public void Unregister(string evtName, EventDelegate del)
	{
		try
		{
			m_eventMap[evtName] -= del;
		}
		catch(KeyNotFoundException)
		{}
	}

	#endregion

	#region Post Events

	public void Raise(string name, GPEvent evt)
	{
		EventDelegate value;

		if(m_eventMap.TryGetValue(name,out value))
			value(name,evt);
	}

	#endregion
}
