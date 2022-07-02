Shader "Unlit/pp"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_texf ("_texf", 2D) = "white" {}
		_texf2 ("_texf2", 2D) = "white" {}
		_LUT("LUT", 2D) = "white" {}
		_r1 ("_r1", Float) = 0
		_r2 ("_r2", Float) = 0
		_r3 ("_r3", Float) = 0
		_r4 ("_r4", Float) = 0
		_r5 ("_r5", Float) = 0
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
			sampler2D _texf2;
			float4 _tex2f_ST;
			sampler2D _LUT;
            float4 _LUT_TexelSize;
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
			float ov(float x, float y)
{
    if (x < 0.5)
        return 2.0*x*y;
    else
        return 1.0 - 2.0*(1.0 - x)*(1.0 - y);
}
float3 ov3 (float3 a , float3 b){
	return float3 (ov(a.x,b.x),ov(a.y,b.y),ov(a.z,b.z));
}
float rd (float t) {return frac(sin(dot(floor(t),12.45))*4597.268);}
float no (float t) {return lerp(rd(t),rd(t+1.0),smoothstep(0.,1.,frac(t)));}
float4 nm ( float2 uv , float _r, float _s ){
					 float2 texel = _r/_ScreenParams;					
				 float2 n  = float2(0.0, texel.y);
    float2 e  = float2(texel.x, 0.0);
    float2 s  = float2(0.0, -texel.y);
    float2 w  = float2(-texel.x, 0.0);
    float d   = tex2D(_texf2,  uv).y;
    float d_n  = tex2D(_texf2,  frac(uv+n)  ).y;
    float d_e  = tex2D(_texf2,  frac(uv+e)  ).y;
    float d_s  = tex2D(_texf2,  frac(uv+s)  ).y;
    float d_w  = tex2D(_texf2,  frac(uv+w)  ).y; 
    float d_ne = tex2D(_texf2,  frac(uv+n+e)).y;
    float d_se = tex2D(_texf2,  frac(uv+s+e)).y;
    float d_sw = tex2D(_texf2,  frac(uv+s+w)).y;
    float d_nw = tex2D(_texf2,  frac(uv+n+w)).y; 
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
	 return  float4(avd /= den,ma);
} 
			fixed4 frag (v2f i) : SV_Target
			{
	
	float2 texcoord = i.uv;
	float maxColor = COLORS - 1.0;
                float3 col = saturate(tex2D(_texf, i.uv)).xyz*saturate(tex2D(_texf, i.uv)).a;

	float c2 =  smoothstep(_r3,_r4,dot(col,float3(0.3,0.59,0.11)));
	float3 nor = nm( texcoord,_r1,_r2);
	float ft = (abs(nor.x)+abs(nor.y));
	float3 hu = saturate(3.0*abs(1.0-2.0*frac(ft+float3(0.0,-1.0/3.0,1.0/3.0)))-1);
	float3 fin = hu*ft;
	
                
                float halfColX = 0.5 / _LUT_TexelSize.z;
                float halfColY = 0.5 / _LUT_TexelSize.w;
                float threshold = maxColor / COLORS;
 
                float xOffset = halfColX + fin.r * threshold / COLORS;
                float yOffset = halfColY + fin.g * threshold;
                float cell = floor(fin.b * maxColor);
 
                float2 lutPos = float2(cell / COLORS + xOffset, yOffset);
                float3 fr = tex2D(_LUT, lutPos).xyz;
				float t = tex2D(_texf2,  texcoord).y;
				float nb = lerp(c2,1.-c2,t);
				float3 fr3 = float3 (nb,nb,nb);
				float3 resu = lerp(fr,fr3,step(0.9,rd(_Time.x*1000.)));
				return float4(fr,1.);
				
			}
			ENDCG
		}
	}
}
