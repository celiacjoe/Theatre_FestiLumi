Shader "Unlit/var1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_texf ("_texf", 2D) = "white" {}
		_t ("_t", Float) = 0
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
			#define COLORS 32.
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
			sampler2D _texf;
			float4 _texf_ST;
			float _t;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			float rd (float t) {return frac(sin(dot(floor(t),12.45))*4597.268);}
			float2 hash( float2 p )
			{
				p = float2( dot(p,float2(127.1,311.7)), dot(p,float2(269.5,183.3)) );
				return -1.0 + 2.0*frac(sin(p)*43758.5453123);
			}

			float noise( in float2 p )
			{
				const float K1 = 0.366025404; 
				const float K2 = 0.211324865; 

				float2  i = floor( p + (p.x+p.y)*K1 );
				float2  a = p - i + (i.x+i.y)*K2;
				float m = step(a.y,a.x); 
				float2  o = float2(m,1.0-m);
				float2  b = a - o + K2;
				float2  c = a - 1.0 + 2.0*K2;
				float3  h = max( 0.5-float3(dot(a,a), dot(b,b), dot(c,c) ), 0.0 );
				float3  n = h*h*h*h*float3( dot(a,hash(i+0.0)), dot(b,hash(i+o)), dot(c,hash(i+1.0)));
				return dot( n, float3(70.0,70.,70.) );
			}

			float ov(float x, float y)
			{
				if (x < 0.5)
					return 2.0*x*y;
				else
					return 1.0 - 2.0*(1.0 - x)*(1.0 - y);
			}
			float nm ( float2 uv , float _r, float _s ){
					 float2 texel = _r/_ScreenParams;					
				 float2 n  = float2(0.0, texel.y);
    float2 e  = float2(texel.x, 0.0);
    float2 s  = float2(0.0, -texel.y);
    float2 w  = float2(-texel.x, 0.0);
    float d   = tex2D(_texf,  uv).y;
    float d_n  = tex2D(_texf,  frac(uv+n)  ).y;
    float d_e  = tex2D(_texf,  frac(uv+e)  ).y;
    float d_s  = tex2D(_texf,  frac(uv+s)  ).y;
    float d_w  = tex2D(_texf,  frac(uv+w)  ).y; 
    float d_ne = tex2D(_texf,  frac(uv+n+e)).y;
    float d_se = tex2D(_texf,  frac(uv+s+e)).y;
    float d_sw = tex2D(_texf,  frac(uv+s+w)).y;
    float d_nw = tex2D(_texf,  frac(uv+n+w)).y; 
    float dxn[3];
    float dyn[3];
    float dcn[3];   
    dcn[0] = 0.5;
    dcn[1] = 1.0; 
    dcn[2] = 0.5;
    dyn[0] = d_nw - d_sw;
    dyn[1] = d_n  - d_s; 
    dyn[2] = d_ne - d_se;
    dxn[0] = d_ne - d_nw; 
    dxn[1] = d_e  - d_w; 
    dxn[2] = d_se - d_sw; 
	float den = 0.0;
    float3 avd = float3(0.,0.,0.);
	float ma = d+d_n+d_e+d_s+d_w+d_ne+d_se+d_sw+d_nw/9.;
    for(int i = 0; i < 3; i++) {
        for(int j = 0; j < 3; j++) {
            float2 dxy = float2(dxn[i], dyn[j]);
            float w = dcn[i] * dcn[j];
            float3 bn = reflect(normalize(float3(_s*dxy, -1.0)), float3(0,1,0));
			//float3 bn2 = reflect(normalize(float3(_s2*dxy, -1.0)), float3(0,1,0));
            avd += w * bn;
			
            den += w;
        }
    }
	 float4 ba =  float4(avd /= den,ma);
	 return  max(abs(ba.x),abs(ba.y));
} 
			fixed4 frag (v2f i) : SV_Target
			{
	
	
                float c = tex2D(_texf, i.uv).x;
				float3 cs =lerp(float3(0.14,0.14,0.28),float3(1.,1.,1.),smoothstep(0.5,1.,c));
				float bt = lerp(lerp(noise(i.uv*float2(1920/1080,1.)*120.+_Time.x*40.+c),hash(i.uv),0.5),0.5,0.8);
				float3 col = lerp(float3(1.,0.1,0.14),float3(ov(cs.x,bt),ov(cs.y,bt),ov(cs.z,bt)),step(0.6,c));
				float n = smoothstep(-0.1,0.9,nm(i.uv,0.3,40.));
				float3 fin = lerp (float3(c,c,c),col,step(0.9,_t));
				float3 fin2 = lerp(fin, float3(n,n,n),step(0.9,rd(_Time.x*100.+78.236)));			
				//return float4 (tex2D(_texf, i.uv).a, tex2D(_texf, i.uv).a, tex2D(_texf, i.uv).a,1.);
				return float4 (fin2, 1.);
			}
			ENDCG
		}
	}
}
