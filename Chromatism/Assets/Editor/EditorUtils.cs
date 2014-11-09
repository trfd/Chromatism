﻿//
// EditorUtils.cs
//
// Author:
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

#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Reflection;

public class EditorUtils
{
	public const float defaultTextHeight = 16f;

	/// <summary>
	/// Draw in the inspector a button for every method
	/// in the target object that has an attribute GUIButton.
	/// This method should be called into OnInspectorGUI()
	/// </summary>
	/// <param name="target">Inspector's target.</param>
	public static void DrawMethodGUIButton(UnityEngine.Object target)
	{
		Type targetType = target.GetType();
		
		MethodInfo[] mInfos = targetType.GetMethods(BindingFlags.Instance  | 
		                                            BindingFlags.NonPublic | 
		                                            BindingFlags.Public);

		// Go through all methods 
		// to find attribute GUIButton
		for(int i=0 ; i<mInfos.Length ; i++)
		{
			if(Attribute.IsDefined(mInfos[i] , typeof(InspectorButton)))
			{
				InspectorButton button = 
					(InspectorButton) Attribute.GetCustomAttribute(mInfos[i], typeof(InspectorButton));
				
				if(GUILayout.Button(button.name))
				{
					mInfos[i].Invoke(target,button.args);
				}
			}
		}
	}

	public static void DrawMemberValue(UnityEngine.Object target)
	{
		DrawFieldValue(target);
		DrawPropertyValue(target);
	}

	private static void DrawFieldValue(UnityEngine.Object target)
	{
		Type targetType = target.GetType();
		
		FieldInfo[] fieldInfos = targetType.GetFields(BindingFlags.Instance  | 
		                                              BindingFlags.NonPublic | 
		                                              BindingFlags.Public);
		
		// Go through all methods 
		// to find attribute GUIButton
		for(int i=0 ; i<fieldInfos.Length ; i++)
		{
			if(Attribute.IsDefined(fieldInfos[i] , typeof(InspectorLabel)))
			{
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.LabelField(fieldInfos[i].Name);
				EditorGUILayout.LabelField(fieldInfos[i].GetValue(target).ToString());

				EditorGUILayout.EndHorizontal();
			}
		}
	}

	private static void DrawPropertyValue(UnityEngine.Object target)
	{
		Type targetType = target.GetType();
		
		PropertyInfo[] propInfos = targetType.GetProperties(BindingFlags.Instance  | 
		                                                    BindingFlags.NonPublic | 
		                                                    BindingFlags.Public);
		
		// Go through all methods 
		// to find attribute GUIButton
		for(int i=0 ; i<propInfos.Length ; i++)
		{
			if(Attribute.IsDefined(propInfos[i] , typeof(InspectorLabel)))
			{
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.LabelField(propInfos[i].Name);
				EditorGUILayout.LabelField(propInfos[i].GetValue(target,null).ToString());
				
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}


#endif