Shader "Unlit/FinishZoneShader"
{
	Properties
	{
		_Color("Color", Color) = (0, 0, 0, 1)
	}

		SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend Off
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				float4 _Color;

				struct appdata
				{
					float4 pos : POSITION;
					float3 n : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 pos : POSITION;
					float3 n : NORMAL;
					float2 uv : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.pos);
					o.n = v.n;
					o.uv = v.uv;
					return o;
				}

				float4 frag(v2f v) : COLOR
				{
					float x = (v.uv.x > 0.97 || v.uv.x < 0.03) || (v.uv.y > 0.97 || v.uv.y < 0.03) ? 0.8 : 0.3;
					return _Color * x;
				}

			ENDCG
		}
	}
}
