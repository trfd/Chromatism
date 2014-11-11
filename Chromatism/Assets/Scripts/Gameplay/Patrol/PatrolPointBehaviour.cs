//
// PatrolPointBehaviour.cs
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

public class PatrolPointBehaviour : MonoBehaviour
{
	/// <summary>
	/// Raises when a start heading toward the patrol point
	/// </summary>
	/// <param name="pawn">Pawn.</param>
	public virtual void OnObjectStartHeading(GameObject gameObject){}

	/// <summary>
	/// Raises each time the pawn is heading toward the patrol point.
	/// </summary>
	/// <param name="pawn">Pawn.</param>
	public virtual void OnObjectHeading(GameObject pawn){}

	/// <summary>
	/// Raises a pawn enters patrol point.
	/// </summary>
	/// <param name="pawn">Pawn.</param>
	public virtual void OnObjectEnterPatrolPoint(GameObject pawn){}

	/// <summary>
	/// Raises each fram a pawn stays on patrol point.
	/// </summary>
	/// <param name="pawn">Pawn.</param>
	public virtual void OnObjectStayOnPatrolPoint(GameObject pawn){}

	/// <summary>
	/// Raises when a pawn exit patrol point.
	/// </summary>
	/// <param name="pawn">Pawn.</param>
	public virtual void OnObjectExitPatrolPoint(GameObject pawn){}
}
