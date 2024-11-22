Shader "Custom/RectangleOutlineConstantThickness"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineThickness ("Outline Thickness (pixels)", Float) = 2.0
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
                float4 screenPos : TEXCOORD1;
            };

            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 화면 공간에서의 UV 변화량 계산
                float2 uv_dx = ddx(i.uv);
                float2 uv_dy = ddy(i.uv);
                float2 uv_fwidth = abs(uv_dx) + abs(uv_dy);

                // 픽셀 단위의 외곽선 두께를 UV 공간으로 변환
                float pixelSize = fwidth(i.screenPos.x);
                float outlineThicknessUV = (_OutlineThickness / pixelSize) * uv_fwidth.x;

                // UV 좌표를 기준으로 사각형 외곽선 계산
                float2 uv = i.uv;

                // 외곽선 마스크 계산
                float minUV = outlineThicknessUV;
                float maxUV = 1.0 - outlineThicknessUV;
                float mask = step(uv.x, minUV) + step(maxUV, uv.x) + step(uv.y, minUV) + step(maxUV, uv.y);
                mask = saturate(mask);

                // 외곽선 색상 적용
                fixed4 color = _OutlineColor;
                color.a *= mask;

                return color;
            }
            ENDCG
        }
    }
}
