using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelPattern : ScriptableObject 
{
	[SerializeField]
	public string Name	{	get; set;	}

	[SerializeField]
	public GameObject[] Pawns	{	get; set;	}
	[SerializeField]
	public GameObject[] Walls	{	get; set;	}
	[SerializeField]
	public GameObject[] Doors	{	get; set;	}


	public void Bake()
	{
		Debug.Log("LevelName = " + Name);
		AssetDatabase.CreateAsset(this, "Assets/Patterns/"+Name+".asset");
		AssetDatabase.SaveAssets();
	}

}
