//
// EntityProperties.cs
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

[AddComponentMenu("Gameplay/EntityProperties")]
public class EntityProperties : MonoBehaviour
{
	public enum Property
	{
		GRAVITY					= 0,
		
		ENTITY_VELOCITY			= 1,
		ENTITY_DASH_RANGE		= 2,
		
		WEAPON_BULLET_DAMAGES	= 3,
		WEAPON_BULLET_SIZE		= 4,
		WEAPON_BULLET_VELOCITY	= 5,
		WEAPON_BULLET_RANGE		= 6,
		WEAPON_MAGAZINE_SIZE	= 7,
		WEAPON_FIRERATE			= 8,
		WEAPON_PRECISION		= 9,
		WEAPON_RELOAD_DURATION  = 10,

		PROPERTY_COUNT
	}

	public delegate void  PropertySetter(float value);
	public delegate float PropertyGetter();

	[System.Serializable]
	public class PropertyBindingDelegates
	{
		#region Private Members

		private PropertyGetter m_getter;

		private PropertySetter m_setter;

		#endregion

		#region Public Member

		#endregion

		#region Properties

		[UnityEngine.SerializeField]
		public PropertyBinding Binding
		{
			get; set;
		}

		public PropertyGetter Getter
		{
			get{ return m_getter;  }
			set{ m_getter = value; }
		}

		public PropertySetter Setter
		{
			get{ return m_setter;  }
			set{ m_setter = value; }
		}

		#endregion

		#region Constructor

		public PropertyBindingDelegates()
		{
		}

		#endregion

		#region Accessor

		public void SetPropertyValue(float colorValue)
		{
			if(Setter == null)
				return;

			Setter(Binding.MapColorToProperty(colorValue));
		}

		#endregion
	}

	[System.Serializable]
	public class ColorChannel
	{
		#region Private Members

		/// <summary>
		/// Color value
		/// </summary>
		[UnityEngine.SerializeField]
		private float m_colorValue;

		/// <summary>
		/// List of all bindings
		/// </summary>
		internal List<PropertyBindingDelegates> m_bindingDelegates;
	
		#endregion

		#region Properties

		public float ColorValue
		{
			get{ return m_colorValue; }
			set
			{
				m_colorValue = value;
				foreach(PropertyBindingDelegates delegates in m_bindingDelegates)
					delegates.SetPropertyValue(m_colorValue);
			}
		}
	
		#endregion

		#region Constructor

		public ColorChannel()
		{
			m_bindingDelegates = new List<PropertyBindingDelegates>();
		}

		#endregion

		#region Binding Management

		public void SetPropertyBindings(EntityProperties entity, List<PropertyBinding> bindings)
		{
			m_bindingDelegates.Clear();

			foreach(PropertyBinding bind in bindings)
			{
				PropertyBindingDelegates delegates = BindingDelegate(entity,bind);
				m_bindingDelegates.Add(delegates);
			}
		}

		public PropertyBindingDelegates BindingDelegate(EntityProperties entity,PropertyBinding propertyBinding)
		{
			PropertyBindingDelegates bindDelegates = new PropertyBindingDelegates();

			bindDelegates.Binding = propertyBinding;

			switch(propertyBinding._boundProperty)
			{
			case Property.GRAVITY:
				bindDelegates.Setter = (float value) => { entity.Gravity = value; };
				bindDelegates.Getter = () => { return entity.Gravity; };
				break;
			case Property.ENTITY_DASH_RANGE:
				bindDelegates.Setter = (float value) => { entity.EntityDashRange = value; };
				bindDelegates.Getter = () => { return entity.EntityDashRange; };
				break;
			case Property.ENTITY_VELOCITY:
				bindDelegates.Setter = (float value) => { entity.EntityVelocity = value; };
				bindDelegates.Getter = () => { return entity.EntityVelocity; };
				break;
			case Property.WEAPON_BULLET_SIZE:
				bindDelegates.Setter = (float value) => { entity.WeaponBulletSize = value; };
				bindDelegates.Getter = () => { return entity.WeaponBulletSize; };
				break;
			case Property.WEAPON_BULLET_VELOCITY:
				bindDelegates.Setter = (float value) => { entity.WeaponBulletVelocity = value; };
				bindDelegates.Getter = () => { return entity.WeaponBulletVelocity; };
				break;
			case Property.WEAPON_BULLET_DAMAGES:
				bindDelegates.Setter = (float value) => { entity.WeaponBulletDamages = value; };
				bindDelegates.Getter = () => { return entity.WeaponBulletDamages; };
				break;
			case Property.WEAPON_BULLET_RANGE:
				bindDelegates.Setter = (float value) => { entity.WeaponBulletRange = value; };
				bindDelegates.Getter = () => { return entity.WeaponBulletRange; };
				break;
			case Property.WEAPON_FIRERATE:
				bindDelegates.Setter = (float value) => { entity.WeaponFireRate = value; };
				bindDelegates.Getter = () => { return entity.WeaponFireRate; };
				break;
			case Property.WEAPON_MAGAZINE_SIZE:
				bindDelegates.Setter = (float value) => { entity.WeaponMagazineSize = value; };
				bindDelegates.Getter = () => { return entity.WeaponMagazineSize; };
				break;
			case Property.WEAPON_PRECISION:
				bindDelegates.Setter = (float value) => { entity.WeaponPrecision = value; };
				bindDelegates.Getter = () => { return entity.WeaponPrecision; };
				break;
			case Property.WEAPON_RELOAD_DURATION:
				bindDelegates.Setter = (float value) => { entity.WeaponReloadDuration = value; };
				bindDelegates.Getter = () => { return entity.WeaponReloadDuration; };
				break;
			default: 
				Debug.LogError("Property binding not supported: "+propertyBinding._boundProperty);
				break;
			}

			return bindDelegates;
		}

