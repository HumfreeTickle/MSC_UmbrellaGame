Shader "Toon/Basic Outline" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1) //Standard colour of the texture
		//to be honest the outline is the main problem with this shader hence why I'm changing it
		_OutlineColor ("Outline Color", Color) = (0,0,0,1) //What the outline colour should be, can't remember if this can be changed or not
		_Outline ("Outline width", Range (.002, 0.03)) = .005 //width of the outline
		//Quickly looking at the two shows that the outline i svery different
		//The youtube cell shader uses only an outline width
		
		
		_MainTex ("Base (RGB)", 2D) = "white" { } 
		
		
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { } //this needs to go into the new shader
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		UNITY_FOG_COORDS(0)
		fixed4 color : COLOR;
	};
	
	uniform float _Outline;
	uniform float4 _OutlineColor;
	
	v2f vert(appdata v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}
	ENDCG

	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/Basic/BASE"
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_APPLY_FOG(i.fogCoord, i.color);
				return i.color;
			}
			ENDCG
		}
	}
	
	Fallback "Toon/Basic"
}
