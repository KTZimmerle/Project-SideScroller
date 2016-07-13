Shader "ProjectSideScroller/TransparentShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Alpha ("Alpha", Range(0.0, 1.0)) = 0.5
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct VertexInfo
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct FragmentInfo
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : COLOR0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Alpha;
			
			// vertex shader
			FragmentInfo vert (VertexInfo v)
			{
				FragmentInfo o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = _Color;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			// fragment shader
			fixed4 frag (FragmentInfo i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col = i.color;
				col.a = _Alpha;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
