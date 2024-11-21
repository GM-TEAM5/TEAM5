Shader "Custom/AbilityArea"
{
    Properties
    {
        _Radius ("Outer Radius", Float) = 0.5 // 바깥 원 반경
        _InnerThickness ("Inner Thickness", Float) = 1.0 // 외곽선 안쪽 두께 (픽셀 단위)
        _InnerThreshold ("Inner Threshold", Float) = 0.9
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
            float _InnerThreshold;


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

                // Define the threshold for the inner region (90% of _Radius) 
                float innerThreshold = _InnerThreshold * _Radius;

                // Calculate screen size and pixel size for thickness scaling
                float2 screenSize = float2(_ScreenParams.x, _ScreenParams.y); // Screen dimensions
                float pixelSize = 1.0 /  max(screenSize.x, screenSize.y); // Pixel size based on vertical resolution
                float scaledInnerThickness = _InnerThickness * pixelSize; // Scale thickness to normalized units

                if (innerThreshold - scaledInnerThickness < dist  && dist <= innerThreshold) 
                {
                    // float innerFactor = (dist - (_Radius - scaledInnerThickness)) / scaledInnerThickness; // 안쪽 비율 (0~1)
                    // float alpha = innerFactor * color.a; // 알파값 적용
                    
                    float innerFactor = smoothstep(
                        innerThreshold - scaledInnerThickness,
                        innerThreshold,
                        dist
                    );



                    return fixed4(color.rgb, innerFactor * color.a);
                    // return fixed4(color.rgb, alpha); // 알파값이 최종 출력에 반영
                }
                else if (innerThreshold < dist && dist <= _Radius)
                {
                    float transitionFactor = smoothstep(
                        innerThreshold,
                        _Radius,
                        dist
                    );
                    return fixed4(color.rgb, (1.0 - transitionFactor) * color.a);
                    
                    
                    // Transition region: Apply smooth alpha gradient

                    // Calculate how far into the transition region the fragment is
                    // float transitionFactor = (dist - innerThreshold) / ( _Radius - innerThreshold );

                    // Optionally, you can use smoothstep for a smoother transition
                    // float smoothFactor = smoothstep(0.0, 1.0, transitionFactor);

                    // Calculate alpha based on the transition factor
                    // float alpha = (1.0 - transitionFactor) * color.a;

                    // Optionally, you can modulate alpha with the inner thickness
                    // float alpha = (1.0 - transitionFactor) * (1.0 - transitionFactor) * color.a;

                    // return fixed4(color.rgb, alpha);
                }
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
