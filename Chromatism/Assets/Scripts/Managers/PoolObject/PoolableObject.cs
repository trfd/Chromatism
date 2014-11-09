using UnityEngine;
using System.Collections;

public class PoolableObject : MonoBehaviour {

	private bool m_isUsed = false;

	public bool IsUsed
	{
		get{ return m_isUsed; }
		set{ m_isUsed = value; }
	}
}
