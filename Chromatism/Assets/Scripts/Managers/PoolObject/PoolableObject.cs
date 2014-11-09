using UnityEngine;
using System.Collections;

public abstract class PoolableObject : MonoBehaviour {

	#region members

	private bool m_isPoolable = true;

	#endregion

	#region Properties

	public bool IsPoolable
	{
		get{ return m_isPoolable; }
		set{ m_isPoolable = value; }
	}

	#endregion
}
