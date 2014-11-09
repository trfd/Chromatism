using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Utility/Pool")]
public class Pool : MonoBehaviour
{
	#region members

	public PoolableObject _prefabPooled;
	public int _numberOfInstance;

	private List<PoolableObject> m_objects = new List<PoolableObject>();
//	private int m_index = 0;

	#endregion

	#region MonoBehaviour

	void Start()
	{
		AllocateObjects(_numberOfInstance);
	}
	
	#endregion

	#region PoolFunction

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
			tempObject.IsPoolable = true;
			m_objects.Add(tempObject);
		}
	}

	public PoolableObject GetUnusedObject()
	{
		PoolableObject tempPO = m_objects.Where(po => po.IsPoolable == true).FirstOrDefault();

		if(tempPO)
		{
			tempPO.IsPoolable = false;
			return tempPO;
		}else{
			AllocateObjects(_numberOfInstance);
			tempPO = m_objects.Where(po => po.IsPoolable == true).FirstOrDefault();
			tempPO.IsPoolable = false;
			return tempPO;
		}
		
	}

	#endregion
	
}
