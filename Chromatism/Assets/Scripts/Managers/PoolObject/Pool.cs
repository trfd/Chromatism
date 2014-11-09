using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Utility/Pool")]
public class Pool : MonoBehaviour
{
	public PoolableObject _prefabPooled;
	public int _numberOfInstance;

	private List<PoolableObject> m_objects = new List<PoolableObject>();
	private int m_index = 0;

	void Start()
	{
		AllocateObjects(_numberOfInstance);
	}

	void AllocateObjects(int number)
	{
		if(number == 0 || _prefabPooled == null)
		{
			Debug.LogError("number is equal to 0 or _prefabPooled is null. None of this is acceptable !");
			return;
		}

		for(int i = 0; i < _numberOfInstance; i++)
		{
			PoolableObject tempObject = GameObject.Instantiate(_prefabPooled) as PoolableObject;
			m_objects.Add(tempObject);
			tempObject.IsUsed = false;
		}
	}

	PoolableObject GetUnusedObject()
	{
		PoolableObject tempPO = m_objects.Where(po => po.IsUsed == false).FirstOrDefault();

		if(tempPO)
		{
			return tempPO;
		}else{
			AllocateObjects(_numberOfInstance);
			return m_objects.Where(po => po.IsUsed == true).FirstOrDefault();
		}

//		if(m_objects.Count > m_index)
//		{
//			if(m_objects[m_index] != null)
//			{
//				m_index ++;
//				return m_objects[m_index - 1];
//			}else{
//				Debug.LogError("Error null object catch.");
//				return;
//			}
//		}else{
//			AllocateObjects(_numberOfInstance);
//			m_index ++;
//			return m_objects[m_index - 1];
//		}
	}
	
}
