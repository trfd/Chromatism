Shader "Custom/TriangleScatter" {
	Properties 
	{
		_Color ("Color",Color) = (0,0,0,0)
		_Size ("Size", float) = 0.0
		_Randomness("Randomness",float) = 0.0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		Pass 
		{
		
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"

		float4 _Color;
		float _Randomness;
		float _Size;
		
		 float rand(float seed)  {
             return frac(sin( seed * 12.9898 + 78.233 ) * 43758.5453);
         }
		
		 float rand(float3 myVector)  {
             return frac(sin( dot(myVector ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
         }
		
		struct v2f 
		{
    		float4 pos        : SV_POSITION;
    		float2 uv_MainTex : TEXCOORD0;
    		float2 uv1        : TEXCOORD1;
		};

		v2f vert (appdata_full v) 
		{
			float4 randOff = float4(rand(v.vertex.x),rand(v.vertex.y),rand(v.vertex.z),0.0);
    		v2f o;
    		o.pos = mul (UNITY_MATRIX_MVP, v.vertex + _Size * float4(v.normal,0)+ _Randomness * randOff);
    	

    		o.uv_MainTex.xy = v.texcoord.xy;
    		
    		return o;
		}

		half4 frag(v2f IN) : COLOR 
		{
			return _Color;
		}
		
		ENDCG
	}	
	
	} 
	FallBack "Diffuse"
}
