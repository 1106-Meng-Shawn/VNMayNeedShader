Shader "NV/SpeedLines"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _LineColor("Line Color", Color) = (1,1,1,1)
        _Intensity("Line Intensity", Range(0,5)) = 1.0
        _Direction("Direction", Vector) = (1,0,0,0)
        _Speed("Speed", Float) = 1.0
        _Density("Density", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _LineColor;
            float _Intensity;
            float2 _Direction;
            float _Speed;
            float _Density;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // ?? Unity ?? _Time
                float line = frac(dot(uv, normalize(_Direction)) * _Density - _Time.y * _Speed);

                // ?????
                float alpha = smoothstep(0.0, 0.05, 1.0 - line) * _Intensity;

                // ????????????
                fixed4 col = tex2D(_MainTex, uv) * (1 - alpha) + _LineColor * alpha;
                col.a = alpha; // ???????
                return col;
            }
            ENDCG
        }
    }
}
