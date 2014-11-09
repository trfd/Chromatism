using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class KeyBindRefs
{

    public static Dictionary<KeyConfig, List<string>> DefaultBindsRefs
    {
        get
        {
            var temp = new Dictionary<KeyConfig, List<string>>();
			temp.Add(new KeyConfig(KeyCode.Z), new List<string>{"Forward"});
			temp.Add(new KeyConfig(KeyCode.S), new List<string>{"Backward"});
			temp.Add(new KeyConfig(KeyCode.Q), new List<string>{"StrafeLeft"});
			temp.Add(new KeyConfig(KeyCode.D), new List<string>{"StrafeRight"});
			temp.Add(new KeyConfig(KeyCode.Space), new List<string>{"Jump/Dash"});
			temp.Add(new KeyConfig("MouseX"), new List<string>{"MouseX"});
			temp.Add(new KeyConfig("MouseY"), new List<string>{"MouseY"});
			temp.Add(new KeyConfig(KeyCode.Tab), new List<string>{"KeyBindGUI"});

            return temp;
        }
    }

    public static Dictionary<string, ActionConfig> DefaultBinds
    {
        get
        {
            var temp = new Dictionary<string, ActionConfig>();
			temp.Add("Forward", new KeyActionConfig(KeyType.Movement, 0, null, null));
			temp.Add("Backward", new KeyActionConfig(KeyType.Movement, 1, null, null));
			temp.Add("StrafeLeft", new KeyActionConfig(KeyType.Movement, 2, null, null));
			temp.Add("StrafeRight", new KeyActionConfig(KeyType.Movement, 3, null, null));
			temp.Add("Jump/Dash", new KeyActionConfig(KeyType.Movement, 4, null, null));
			temp.Add("MouseX", new AxisActionConfig(KeyType.Head, 0, null));
			temp.Add("MouseY", new AxisActionConfig(KeyType.Head, 0, null));
			temp.Add("KeyBindGUI", new KeyActionConfig(KeyType.Menu, 0, null, null));

            return temp;
        }
    }

    public static List<string> Axes
    {
        get
        {
            return new List<string>
            {   
				"MouseX",
				"MouseY",
                "Horizontal",
                "Vertical",
                "Horizontal_1",
                "Vertical_1",
                "Horizontal_2",
                "Vertical_2",
                "TriggerLeft",
                "TriggerRight"
            };
        }
    }

	public static string ChangingKey = "";
	public static KeyConfig LastInput;

}
