﻿Shader "Custom/Mirror"
{
	Properties
	{
		_Inverse("Inverse", int) = 0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};

			sampler2D _MainTex;
			int _Inverse;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.screenPos.xy / i.screenPos.w;
				if (_Inverse == 1)
				{
					uv.x = 1 - uv.x;
				}
				fixed4 portalCol = tex2D(_MainTex, uv);
				return portalCol;
			}
			ENDCG
		}
	}
	Fallback "Standard" // for shadows
}