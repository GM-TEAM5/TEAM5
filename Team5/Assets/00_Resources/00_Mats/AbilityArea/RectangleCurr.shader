Shader "Custom/RectangleFillGradientOnly"
{
    Properties
    {
        _FillColor ("Fill Color", Color) = (1, 1, 1, 1) // 그라데이션 색상
        _FillValue ("Fill Value", Range(0, 1)) = 0.5 // 채워지는 양
        _GradientThickness ("Gradient Thickness", Range(0, 1)) = 0.1 // 그라데이션 두께
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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _FillValue;
            float _GradientThickness;
            float4 _FillColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 채워지는 진행도를 설정
                float fillProgress = saturate(_FillValue);

                // 그라데이션의 시작과 끝 계산
                float edge1 = fillProgress;
                float edge0 = max(fillProgress - _GradientThickness, 0.0);

                // 그라데이션 영역 확인
                float isInGradient = step(edge0, uv.x) * step(uv.x, edge1);

                // 그라데이션 값 계산
                float gradient = saturate((uv.x - edge0) / (edge1 - edge0));

                // 알파 값 적용: 그라데이션 영역에만 표시
                float alpha = gradient * isInGradient;

                // 최종 색상 설정
                fixed4 fillColor = _FillColor;
                fillColor.a *= alpha;

                return fillColor;
            }
            ENDCG
        }
    }
}
