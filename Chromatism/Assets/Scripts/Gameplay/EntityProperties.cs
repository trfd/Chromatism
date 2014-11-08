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

public class EntityProperties : MonoBehaviour
{
	#region Color Channel Properties

	/// <summary>
	/// Entity's level of color in first color channel.
	/// </summary>
	/// <value>The color channel0.</value>
	public float ColorChannel0 { get; set; }

	/// <summary>
	/// Entity's level of color in second color channel.
	/// </summary>
	/// <value>The color channel0.</value>
	public float ColorChannel1 { get; set; }

	/// <summary>
	/// Entity's level of color in third color channel.
	/// </summary>
	/// <value>The color channel0.</value>
	public float ColorChannel2 { get; set; }

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
	public float WeaponDamages { get; set; }
	
	/// <summary>
	/// Fire rate of weapons held by entity.
	/// </summary>
	/// <value>The fire rate.</value>
	public float WeaponFireRate { get; set; }
	
	/// <summary>
	/// Precision of weapons held by entity.
	/// </summary>
	/// <value>The precision.</value>
	public float WeaponPrecision { get; set; }
	
	/// <summary>
	/// Range of shoots
	/// </summary>
	/// <value>The range.</value>
	public float WeaponRange { get; set; }

	/// <summary>
	/// Duration of weapon reloading.
	/// </summary>
	/// <value>The weapon reload time.</value>
	public float WeaponReloadDuration { get; set; }

	#endregion

}
