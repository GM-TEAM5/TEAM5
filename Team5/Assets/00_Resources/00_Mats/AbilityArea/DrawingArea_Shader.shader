Shader "Custom/DrawingArea"
{
    Properties
    {
        _Radius ("Outer Radius", Float) = 0.5
        _InnerThickness ("Inner Thickness", Float) = 1.0
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
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            float _Radius;
            float _InnerThickness;
            float _InnerThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5;
                float dist = length(uv);

                fixed4 color = i.color;

                float innerThreshold = _InnerThreshold * _Radius;

                float2 screenSize = float2(_ScreenParams.x, _ScreenParams.y);
                float pixelSize = 1.0 / max(screenSize.x, screenSize.y);
                float scaledInnerThickness = _InnerThickness * pixelSize;

                // Inner ring
                if ( innerThreshold - scaledInnerThickness < dist && dist <= innerThreshold) 
                {
                    float innerFactor = smoothstep(
                        innerThreshold - scaledInnerThickness,
                        innerThreshold,
                        dist
                    );

                    // float innerFactor = lerp(innerThreshold - scaledInnerThickness,
                    //     innerThreshold,
                    //     dist
                    // ); 

                    // float normalizedDist = (dist - (innerThreshold - scaledInnerThickness)) 
                    //    / (innerThreshold - (innerThreshold - scaledInnerThickness));
                    // float innerFactor = lerp(0.0, 1.0, normalizedDist);

                    return fixed4(color.rgb, innerFactor * color.a);
                }

                // Outer transition
                else if (innerThreshold < dist && dist <= _Radius)
                {
                    float transitionFactor = smoothstep(
                        innerThreshold,
                        _Radius,
                        dist
                    );
                    return fixed4(color.rgb, (1.0 - transitionFactor) * color.a);
                }

                // Outside area
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
