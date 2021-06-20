Shader "Unlit/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct v2f
				{
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert(float4 pos : POSITION, float2 uv : TEXCOORD0)
				{
					v2f v;
					v.pos = pos * 1.5;
					v.uv = uv;
					return v;
				}

				float4 frag(v2f v) : Color
				{
					return float4(1,1,1,1);
				}
			ENDCG
        }
    }
}
