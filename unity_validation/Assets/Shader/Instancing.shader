Shader "Custom/Instancing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        // @memo. 複数パスなどにおける共通設定を記述する為のもの(ENDCGまでの間に記述したものが対象)
        CGINCLUDE

        #include "UnityCG.cginc"
        // @memo. Instancing系マクロはUnityInstancing.cgincで定義されている
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        //float4 _Color;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            // @memo. GPU Instancing用に追加
            UNITY_VERTEX_INPUT_INSTANCE_ID    
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            // @memo. 座標系のインスタンスを使用する例のために追加
            float4 worldPos : TEXCOORD1;
            UNITY_FOG_COORDS(1)
            // @memo. GPU Instancing用に追加
            // @memo. フラグメントシェーダーで座標系の値を使用する場合に記述必須
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        v2f vert(appdata v)
        {
             v2f o;
             // @memo. GPU Instancing用に追加
             // @memo. これが無いと、各インスタンス毎のモデル行列が区別されない
             UNITY_SETUP_INSTANCE_ID(v);
             // @memo. GPU Instancing用に追加
             // @memo. フラグメントシェーダーにてGPU Instancing有効時に座標系の値を使用する場合はこのマクロが必要
             UNITY_TRANSFER_INSTANCE_ID(v, o);
             o.pos = UnityObjectToClipPos(v.vertex);
             o.uv = TRANSFORM_TEX(v.uv, _MainTex);
             // @memo. 座標系のインスタンスを使用する例のために追加
             o.worldPos = mul(unity_ObjectToWorld, v.vertex);
             UNITY_TRANSFER_FOG(o, o.vertex);
             return o;   
        }

        // @memo. スクリプト側から値を取る場合の記述例
        // @memo. この場合では_Colorという変数を定義していることになる
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
        UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 frag(v2f i) : SV_Target
        {
             // @memo. GPU Instancing用に追加
             // @memo. これが無いと、各インスタンス毎のモデル行列が区別されない
             UNITY_SETUP_INSTANCE_ID(i);
             fixed4 col = tex2D(_MainTex, i.uv);

             // @memo. 外部変数値を使用する例
             //col *= UNITY_ACCESS_INSTANCED_PROP(Props, _Color);

             UNITY_APPLY_FOG(i.fogCoord, col);
             return col;

             // @memo. 座標系のインスタンスを使用する例のために追加
             //return mul(unity_WorldToObject, i.worldPos);
        }

        ENDCG

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            // @memo. GPU Instancing用に追加
            // @memo. マテリアルのインスペクターにGPU Instancingが追加される
            #pragma multi_compile_instancing
        
            ENDCG
        }

    }

    FallBack "Diffuse"
}
