using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PatternManager : MonoBehaviour 
{

	public List<LevelPattern> _patterns = new List<LevelPattern>();

	private int m_selectedPattern = 0;

	void Start()
	{
		Debug.Log(_patterns[0].Name);
		Debug.Log("Pawns " + _patterns[0].Pawns.Length);
		Debug.Log("Walls " + _patterns[0].Walls.Length);
		Debug.Log("Doors " + _patterns[0].Doors.Length);
		DeployLevel();
	}


	public void DeployLevel()
	{

		foreach(GameObject go in _patterns[m_selectedPattern].Pawns)
		{
			GameObject.Instantiate(go);
		}

		foreach(GameObject go in _patterns[m_selectedPattern].Walls)
		{
			GameObject.Instantiate(go);
		}

		foreach(GameObject go in _patterns[m_selectedPattern].Doors)
		{
			GameObject.Instantiate(go);
		}
	}
}
