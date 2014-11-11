using UnityEngine;
using System.Collections;
using UnityEditor;

public class PatternCreator : MonoBehaviour {


	public string _name;
	
	public GameObject[] _pawns;
	public GameObject[] _walls;
	public GameObject[] _doors;

	private LevelPattern m_levelPattern;

	[InspectorButton("Bake")]
	void Bake()
	{
		if(_name == null || _name == "")
			return;

		m_levelPattern = new LevelPattern();
		m_levelPattern.Name = _name;
		m_levelPattern.Pawns = _pawns;
		m_levelPattern.Walls = _walls;
		m_levelPattern.Doors = _doors;

//		EditorUtility.SetDirty(m_levelPattern);
		m_levelPattern.Bake();
	}
}
