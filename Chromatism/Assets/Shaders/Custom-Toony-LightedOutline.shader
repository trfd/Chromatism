Shader "Custom/Toon/Lighted Outline" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_Noise ("Perlin Noise", 2D) = "white" {}
		_AberrationCoef("AberrationCoef",float) = 0.0
	}

	SubShader {
		Tags {  "Queue"="Transparent" "RenderType"="Transparent" }
		UsePass "Custom/Toon/Lighted/FORWARD"
		UsePass "Toon/Basic Outline/OUTLINE"
	} 
	
	//Fallback "Toon/Lighted"
}