		#endregion
	}

	#region Private Members

	[UnityEngine.SerializeField]
	private ColorChannel[] m_channels;

	
	[UnityEngine.SerializeField]
	private float[] m_defaultValues;


	#endregion

	#region Public Members

	public bool _currentValueFoldout;
	public bool _defaultValueFoldout;

	#endregion

	#region Color Channel Properties

	/// <summary>
	/// Entity's level of color in first color channel.
	/// </summary>
	/// <value>The color channel0.</value>
	public float ColorChannel0 
	{
		get{ return m_channels[0].ColorValue;  }
		set{ m_channels[0].ColorValue = value; }
	}

	/// <summary>
	/// Entity's level of color in second color channel.
	/// </summary>
	/// <value>The color channel0.</value>
	public float ColorChannel1 
	{
		get{ return m_channels[1].ColorValue;  }
		set{ m_channels[1].ColorValue = value; }
	}

	/// <summary>
	/// Entity's level of color in third color channel.
	/// </summary>
	/// <value>The color channel0.</value>
	public float ColorChannel2 
	{
		get{ return m_channels[2].ColorValue;  }
		set{ m_channels[2].ColorValue = value; }
	}

	#endregion

	#region Global Properties

	/// <summary>
	/// Boost of gravity.
	/// </summary>
	/// <value>The gravity.</value>
	public float Gravity { get; set; }

	#endregion

	#region Entity Properties

	/// <summary>
	/// Maximum of magnitude of entity's velocity vector.
	/// </summary>
	/// <value>The entity velocity.</value>
	public float EntityVelocity { get; set; }

	/// <summary>
	/// Distance of dash moves.
	/// </summary>
	/// <value>The entity dash range.</value>
	public float EntityDashRange { get; set; }

	#endregion

	#region Weapon Propeties

	/// <summary>
	/// Magnitude of bullet's velocity.
	/// </summary>
	/// <value>The weapon bullet velocity.</value>
	public float WeaponBulletVelocity { get; set; }

	/// <summary>
	/// Size of bullets.
	/// </summary>
	/// <value>The size of the weapon bullet.</value>
	public float WeaponBulletSize { get; set; }

	/// <summary>
	/// Damages of weapons held by entity.
	/// </summary>
	/// <value>The damages.</value>
	public float WeaponBulletDamages { get; set; }
	
	/// <summary>
	/// Fire rate of weapons held by entity.
	/// </summary>
	/// <value>The fire rate.</value>
	public float WeaponFireRate { get; set; }

	/// <summary>
	/// Number of bullet a weapon has in a magazine
	/// </summary>
	/// <value>The size of the weapon magazine.</value>
	public float WeaponMagazineSize { get; set; }

	/// <summary>
	/// Precision of weapons held by entity.
	/// </summary>
	/// <value>The precision.</value>
	public float WeaponPrecision { get; set; }
	
	/// <summary>
	/// Range of shoots
	/// </summary>
	/// <value>The range.</value>
	public float WeaponBulletRange { get; set; }

	/// <summary>
	/// Duration of weapon reloading.
	/// </summary>
	/// <value>The weapon reload time.</value>
	public float WeaponReloadDuration { get; set; }

	#endregion

	#region Constructor

	public EntityProperties()
	{
		m_channels = new ColorChannel[3]
		{
			new ColorChannel(),
			new ColorChannel(),
			new ColorChannel()
		};

		m_defaultValues = new float[(int)Property.PROPERTY_COUNT];
	}

	#endregion

	#region MonoBehaviour

	public void Awake()
	{
		SetDefaultValues();;
	}

