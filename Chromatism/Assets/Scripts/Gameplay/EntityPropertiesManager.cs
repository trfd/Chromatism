//
// EntityPropertiesManager.cs
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

[System.Serializable]
public class PropertyBinding
{
	#region Public Members
	
	/// <summary>
	/// Minimum value of binding.
	/// </summary>
	public float _minValue;
	
	/// <summary>
	/// Maximum value of binding.
	/// </summary>
	public float _maxValue;
	
	/// <summary>
	/// The bound property.
	/// </summary>
	public EntityProperties.Property _boundProperty;

	#endregion

	/// <summary>
	/// Maps a color value to property value.
	/// </summary>
	/// <returns>The color to property.</returns>
	/// <param name="value">Value.</param>
	public float MapColorToProperty(float value)
	{
		return Mathf.Lerp(_minValue,_maxValue,value);
	}

	/// <summary>
	/// Maps a property value to color value.
	/// </summary>
	/// <returns>The property to color.</returns>
	/// <param name="value">Value.</param>
	public float MapPropertyToColor(float value)
	{
		return Mathf.InverseLerp(_minValue,_maxValue,value);
	}

}

[AddComponentMenu("Gameplay/EntityPropertiesManager")]
public class EntityPropertiesManager : MonoBehaviour
{
	#region Singleton
	
	private static EntityPropertiesManager m_instance;
	
	public static EntityPropertiesManager Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = GameObject.FindObjectOfType<EntityPropertiesManager>();

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

	#region Delegates

	public delegate void ManagerCallback();

	public ManagerCallback OnBindingsChange;

	#endregion

	#region Public Members

	public List<PropertyBinding> _channel0Bindings;
	public List<PropertyBinding> _channel1Bindings;
	public List<PropertyBinding> _channel2Bindings;

	#endregion
	
	#region Accessors

	public List<PropertyBinding> ChannelBindings(int channel)
	{
		switch(channel)
		{
		case 0 : return _channel0Bindings;
		case 1 : return _channel1Bindings;
		case 2 : return _channel2Bindings;
		default:
			Debug.LogError("Wrong channel number "+channel);
			return null;
		}
	}

	#endregion
}
