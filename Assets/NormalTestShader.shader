Shader "Unlit/NormalTestShader"
{
	Properties
	{
		_MainText("Texture", 2D) = "white" {}
		_OutlineColor1("Normal color 1", Color) = (0, 0, 0, 1)
		_OutlineColor2("Normal color 2", Color) = (0, 0, 0, 1)
		_OutlineColor3("Normal color 3", Color) = (0, 0, 0, 1)
		_GridSize("GridSize", float) = 1
	}

	SubShader
	{
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
					float r = n.n.x;
					float b = n.n.y;
					return float4(r, 0, b, 1);

					float del = 1 / _GridSize;
					float thickness = 0.005 / (_GridSize / 2);
					if ((n.uv.x % del) < thickness || (n.uv.x % del) > 1 - thickness) return float4(1,1,1,1); 
					if ((n.uv.y % del) < thickness || (n.uv.y % del) > 1 - thickness) return float4(1,1,1,1);
					if ((abs((n.uv.x - thickness) - (1 - n.uv.y)) % del) <= (thickness)) return float4(1,1,1,1);

					if (n.n.x > n.n.y && n.n.x > n.n.z) { 
						return _OutlineColor1; 
					}
					if (n.n.y > n.n.x && n.n.y > n.n.z) {
						return _OutlineColor2;
					} else {
						return _OutlineColor3;
					}

					//float4 c = (_OutlineColor1 * n.n.x + _OutlineColor2 * n.n.y + _OutlineColor3 * n.n.z) / 3;
					//c.x = min(1, c.x);
					//c.y = min(1, c.y);
					//c.z = min(1, c.z);
					//c.w = 1;
					//return c;
					//return float4(n.n.x, n.n.z, n.n.y, 1);
					//return float4(1,1,1,1);
				}

			ENDCG
		}
	}
}