	public void Start()
	{
		SetChannelBindings();
	}

	#endregion

	#region Channel Binding

	public void SetChannelBindings()
	{
		m_channels[0].SetPropertyBindings(this, EntityPropertiesManager.Instance._channel0Bindings);
		m_channels[1].SetPropertyBindings(this, EntityPropertiesManager.Instance._channel1Bindings);
		m_channels[2].SetPropertyBindings(this, EntityPropertiesManager.Instance._channel2Bindings);
	}

	public Property[] ChannelBindings(int channel)
	{
		int count = m_channels[channel].m_bindingDelegates.Count;

		Property[] properties = new Property[count];

		for(int i=0 ; i<count ; i++)
		{
			properties[i] = m_channels[channel].m_bindingDelegates[i].Binding._boundProperty;
		}
	
		return properties;
	}

	#endregion

	private void SetDefaultValues()
	{
		if(m_defaultValues.Length != (int)Property.PROPERTY_COUNT)
		{
			Debug.LogError("Default Value array is invalid");
			return;
		}

		for(int i= 0 ; i<(int) Property.PROPERTY_COUNT ; i++)
		{
			SetPropertyValue((Property)i,m_defaultValues[i]);
		}
	}

	#region Accessors

	public ColorChannel Channel(int channel)
	{
		return m_channels[channel];
	}

	/// <summary>
	/// Gets the level of color in specified channel.
	/// </summary>
	/// <returns>The color value.</returns>
	/// <param name="channel">Channel.</param>
	public float ColorValueChannel(int channel)
	{
		return m_channels[channel].ColorValue;
	}

	/// <summary>
	/// Sets the value channel.
	/// </summary>
	/// <param name="channel">Channel.</param>
	/// <param name="color">Color.</param>
	public void SetColorValueChannel(int channel,float color)
	{
		m_channels[channel].ColorValue = Mathf.Clamp01(color);
	}

	public void SetPropertyValue(Property prop, float value)
	{
		switch(prop)
		{
		case Property.GRAVITY: 				  Gravity = value; break;
		case Property.ENTITY_DASH_RANGE:      EntityDashRange = value; break;
		case Property.ENTITY_VELOCITY:        EntityVelocity = value; break;
		case Property.WEAPON_BULLET_SIZE:     WeaponBulletSize = value; break;
		case Property.WEAPON_BULLET_VELOCITY: WeaponBulletVelocity = value; break;
		case Property.WEAPON_BULLET_DAMAGES:  WeaponBulletDamages = value; break;
		case Property.WEAPON_BULLET_RANGE:    WeaponBulletRange = value; break;
		case Property.WEAPON_FIRERATE:		  WeaponFireRate = value; break;
		case Property.WEAPON_MAGAZINE_SIZE:   WeaponMagazineSize = value; break;
		case Property.WEAPON_PRECISION:		  WeaponPrecision = value; break;
		case Property.WEAPON_RELOAD_DURATION: WeaponReloadDuration = value; break;
		default: 
			Debug.LogError("Property not supported: "+prop);
			break;
		}
	}

	public float PropertyValue(Property prop)
	{
		switch(prop)
		{
		case Property.GRAVITY: 				  return Gravity;
		case Property.ENTITY_DASH_RANGE:      return EntityDashRange;
		case Property.ENTITY_VELOCITY:        return EntityVelocity;
		case Property.WEAPON_BULLET_SIZE:     return WeaponBulletSize;
		case Property.WEAPON_BULLET_VELOCITY: return WeaponBulletVelocity;
		case Property.WEAPON_BULLET_DAMAGES:  return WeaponBulletDamages;
		case Property.WEAPON_BULLET_RANGE:    return WeaponBulletRange;
		case Property.WEAPON_FIRERATE:		  return WeaponFireRate;
		case Property.WEAPON_MAGAZINE_SIZE:	  return WeaponMagazineSize;
		case Property.WEAPON_PRECISION:		  return WeaponPrecision;
		case Property.WEAPON_RELOAD_DURATION: return WeaponReloadDuration;
		default: 
			Debug.LogError("Property not supported: "+prop);
			break;
		}

		return 0f;
	}

	public void ResetDefaultValues()
	{
		m_defaultValues = new float[(int)Property.PROPERTY_COUNT];
	}

	public float DefaultValue(Property prop)
	{
		if(((int)prop) >= m_defaultValues.Length)
			Debug.Log("Out Of Range Property: "+prop.ToString()+" int value:"+((int)prop)+" size: "+m_defaultValues.Length);

		return m_defaultValues[(int) prop];
	}

	public void SetDefaultValue(Property prop, float value)
	{
		m_defaultValues[(int) prop] = value;
	}

	#endregion
}
