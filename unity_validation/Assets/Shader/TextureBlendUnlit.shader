Shader "Unlit/TextureBlendUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SubTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Alpha ("Alpha", Range(1.0, 0.0)) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
        ZTest LEqual
        ZWrite On

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _SubTex;
            float4 _MainTex_ST;
            float4 _SubTex_ST;
            fixed4 _Color;
            half _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 main_uv : TEXCOORD0;
                float2 sub_uv : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 main_uv : TEXCOORD0;
                float2 sub_uv : TEXCOORD1;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.main_uv = TRANSFORM_TEX(v.main_uv, _MainTex);
                o.sub_uv = TRANSFORM_TEX(v.sub_uv, _SubTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, i.main_uv);
                fixed4 subCol = tex2D(_SubTex, i.sub_uv);
                //fixed4 fixedCol = mainCol * subCol;
                fixed4 fixedCol = fixed4(mainCol.r * (1.0f - subCol.r), mainCol.g * (1.0f - subCol.g), mainCol.b * (1.0f - subCol.b), mainCol.a * (1.0f - subCol.a));
                return fixedCol;
            }

            ENDCG
        }
    }

    //FallBack "Diffuse"
}
