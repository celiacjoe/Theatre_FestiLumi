Shader "Unlit/bluur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_r1 ("_r1",Float) = 0
		_r2 ("_r2",Float) = 0
		_r3 ("_r3",Float) = 0
		_r4 ("_r4",Float) = 0
		_r5 ("_r5",Float) = 0
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _r1;
			float _r2;
			float _r3;
			float _r4;
			float _r5;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
			}
			float Gaussian (float sigma, float x)
			{
			return exp(-(x*x) / (2.0 * sigma*sigma));
			}
			float blur (in float2 uv)
			{
				float c = _r3;
				float c2 = 0.01*_r4;
				float total = 0.0;
				float ret = 0.;
				int nbr = 8;
				for (int iy = 0; iy < nbr; ++iy)
				{
					float fy = Gaussian (c, float(iy-nbr/2));
					float offsety = float(iy-nbr/2) * c2;
					for (int ix = 0; ix < nbr; ++ix)
					{
						float fx = Gaussian (c, float(ix-nbr/2));
						float offsetx = float(ix-nbr/2) *c2;
						total += fx * fy;            
						ret += tex2D(_MainTex, uv + float2(offsetx, offsety)).r * fx*fy;
					}
				}
				return ret / total;
			}

            fixed4 frag (v2f i) : SV_Target
            {

				float bl =smoothstep(_r1,_r2, blur(i.uv));

                fixed4 col = float4(bl,bl,bl,1.);
                return col;
            }
            ENDCG
        }
    }
}
