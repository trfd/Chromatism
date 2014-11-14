Shader "Custom/Aberration" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Noise("Base",2D) = "white" {}
		_Coef ("Coef",float) = 0.0
		_RedCoef   ("Red Base Coef",float) = 1.0
		_GreenCoef ("Green Base Coef",float) = 1.0
		_BlueCoef  ("Blue Base Coef",float) = 1.0
	}
	SubShader {
	
		Tags { "RenderType"="Opaque" }
		Pass {
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		
		sampler2D _Noise;
	
		float _Coef;
		float _RedCoef;
		float _GreenCoef;
		float _BlueCoef;
	
		struct v2f 
		{
    		float4 pos : SV_POSITION;
    		float2 uv_MainTex : TEXCOORD0;
		};

		v2f vert (appdata_base v) 
		{
    		v2f o;
    		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    		o.uv_MainTex.xy = v.texcoord.xy;
    		return o;
		}

		half4 frag(v2f IN) : COLOR 
		{	
			float coef = 2.0 * _Coef;
		
			half4 offset = tex2D(_Noise, float2(IN.uv_MainTex.y,0));
			half4 offset2 = tex2D(_Noise, float2(IN.uv_MainTex.y,0.5));
			
			half4 red   = tex2D(_MainTex, IN.uv_MainTex + (_RedCoef   + coef) * float2(offset.x,offset2.x));
			half4 green = tex2D(_MainTex, IN.uv_MainTex + (_GreenCoef + coef) * float2(offset.y,offset2.y));
			half4 blue  = tex2D(_MainTex, IN.uv_MainTex + (_BlueCoef  + coef) * float2(offset.z,offset2.z));
			
			return float4(red.x,green.y,blue.z,1.0);
		}
		
		ENDCG
		}	
	
	} 
}
