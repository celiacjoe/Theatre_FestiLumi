Shader "Unlit/dete"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_texf("_texf", 2D) = "white" {}
		_p1 ("_p1", Float) = 0
		_p2 ("_p2", Float) = 0
		_p3 ("_p3", Float) = 0
		_p4 ("_p4", Float) = 0
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
			float _p1;
			float _p2;
			float _p3;
			float _p4;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_texf, i.uv);
				float li = max(step(frac(i.uv.x*16.),0.05),step(frac(i.uv.y*8.),0.05));
				//float li2 =max(li, max(step(frac(i.uv.x*128.),0.08),step(frac(i.uv.y*64.),0.08)));
				float2 pos = float2 (tex2D(_texf, float2(0.,0.)).y,tex2D(_texf, float2(12.,0.)).y);
				float2 pos2 = float2 (tex2D(_texf, float2(20.,0.)).y,tex2D(_texf, float2(28.,0.)).y);
				float3 ba = lerp(float3(li,li,li),float3(1.,0.,0.),step(distance(i.uv,float2(_p1,_p2)),0.01));
				float3 ba2 = lerp(ba,float3(0.,0.,1.),step(distance(i.uv,float2(_p3,_p4)),0.01));
                return float4(ba2,1.);
            }
            ENDCG
        }
    }
}
