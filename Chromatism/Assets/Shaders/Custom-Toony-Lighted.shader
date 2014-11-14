Shader "Custom/Toon/Lighted" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma target 3.0 
#pragma surface surf ToonRamp

sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
	c.a = 0;
	return c;
}


sampler2D _MainTex;
sampler2D _Noise;
float4 _Color;
float _AberrationCoef;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
};

void surf (Input IN, inout SurfaceOutput o) 
{	
	float coef = 2.0 * _AberrationCoef;
	
	float4 offset  = tex2D(_Noise , float2(IN.uv_MainTex));
	float4 offset2 = tex2D(_Noise , float2(IN.uv_MainTex.y,IN.uv_MainTex.x));
	
	float4 red   = tex2D(_MainTex, IN.uv_MainTex + coef * float2(offset.x,offset2.x));
	float4 green = tex2D(_MainTex, IN.uv_MainTex + coef * float2(offset.y,offset2.y));
	float4 blue  = tex2D(_MainTex, IN.uv_MainTex + coef * float2(offset.z,offset2.z));
	
	float4 col = float4(red.x,green.y,blue.z,1.0);
	
	o.Albedo = _Color * col.rgb * (1.0 + coef) ;
	o.Alpha = col.a;
}
ENDCG

	} 

	//Fallback "Diffuse"
}
