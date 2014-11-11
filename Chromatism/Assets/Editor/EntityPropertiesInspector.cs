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

		if((entityProperties._defaultValueFoldout = 
		    EditorGUILayout.Foldout(entityProperties._defaultValueFoldout,"Default Values")))
		{

			if(GUILayout.Button("Reset Values"))
				entityProperties.ResetDefaultValues();
		
			entityProperties.SetDefaultValue(EntityProperties.Property.GRAVITY,
			                                  EditorGUILayout.FloatField("Gravity", 
										entityProperties.DefaultValue(EntityProperties.Property.GRAVITY)));
			
			entityProperties.SetDefaultValue(EntityProperties.Property.ENTITY_DASH_RANGE,
			                                  EditorGUILayout.FloatField("EntityDashRange", 
										entityProperties.DefaultValue(EntityProperties.Property.ENTITY_DASH_RANGE)));

			entityProperties.SetDefaultValue(EntityProperties.Property.ENTITY_VELOCITY,
			                                  EditorGUILayout.FloatField("EntityVelocity", 
			                           	entityProperties.DefaultValue(EntityProperties.Property.ENTITY_VELOCITY)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_BULLET_SIZE,
			                                  EditorGUILayout.FloatField("EntityBulletSize", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_BULLET_SIZE)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_BULLET_VELOCITY,
			                                  EditorGUILayout.FloatField("WeaponBulletVelocity", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_BULLET_VELOCITY)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_BULLET_DAMAGES,
			                                  EditorGUILayout.FloatField("WeaponDamages", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_BULLET_DAMAGES)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_BULLET_RANGE,
			                                  EditorGUILayout.FloatField("WeaponRange", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_BULLET_RANGE)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_FIRERATE,
			                                  EditorGUILayout.FloatField("WeaponFireRate", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_FIRERATE)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_MAGAZINE_SIZE,
			                                 EditorGUILayout.FloatField("WeaponMagazineSize", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_MAGAZINE_SIZE)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_PRECISION,
			                                  EditorGUILayout.FloatField("WeaponPrecision", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_PRECISION)));

			entityProperties.SetDefaultValue(EntityProperties.Property.WEAPON_RELOAD_DURATION,
			                                  EditorGUILayout.FloatField("WeaponReloadDuration", 
			                           entityProperties.DefaultValue(EntityProperties.Property.WEAPON_RELOAD_DURATION)));

		}


		EditorGUILayout.Space();

		///

		if((entityProperties._currentValueFoldout = 
		    EditorGUILayout.Foldout(entityProperties._currentValueFoldout,"Current Values")))
		{
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Gravity");
			EditorGUILayout.LabelField(entityProperties.Gravity.ToString());
			
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
			
			EditorGUILayout.LabelField("WeaponBulletDamages");
			EditorGUILayout.LabelField(entityProperties.WeaponBulletDamages.ToString());
			
			EditorGUILayout.EndHorizontal();

			///

			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("WeaponBulletRange");
			EditorGUILayout.LabelField(entityProperties.WeaponBulletRange.ToString());
			
			EditorGUILayout.EndHorizontal();

			///
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("WeaponFireRate");
			EditorGUILayout.LabelField(entityProperties.WeaponFireRate.ToString());
			
			EditorGUILayout.EndHorizontal();

			///
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("WeaponMagazineSize");
			EditorGUILayout.LabelField(entityProperties.WeaponMagazineSize.ToString());
			
			EditorGUILayout.EndHorizontal();
			
			///
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("WeaponPrecision");
			EditorGUILayout.LabelField(entityProperties.WeaponPrecision.ToString());
			
			EditorGUILayout.EndHorizontal();
			
			///
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("WeaponReloadDuration");
			EditorGUILayout.LabelField(entityProperties.WeaponReloadDuration.ToString());
			
			EditorGUILayout.EndHorizontal();
		
		}

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

		if (GUI.changed)
			EditorUtility.SetDirty(entityProperties);
	}
}
