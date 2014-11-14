using UnityEngine;
using System.Collections;

public class ManagersHandler : MonoBehaviour
{
	public GameObject _fabricPrefab;

	// Use this for initialization
	void Start ()
	{
		var go = GameObject.Find("Fabric - Audio");
		if(_fabricPrefab == null || go == null)
		{
			if(_fabricPrefab == null)
			{
				Debug.LogError("You have to set the fabric prefab !");
			}
			return;
		}

		var fabric = GameObject.Instantiate(_fabricPrefab);
		DontDestroyOnLoad(fabric);
		Destroy(this);
	}
}
