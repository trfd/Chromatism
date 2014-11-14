using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject _menu;
	public GameObject _credits;
	public GameObject _selector1;
	public GameObject _selector2;

	void Start()
	{
		Debug.Log("plop");
//		KeyBinder.Instance.DefineActions("SwitchUpMenu", new KeyActionConfig(KeyType.Menu, 0, SwitchMenu, null));
//		KeyBinder.Instance.DefineActions("SwitchDownMenu", new KeyActionConfig(KeyType.Menu, 0, SwitchMenu, null));
//		KeyBinder.Instance.DefineActions("EnterMenu", new KeyActionConfig(KeyType.Menu, 0, EnterMenu, null));
//		KeyBinder.Instance.DefineActions("Quit", new KeyActionConfig(KeyType.Menu, 0, () =>{ Application.Quit(); }, null));

		Fabric.EventManager.Instance.PostEvent("music_menu_on");
	}
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
		{
			SwitchMenu();
		}

		
		if(Input.GetKeyUp(KeyCode.Return))
		{
			EnterMenu();
		}

		
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void SwitchMenu()
	{
		Debug.Log("switch");
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
			Application.LoadLevel("Game");
		}else{
			SwitchDisplay();
		}
	}
}
