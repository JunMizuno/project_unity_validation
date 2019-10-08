Shader "Unlit/SlidingUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            float4 _MainTex_ST;
            fixed4 _Color;
            half _Alpha;

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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // @memo. 以下は不規則な点滅のパターン
                /*
                if (abs(_SinTime.w) < 0.2f)
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下は左半分カットのパターン
                /*
                if (uv.x < 0.5)
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下は左右起点でスライドを繰り返すパターン
                // @memo. 等符号を反転させれば起点が変わる
                /*
                if (uv.x > abs(_SinTime.w))
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下は上下起点でスライドを繰り返すパターン
                // @memo. 等符号を反転させれば起点が変わる
                /*
                if (uv.y > abs(_SinTime.w))
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下は上下起点でスライドするパターン
                // @memo. 等符号を反転させれば起点が変わる
                // @memo. fracは小数値の小数部分を返す
                // @memo. _Time.yは等倍
                /*
                if (uv.x > frac(_Time.y / 2.0f))
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下、一定の範囲のみスライドして表示するパターン
                // @memo. 範囲の値を0.1以上にすると不自然になる
                /*
                float targetValue = frac(_Time.y / 2.0f);
                if ((uv.x > targetValue) || (uv.x < targetValue - 0.08f))
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下、十時の切り抜きのパターン
                /*
                if ((uv.x < 0.45f || uv.x > 0.55f) && (uv.y < 0.45f || uv.y > 0.55f))
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下、十時でスライドするパターン
                /*
                float targetValue = frac(_Time.y / 2.0f);
                if ((uv.x < 0.0f + targetValue || uv.x > 0.1f + targetValue) && (uv.y < 0.0f + targetValue || uv.y > 0.1f + targetValue))
                {
                    return fixed4(0, 0, 0, 0);
                }
                */

                // @memo. 以下、たすき掛けで表示を消すパターン
                // @memo. 左下から右上にかけて消す、数値を大きくすると消す範囲が広がる
                // @memo. 符号を逆転させるとたすき部分のみ表示させるように変わる
                /*
                if (abs(uv.x - uv.y) < 0.15f)
                {
                    return fixed4(0, 0, 0, 0);
                }
                */
                
                // @memo. 以下、たすき掛けで表示を消すパターン
                // @memo. 左上から右下にかけて消す、1.0をベースに左の条件条件値と右の条件値を設定する
                // @memo. 符号を逆転させて&&を||に変えるとたすき部分のみ表示させるように変わる
                if (abs(uv.x + uv.y) > 0.85f && abs(uv.x + uv.y) < 1.15f)
                {
                    return fixed4(0, 0, 0, 0);
                }

                fixed4 col = tex2D(_MainTex, uv);
                col *= _Color;
                
                col.a *= _Alpha;
                // @memo. 以下は正常点滅
                // @memo. _SinTime.wは等倍
                //col.a *= abs(_SinTime.w);

                return col;
            }

            ENDCG
        }
    }

    //FallBack "Diffuse"
}
