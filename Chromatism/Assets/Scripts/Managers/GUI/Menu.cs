﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{

	public GameObject _menu;
	public GameObject _credits;
	public GameObject _selector1;
	public GameObject _selector2;
	
	public string _gameSceneName;

	void Start()
	{
		KeyBinder.Instance.DefineActions("SwitchUpMenu", new KeyActionConfig(KeyType.Menu, 0, SwitchMenu, null));
		KeyBinder.Instance.DefineActions("SwitchDownMenu", new KeyActionConfig(KeyType.Menu, 0, SwitchMenu, null));
		KeyBinder.Instance.DefineActions("EnterMenu", new KeyActionConfig(KeyType.Menu, 0, EnterMenu, null));
		KeyBinder.Instance.DefineActions("Quit", new KeyActionConfig(KeyType.Menu, 0, () =>{ Application.Quit(); }, null));

		Fabric.EventManager.Instance.PostEvent("music_menu_on");
	}
	
	void Update()
	{
	
	}

	void SwitchMenu()
	{
		_selector1.SetActive(!_selector1.activeSelf);
		_selector2.SetActive(!_selector2.activeSelf); 
	}

	void SwitchDisplay()
	{
		_menu.SetActive(!_menu.activeSelf);
		_credits.SetActive(!_credits.activeSelf); 
	}

	void EnterMenu()
	{
		if(_menu.activeSelf && _selector1.activeSelf)
		{
			Fabric.EventManager.Instance.PostEvent("music_menu_off");
			Fabric.EventManager.Instance.PostEvent("music_level_on");
			Application.LoadLevel(_gameSceneName);
		}else{
			SwitchDisplay();
		}
	}
}
