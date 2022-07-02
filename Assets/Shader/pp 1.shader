Shader "Unlit/pp1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_texf ("_texf", 2D) = "white" {}
		_tex2 ("_tex2", 2D) = "white" {}
		_r ("_r",Float) = 0
		_rb ("_rb",Float) = 0
		_s ("_s",Float) = 0
		_s2 ("_s2",Float) = 0
		_r1 ("_r1",Float) = 0
		_r2 ("_r2",Float) = 0
		_r3 ("_r3",Float) = 0
		_r4 ("_r4",Float) = 0
		_r5 ("_r5",Float) = 0
		//_texf2 ("_texf2", 2D) = "white" {}
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
			sampler2D _texf;
			float4 _texf_ST;
			sampler2D _tex2;
			float4 _tex2_ST;
			float _r;
			float _rb;
			float _s;
			float _s2;
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
			float3 nb(float3 n1, float3 n2)
{

	n1 += float3(0, 0, 1);
	n2 *= float3(-1, -1, 1);

    return n1*dot(n1, n2)/n1.z - n2;
}
float ov(float x, float y)
{
    if (x < 0.5)
        return 2.0*x*y;
    else
        return 1.0 - 2.0*(1.0 - x)*(1.0 - y);
}
float4 nm ( float2 uv , float _r, float _s ){
					 float2 texel = _r/_ScreenParams;					
				 float2 n  = float2(0.0, texel.y);
    float2 e  = float2(texel.x, 0.0);
    float2 s  = float2(0.0, -texel.y);
    float2 w  = float2(-texel.x, 0.0);
    float d   = tex2D(_texf,  uv).a;
    float d_n  = tex2D(_texf,  frac(uv+n)  ).a;
    float d_e  = tex2D(_texf,  frac(uv+e)  ).a;
    float d_s  = tex2D(_texf,  frac(uv+s)  ).a;
    float d_w  = tex2D(_texf,  frac(uv+w)  ).a; 
    float d_ne = tex2D(_texf,  frac(uv+n+e)).a;
    float d_se = tex2D(_texf,  frac(uv+s+e)).a;
    float d_sw = tex2D(_texf,  frac(uv+s+w)).a;
    float d_nw = tex2D(_texf,  frac(uv+n+w)).a; 
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
			float3 bn2 = reflect(normalize(float3(_s2*dxy, -1.0)), float3(0,1,0));
            avd += w * bn;
			
            den += w;
        }
    }

   return  float4(avd /= den,ma);
} 
float G1V(float dnv, float k){
    return 1.0/(dnv*(1.0-k)+k);
}
float ggx(float3 n, float3 v, float3 l, float rough, float f0){
    float alpha = rough*rough;
    float3 h = normalize(v+l);
    float dnl = clamp(dot(n,l), 0.0, 1.0);
    float dnv = clamp(dot(n,v), 0.0, 1.0);
    float dnh = clamp(dot(n,h), 0.0, 1.0);
    float dlh = clamp(dot(l,h), 0.0, 1.0);
    float f, d, vis;
    float asqr = alpha*alpha;
    const float pi = 3.14159;
    float den = dnh*dnh*(asqr-1.0)+1.0;
    d = asqr/(pi * den * den);
    dlh = pow(1.0-dlh, 5.0);
    f = f0 + (1.0-f0)*dlh;
    float k = alpha/1.0;
    vis = G1V(dnl, k)*G1V(dnv, k);
    float spec = dnl * d * f * vis;
    return spec;
}
fixed4 frag (v2f i) : SV_Target
			{
					
				 float2 uv = i.uv;

	//avd2 /= den;
	float4 avd = nm(uv,_r,_s);
	float4 avd2 = nm(uv,_rb,_s2);
	float3 avd3 = normalize(avd.xyz+avd2.xyz);
	float3 sp = float3(uv-0.5, 0);
    float3 light = float3(cos(_Time.x/2.0)*0.5, sin(_Time.x/2.0)*0.5, -8.0);
    float3 ld = light - sp;
    float lDist = max(length(ld), 0.001);
    ld /= lDist;
	float spec = 0.;
	float mask = clamp(max(1.-avd2.a,avd.a),0.,1.);
	spec += ggx(avd3, float3(0,1,0), ld, _r4, _r5);
	//float3 resul =max(float3 (max(spec,mask),max(spec,mask),max(spec,mask)),float3(0.5,0.,0.));
	//float fn = (spec*max(mask,_r2));
	float fn = spec;
	float3 col = sin(spec * float3(0.0, 1.0, 0.0) * 40.0 * _r1) * 0.5 + 0.5;  
    col += sin(spec * float3(1.0, 0.0, 0.0) * 40.0 *_r2) * 0.5 + 0.5;  
    col += sin(spec * float3(0.0, 0.0, 1.0) * 40.0 *_r3) * 0.5 + 0.5;
    col = clamp(normalize(col), 0.0, 1.0);
	float3 ovf = float3 (ov(spec,col.x),ov(spec,col.y),ov(spec,col.z));
	return float4(ovf,1.);
			}
			ENDCG
		}
	}
}
