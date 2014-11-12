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

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
	[DrawGizmo(GizmoType.NotSelected | GizmoType.Selected)]
	public static void DrawRoom(RoomCreator creator, GizmoType gizmo)
	{
		//RoomCreator creator = (RoomCreator) target;

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

		DrawEditionLevel(creator);

		Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);


	}

	void CreateWindow(int id)
	{
		GUILayout.Button("Hello Button");
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
}
