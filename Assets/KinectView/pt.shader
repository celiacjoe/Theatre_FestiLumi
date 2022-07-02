Shader "Unlit/pt"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_f1 ("_f1", Vector) = (0,0,0,0)
		_f2 ("_f2", Vector) = (0,0,0,0)
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
			#include "ca.cginc"
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
			float4 _f1 ;
			float4 _f2 ;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
			float2 uv = i.uv;
			float2 zo = invBilinear (uv,_f2.xy,_f2.zw,_f1.zw,_f1.xy);
                fixed4 col = smoothstep(1.,0.,tex2D(_MainTex,uv).r);
    float2 g = float2(0.,0.); float vtot=0.;
	for (int j=0; j< 16; j++)
	  for (int k=0; k< 16; k++)
	  {
		  float2 pos = (.5+float2(k,j))/16.;
		  float v = smoothstep(0.4,0.3,tex2D(_MainTex,pos).r);
		  v = clamp(2.*(v-.5),0.,1.);
		  v = pow(v,2.);
		  g += pos*v;
		  vtot += v;
	  }
				float pt = smoothstep(0.01,0.009,distance(uv,g/vtot));
                return lerp(col,float4(1.,0.,0.,1.),pt);
				//return float4 (zo,0.,1.);
            }
            ENDCG
        }
    }
}
