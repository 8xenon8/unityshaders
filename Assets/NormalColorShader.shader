Shader "Unlit/NormalColorShader"
{
	Properties
	{
		_MainText("Texture", 2D) = "white" {}
		_OutlineColor1("Normal color 1", Color) = (0, 0, 0, 1)
		_OutlineColor2("Normal color 2", Color) = (0, 0, 0, 1)
		_OutlineColor3("Normal color 3", Color) = (0, 0, 0, 1)
		_GridSize("Grid size", float) = 1
		_Pointer("Pointer", Vector) = (0, 0, 0, 0)
	}

	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend Off

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

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

				float4 _OutlineColor1;
				float4 _OutlineColor2;
				float4 _OutlineColor3;
				float _GridSize;

				float4 blendColors(float share1, float share2, float share3)
				{
	
					float share1Percent = max(share1 / (share1 + share2 + share3), 0.01);
					float share2Percent = max(share2 / (share1 + share2 + share3), 0.01);
					float share3Percent = max(share3 / (share1 + share2 + share3), 0.01);
	
					float resultRed = _OutlineColor1.x * share1Percent + _OutlineColor2.x * share2Percent + _OutlineColor3.x * share3Percent;
					float resultGreen = _OutlineColor1.y * share1Percent + _OutlineColor2.y * share2Percent + _OutlineColor3.y * share3Percent;
					float resultBlue = _OutlineColor1.z * share1Percent + _OutlineColor2.z * share2Percent + _OutlineColor3.z * share3Percent;
	
					return float4(resultRed, resultGreen, resultBlue + 1, 1);
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.pos);
					o.n = v.n;
					o.uv = v.uv;
					return o;
				}

				float4 frag(v2f n) : COLOR
				{
					float del = 1 / _GridSize;
					float thickness = 0.005 / (_GridSize / 2);
					if ((n.uv.x % del) < thickness || (n.uv.x % del) > 1 - thickness) return float4(1,1,1,1); 
					if ((n.uv.y % del) < thickness || (n.uv.y % del) > 1 - thickness) return float4(1,1,1,1);
					if ((abs((n.uv.x - thickness) - (1 - n.uv.y)) % del) <= (thickness)) return float4(1,1,1,1);

					float3 normal = n.n;
					//n.n = UnityObjectToClipPos(n.n);
					float2 sp = ComputeScreenPos(UnityObjectToClipPos(n.pos));
					float3 v = normalize(WorldSpaceViewDir(n.pos));

					//n.n = normalize(n.n);
					float4 resultColor = blendColors((n.n.x + 1), (n.n.y + 1), (n.n.z + 1));
					//resultColor.x += sp.x / 2000;
					//resultColor.z += sp.y / 2000;
					return resultColor;
				}

			ENDCG
		}
	}
}
