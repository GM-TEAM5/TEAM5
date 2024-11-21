Shader "Custom/CircularFillWithInnerOutline"
{
    Properties
    {
        _Radius ("Outer Radius", Float) = 0.5 // 바깥 원 반경
        _InnerThickness ("Inner Thickness", Float) = 1.0 // 외곽선 안쪽 두께 (픽셀 단위)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" } 
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;   // SpriteRenderer에서 전달된 색상
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // 클립 공간 좌표
                fixed4 color : COLOR;     // Vertex에서 전달받은 색상
                float2 uv : TEXCOORD0;    // 텍스처 좌표
            };

            float _Radius;
            float _InnerThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // 클립 공간으로 변환
                o.uv = v.uv;
                o.color = v.color; // Vertex에서 Fragment로 색상 전달
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert UV coordinates to center-based coordinates
                float2 uv = i.uv - 0.5;
                float dist = length(uv); // Calculate distance from center
                fixed4 color = i.color; // Use the color passed from the vertex

                // Calculate screen size and pixel size for thickness scaling
                float2 screenSize = float2(_ScreenParams.x, _ScreenParams.y); // Screen dimensions
                float pixelSize = 1.0 / screenSize.y; // Pixel size based on vertical resolution
                float scaledThickness = _InnerThickness * pixelSize; // Scale thickness to normalized units

                // Define the inner boundary of the outline
                float innerBoundary = _Radius - scaledThickness;

                // Check if the fragment is within the outline range
                if (dist > innerBoundary && dist <= _Radius)
                {
                    // Render with full opacity
                    return fixed4(color.rgb, color.a);
                }
                else
                {
                    // Outside the outline: Discard the fragment
                    discard;
                }

                return color;
            }
            ENDCG
        }
    }
}
