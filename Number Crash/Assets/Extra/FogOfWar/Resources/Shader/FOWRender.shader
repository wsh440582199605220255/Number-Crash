// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HOG/FOWRender"
{
	Properties
	{
		_MainTex("Fog Texture", 2D) = "white" {}
		_Unexplored("Unexplored Color", Color) = (0.05, 0.05, 0.05, 0.05)
		_Explored("Explored Color", Color) = (0.35, 0.35, 0.35, 0.35)
		_BlendFactor("Blend Factor", range(0,1)) = 0
	}
	
	SubShader
	{
		Pass{
		Tags{ "Queue" = "Transparent+151"  "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		//ZTest Off
		//Cull Back

		CGPROGRAM
#include "UnityCG.cginc" 
#pragma vertex vert
#pragma fragment frag	
#pragma fragmentoption ARB_precision_hint_fastest

		sampler2D _MainTex;
		uniform half4 _Unexplored;
		uniform half4 _Explored;
		uniform half _BlendFactor;
		float4x4 unity_Projector;

		struct v2f
		{
			half4 pos : SV_POSITION;
			half4 uv : TEXCOORD0;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			//o.uv = v.texcoord;
		
			o.uv = mul(unity_Projector,v.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			//进行投影采样  
			//half4 data = tex2Dproj(_MainTex,i.uv);
			//限制投影方向  
			//data = data*step(0,i.uv.w);

			//return data;
			
			half4 data = tex2D(_MainTex, i.uv);
			half2 fog = lerp(data.rg, data.ba, _BlendFactor);
			half4 color = lerp(_Unexplored, _Explored, fog.g);
			color.a = (1 - fog.r) * color.a;

			return color;
		}
		ENDCG
		}	
	}
	FallBack Off
}
