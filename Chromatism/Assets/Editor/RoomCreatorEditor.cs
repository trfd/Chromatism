//
// RoomCreatorEditor.cs
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

public struct Cell2i
{
	public int x;
	public int y;
}

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
	private int m_selectedPrefabIndex;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();

		RoomCreator creator = (RoomCreator) target;

		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Height");
		
		if(GUILayout.Button("Up"))
			creator.EditingHeight = Mathf.Min(creator.EditingHeight+1,creator._height-1);

		if(GUILayout.Button("Down"))
			creator.EditingHeight = Mathf.Max(creator.EditingHeight-1,0);
		
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		m_selectedPrefabIndex = EditorGUILayout.Popup("Prefab",m_selectedPrefabIndex,creator.Prefabs);

		if(GUILayout.Button("Refresh"))
			creator.CreatePrefabList();

		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button("Create Prefab"))
			creator.StartCreatePrefabAtIndex(m_selectedPrefabIndex);
	}

	void OnSceneGUI()
	{
		RoomCreator creator = (RoomCreator) target;

		/// Compute mouse position

		Vector3 mouse = MouseWorldPosition(creator);
		
		Cell2i cell;
		
		cell.x = Mathf.FloorToInt(mouse.x/RoomCreator.CellSize);
		cell.y = Mathf.FloorToInt(mouse.z/RoomCreator.CellSize);

		Vector3 floorMouse = RoomCreator.CellSize * ( new Vector3(cell.x , creator.EditingHeight ,cell.y));

		/// Draw Mouse cell

		Handles.color = Color.cyan;
		
		DrawWireCell(creator,cell);

		/// 
		
		CheckMouseClick(creator);

		CheckEscape(creator);
		
		if(creator.CurrentSelectedPrefab != null)
		{
			creator.CurrentSelectedPrefab.transform.position = floorMouse;
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		}
	}

	[DrawGizmo(GizmoType.NotSelected | GizmoType.Selected)]
	public static void DrawRoom(RoomCreator creator, GizmoType gizmo)
	{
		// Reset position

		creator.transform.position = Vector3.zero;

		DrawRoomCreator(creator);

		SnapCurrentSelection(creator);

		// Move and resize the current edition plane

		DrawEditionLevel(creator);
	}


	#region Drawings

	static void DrawRoomCreator(RoomCreator creator)
	{
		Handles.color = Color.white;
		
		Vector3 origin = creator.transform.position;
		
		DrawWireCube(origin,
		             creator._width * RoomCreator.CellSize,
		             creator._height * RoomCreator.CellSize,
		             creator._depth * RoomCreator.CellSize);
		
		
		Handles.color = new Color(1f,1f,1f,0.2f);
		
		Vector3 editPlane = Vector3.up * creator.EditingHeight * RoomCreator.CellSize;
		
		for(int x = 1 ; x < creator._width ; x++)
		{
			Handles.DrawLine(origin + Vector3.right * x + editPlane , origin + Vector3.right * x + Vector3.forward * creator._depth + editPlane);
		}
		
		for(int z = 1 ; z < creator._depth ; z++)
		{
			Handles.DrawLine(origin + Vector3.forward * z + editPlane , origin + Vector3.forward * z + Vector3.right * creator._width + editPlane);
		}
	}
			
	static void DrawWireCube(Vector3 origin, Size3i size)
	{
		DrawWireCube(origin,size.width,size.height,size.depth);
	}

	static void DrawWireCube(Vector3 origin, float w, float h, float d)
	{
		// X

		Handles.DrawLine(origin, origin + Vector3.right * w);
		Handles.DrawLine(origin + Vector3.up      * h, origin + Vector3.up      * h + Vector3.right   * w);
		Handles.DrawLine(origin + Vector3.forward * d, origin + Vector3.forward * d + Vector3.right   * w);
		Handles.DrawLine(origin + Vector3.up * h + Vector3.forward * d, 
		                 origin + Vector3.up * h + Vector3.forward * d + Vector3.right * w);

		// Y

		Handles.DrawLine(origin, origin + Vector3.up * h);
		Handles.DrawLine(origin + Vector3.right   * w, origin + Vector3.right   * w + Vector3.up * h);
		Handles.DrawLine(origin + Vector3.forward * d, origin + Vector3.forward * d + Vector3.up * h);
		Handles.DrawLine(origin + Vector3.right * w + Vector3.forward * d, 
		                 origin + Vector3.right * w + Vector3.forward * d + Vector3.up * h);

		// Z

		Handles.DrawLine(origin, origin + Vector3.forward * d);
		Handles.DrawLine(origin + Vector3.up    * h, origin + Vector3.up    * h + Vector3.forward * d);
		Handles.DrawLine(origin + Vector3.right * w, origin + Vector3.right * w + Vector3.forward * d);
		Handles.DrawLine(origin + Vector3.up * h + Vector3.right * w, 
		                 origin + Vector3.up * h + Vector3.right * w + Vector3.forward * d);
	}

	static void DrawWireCell(RoomCreator creator , Cell2i cell)
	{
		Vector3 origin = creator.transform.position + 
			cell.x * RoomCreator.CellSize * Vector3.right + 
			cell.y * RoomCreator.CellSize * Vector3.forward;

		origin.y = creator.EditingHeight * RoomCreator.CellSize;

		Handles.DrawLine(origin,origin + RoomCreator.CellSize * Vector3.right);
		Handles.DrawLine(origin,origin + RoomCreator.CellSize * Vector3.forward);
		Handles.DrawLine(origin + RoomCreator.CellSize * Vector3.right,
		                 origin + RoomCreator.CellSize * Vector3.right+
		                 		  RoomCreator.CellSize * Vector3.forward);
		                 
		Handles.DrawLine(origin + RoomCreator.CellSize * Vector3.forward,
		                 origin + RoomCreator.CellSize * Vector3.right+
		                 RoomCreator.CellSize * Vector3.forward);
		                 
	}

	static void DrawEditionLevel(RoomCreator creator)
	{
		Vector3 off = new Vector3(
			0.5f * creator._width  * RoomCreator.CellSize ,
          	-0.2f ,
			0.5f * creator._depth  * RoomCreator.CellSize );

		Vector3 size = new Vector3(
			creator._width  * RoomCreator.CellSize ,
			creator._depth  * RoomCreator.CellSize ,
			0f );

		creator._editingPlane.transform.position = creator.transform.position + off + 
			Vector3.up *creator.EditingHeight * RoomCreator.CellSize;

		creator._editingPlane.transform.localScale = size;
	}

	#endregion

	#region Edition

	static void SnapCurrentSelection(RoomCreator creator)
	{
		GameObject selectedObject = Selection.activeGameObject;
		
		if(selectedObject != null)
		{
			RoomObject roomObject = selectedObject.GetComponentInParent<RoomObject>();
			
			if(roomObject != null)
			{
				Handles.color = Color.green;
				DrawWireCube(roomObject.transform.position,roomObject._size);
				
				// Snapping
				
				Vector3 position = roomObject.transform.position;
				
				position.x = Mathf.Clamp(Mathf.Round(position.x/RoomCreator.CellSize),0,creator._width )*RoomCreator.CellSize;
				position.y = Mathf.Clamp(Mathf.Round(position.y/RoomCreator.CellSize),0,creator._height)*RoomCreator.CellSize;
				position.z = Mathf.Clamp(Mathf.Round(position.z/RoomCreator.CellSize),0,creator._depth )*RoomCreator.CellSize;
				
				roomObject.transform.position = position;
			}
		}
	}

	static Vector3 MouseWorldPosition(RoomCreator creator)
	{
		float planeHeight = creator.EditingHeight * RoomCreator.CellSize;

		Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

		
		Vector3 projOrigin = mouseRay.origin;
		Vector3 projDir    = mouseRay.direction;
		
		projOrigin.y = 0;
		projDir.y    = 0;

		float mag = (planeHeight - mouseRay.origin.y) * Vector3.Dot(projDir,projDir.normalized) /  mouseRay.direction.y;

		return projOrigin + projDir.normalized * mag;
	}
	
	static void CheckMouseClick(RoomCreator creator)
	{
		if(Event.current.isMouse && Event.current.type == EventType.MouseDown)
		{
			if(Event.current.button == 0)
			{
				if(creator.CurrentSelectedPrefab != null)
				{
					creator.DuplicateCurrentPrefab();
					Event.current.Use();
				}
			}
			else if(Event.current.button == 1)
			{
				GameObject obj = HandleUtility.PickGameObject(Event.current.mousePosition,true);

				if(obj == null) return;

				RoomObject roomObj = obj.GetComponentInParent<RoomObject>();

				if(roomObj == null) return;

				DestroyImmediate(roomObj.gameObject);
			}
		}
	}

	static void CheckEscape(RoomCreator creator)
	{
		if(Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
		{
			if(creator.CurrentSelectedPrefab == null)
				return;
			DestroyImmediate(creator.CurrentSelectedPrefab.gameObject);
			creator.CurrentSelectedPrefab = null;
		}
	}

	#endregion
}
