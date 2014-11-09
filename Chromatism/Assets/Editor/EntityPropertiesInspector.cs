//
// EntityPropertiesInspector.cs
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
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EntityProperties))]
public class EntityPropertiesInspector : Editor
{
	public override void OnInspectorGUI()
	{
		EntityProperties entityProperties = (EntityProperties) target;

		entityProperties.ColorChannel0 = EditorGUILayout.Slider("Channel 0", entityProperties.ColorChannel0, 0f, 1f);
		entityProperties.ColorChannel1 = EditorGUILayout.Slider("Channel 1", entityProperties.ColorChannel1, 0f, 1f);
		entityProperties.ColorChannel2 = EditorGUILayout.Slider("Channel 2", entityProperties.ColorChannel2, 0f, 1f);

		EditorGUILayout.Space();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("Gravity");
		EditorGUILayout.LabelField(entityProperties.Gravity.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponBulletSize");
		EditorGUILayout.LabelField(entityProperties.WeaponBulletSize.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponBulletVelocity");
		EditorGUILayout.LabelField(entityProperties.WeaponBulletVelocity.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponDamages");
		EditorGUILayout.LabelField(entityProperties.WeaponDamages.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponFireRate");
		EditorGUILayout.LabelField(entityProperties.WeaponFireRate.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponPrecision");
		EditorGUILayout.LabelField(entityProperties.WeaponPrecision.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponRange");
		EditorGUILayout.LabelField(entityProperties.WeaponRange.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("WeaponReloadDuration");
		EditorGUILayout.LabelField(entityProperties.WeaponReloadDuration.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("EntityDashRange");
		EditorGUILayout.LabelField(entityProperties.EntityDashRange.ToString());

		EditorGUILayout.EndHorizontal();

		///

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("EntityVelocity");
		EditorGUILayout.LabelField(entityProperties.EntityVelocity.ToString());

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		///

		EditorGUILayout.LabelField("Bindings 0:");

		foreach(EntityProperties.Property prop in entityProperties.ChannelBindings(0))
		{
			EditorGUILayout.LabelField("    "+prop);
		}

		EditorGUILayout.LabelField("Bindings 1:");
		
		foreach(EntityProperties.Property prop in entityProperties.ChannelBindings(1))
		{
			EditorGUILayout.LabelField("    "+prop);
		}

		EditorGUILayout.LabelField("Bindings 2:");
		
		foreach(EntityProperties.Property prop in entityProperties.ChannelBindings(2))
		{
			EditorGUILayout.LabelField("    "+prop);
		}

		EditorGUILayout.Space();

		if(GUILayout.Button("Update Bindings"))
		{
			entityProperties.SetChannelBindings();
		}
	}
}
