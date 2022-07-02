Shader "Unlit/regKi"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_texsec ("_texsec", 2D) = "white" {}
		_f1 ("_f1", Float) = 0
		_f2 ("_f2", Float) = 0
		_f3 ("_f3", Float) = 0
		_f4 ("_f4", Float) = 0
		_f5 ("_f5", Float) = 0
		_f6 ("_f6", Float) = 0
		//_f7 ("_f7", Float) = 0
		//_f8 ("_f8", Float) = 0
		//_f9 ("_f9", Float) = 0
		//_f10 ("_f10", Float) = 0
		_p1 ("_p1", Float) = 0
		_p2 ("_p2", Float) = 0
		_p3 ("_p3", Float) = 0
		_p4 ("_p4", Float) = 0
		_p5 ("_p5", Float) = 0
		lh ("_lh", Float) = 0
		lb ("_lb", Float) = 0
		lg ("_lg", Float) = 0
		ld ("_ld", Float) = 0
		_r1 ("_r1", Float) = 0
		_r2 ("_r2", Float) = 0
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
			sampler2D _texsec;
            float4 _texsec_ST;
			float _f1 ;
			float _f2;
			float _f3;
			float _f4;
			float _f5;
			float _f6;
			float _f7;
			float _f8;
			float _f9;
			float _f10;
			float _p1 ;
			float _p2;
			float _p3 ;
			float _p4;
			float _p5;
			float lh;
			float lb;
			float lg;
			float ld;
			float _r1;
			float _r2;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float2 map2(float2 value, float2 min1, float2 max1, float2 min2, float2 max2) {
			return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
			}
			float Gaussian (float sigma, float x)
			{
			return exp(-(x*x) / (2.0 * sigma*sigma));
			}
			float blur (in float2 uv)
			{
				float c = _f3;
				float c2 = 0.01*_f4;
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
						ret += tex2D(_texsec, uv + float2(offsetx, offsety)).r * fx*fy;
					}
				}
				return ret / total;
			}
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv2 = map2(i.uv, float2(0.,0.),float2(1.,1.),float2(lg,lh),float2(ld,lb));
                float c1 = smoothstep(_r1,_r2,tex2D(_texsec, uv2).x);
				//float c3 = smoothstep(_f5,_f6,smoothstep(_f1,_f2,blur(1. -uv2)));
				float pt1 = smoothstep(0.02,0.01,distance (i.uv,float2(_p1,_p2)));
				float pt2 = smoothstep(0.02,0.01,distance (i.uv,float2(_p3,_p4)));
                //return float4(lerp(lerp(float3(c1,c1,c1),float3(1.,0.,0.),pt),float3(0.,0.,1.),step(i.uv.y,1.-lh)+step(1.-lb,i.uv.y)+step(i.uv.x,1.-lg)+step(1.-ld,i.uv.x)),1.);
				float3 fin = lerp(lerp(c1,float3(1.,0.,0.),pt1),lerp(float3(0.,0.,1.),float3(0.,1.,0.),step(0.05,_p5)),pt2);
				return float4(fin,1.);
            }
            ENDCG
        }
    }
}
