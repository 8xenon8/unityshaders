Shader "Unlit/TestShader"
{
	Properties
	{
		_MainText("Texture", 2D) = "white" {}
		_OutlineColor1("Normal color 1", Color) = (0, 0, 0, 1)
		_OutlineColor2("Normal color 2", Color) = (0, 0, 0, 1)
		_OutlineColor3("Normal color 3", Color) = (0, 0, 0, 1)
		_GridSize("Grid size", float) = 1
	}

	SubShader
	{
		Blend Off

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "AutoLight.cginc"

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
	
					return float4(resultRed, resultGreen, resultBlue, 1);
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.pos);
					//o.n = mul(unity_ObjectToWorld, v.n);
					//o.n = UnityObjectToClipPos(v.n);
					o.n = v.n;
					//o.n = normalize(o.n);
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
					n.n = UnityObjectToClipPos(n.n);
					float2 sp = ComputeScreenPos(n.pos);
					float3 v = normalize(WorldSpaceViewDir(n.pos));

					//float w = 0;
					//float r;
					//float b;

					//float grad = cross(UNITY_MATRIX_IT_MV[2].xyz, n.n);

					//float w = dot(mul(_WorldSpaceLightPos0, unity_ObjectToWorld), normal);
					float w = dot(WorldSpaceViewDir(n.pos), n.n) / 100;

					//n.n.x = normalize(n.n.x);
					//n.n.y = normalize(n.n.y);
					/*
					if (n.n.x < sqrt(2) || n.n.x > sqrt(2) * -1)
					{
						r = n.n.x;
					}
					if (n.n.y < sqrt(2) || n.n.y > sqrt(2) * -1)
					{
						b = n.n.y;
					}
					*/
					n.n = normalize(n.n);
					//return float4((2 + n.n.x) / 4, (2 + n.n.y) / 4, (2 + n.n.z) / 4, 1);
					float4 resultColor = blendColors((n.n.x + 1), (n.n.y + 1), (n.n.z + 1));
					//float4 resultColor = blendColors((n.n.x + 1) / 2, (n.n.y + 1) / 2, (n.n.z + 1) / 2);
					/*
					resultColor.x = r;
					resultColor.y = w;
					resultColor.z = b;
					*/
					return resultColor;
					/*
					float4 c = float4(0, 0, 0, 1);
					c.x = 1 - (n.n.x + 1) / 3;
					c.y = 1 - (n.n.y + 1) / 3;
					c.z = 1 - (n.n.z + 1) / 3;
					return c;
					*/
				}

			ENDCG
		}
	}
}
