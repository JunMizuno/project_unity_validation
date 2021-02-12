Shader "Unlit/Rectangle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [MaterialToggle] _Aspect ("Aspect Ratio", Float) = 0
        [MaterialToggle] _MovingVector ("Moving Vector", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _Aspect;

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
            
            v2f vert (appdata v)
            {
                v2f o;
                float4 vertex = v.vertex;
                float2 uv = v.uv;

                //vertex *= (1.0f + abs(_SinTime.w));
                uv.x += _Time.y % 1.0f;

                o.vertex = UnityObjectToClipPos(vertex);
                o.uv = TRANSFORM_TEX(uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int aspect = _Aspect;
                float2 uv = i.uv;




                //float ratio = _ScreenParams.x / _ScreenParams.y;                
                //uv.x *= (ratio * aspect + (1.0 - aspect));




                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }

            ENDCG
        }
    }

    //FallBack "Diffuse"
}
