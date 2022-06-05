// https://forpro.unity3d.jp/unity_pro_tips/2019/08/19/187/

Shader "Custom/360Shader"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (.5, .5, .5, .5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        _MarginColor ("Margin Color", Color) = (.5, .3, 0, 0)

        _HorizontalRotation ("Horizontal Rotation", Range(0, 360)) = 0
        _VerticalRotation ("Vertical Rotation", Range(0, 360)) = 0

        _Horizontal ("Horizontal", Range(0, 360)) = 120
        _Vertical ("Vertical", Range(0, 180)) = 120

        [NoScaleOffset] _Tex ("Spherical  (HDR)", 2D) = "grey" {}
        [KeywordEnum(6 Frames Layout, Latitude Longitude Layout)] _Mapping("Mapping", Float) = 1
        [Enum(None, 0, Side by Side, 1, Over Under, 2)] _Layout("3D Layout", Float) = 0
        
        _DebugValue ("Debug Value", Color) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Background"
            "RenderType"="Background"
            "PreviewType"="Skybox"
        }

        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile __ _MAPPING_6_FRAMES_LAYOUT
            #pragma enable_d3d11_debug_symbols

            #include "UnityCG.cginc"

            sampler2D _Tex;
            float4 _Tex_TexelSize;
            half4 _Tex_HDR;
            half4 _Tint;
            half4 _MarginColor;
            half _Exposure;
            float _HorizontalRotation;
            float _VerticalRotation;
            float _Horizontal;
            float _Vertical;
            half4 _DebugValue;
            #ifndef _MAPPING_6_FRAMES_LAYOUT
            int _Layout;
            #endif

            #ifndef _MAPPING_6_FRAMES_LAYOUT
            inline float2 ToRadialCoords(float3 coords)
            {
                float3 normalizedCoords = normalize(coords);
                float latitude = acos(normalizedCoords.y);
                float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
                float2 sphereCoords = float2(longitude, latitude) * float2(0.5 / UNITY_PI, 1.0 / UNITY_PI);
                return float2(0.5, 1.0) - sphereCoords;
            }
            #endif

            float3 RotateAroundCenter(float3 vertex, float xDegrees, float yDegrees)
            {
                float x = xDegrees * UNITY_PI / 360.0;
                float y = yDegrees * UNITY_PI / 360.0;

                float sinX, cosX, sinY, cosY;
                
                sincos(x, sinX, cosX);
                sincos(y, sinY, cosY);

                float2x2 matrixX = float2x2(cosX, -sinX, sinX, cosX);
                float2x2 matrixY = float2x2(cosY, -sinY, sinY, cosY);
                float2 mulx = mul(matrixX, vertex.yz);
                float2 muly = mul(matrixY, vertex.xz);
                // return float3(vertex.x, mulx.x, mulx.y).xzy;
                return float3(mul(matrixY, vertex.xz), vertex.y).xzy;
            }

            
            
            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
                float4 layout3DScaleAndOffset : TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                float3 rotated = RotateAroundCenter(v.vertex, _VerticalRotation, _HorizontalRotation);
                
                o.vertex = UnityObjectToClipPos(rotated);
                o.texcoord = v.vertex.xyz;

                // // Calculate constant scale and offset for 3D layouts
                // if (_Layout == 0) // No 3D layout
                //     o.layout3DScaleAndOffset = float4(0, 0, 1, 1);
                // else if (_Layout == 1) // Side-by-Side 3D layout
                //     o.layout3DScaleAndOffset = float4(unity_StereoEyeIndex, 0, 0.5, 1);
                // else // Over-Under 3D layout
                //     o.layout3DScaleAndOffset = float4(0, 1 - unity_StereoEyeIndex, 1, 0.5);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 tc = ToRadialCoords(i.texcoord);

                // Cut around the vertical angles with center at 90 degrees
                if (_Vertical/360 < tc.y)
                    return _MarginColor;
                tc.y = fmod(tc.y * 360/_Vertical, 1);

                // Cut around the horizontal angles
                if (tc.x > _Horizontal/360)
                    return _MarginColor;
                tc.x = fmod(tc.x * 360/_Horizontal, 1);
                
                //tc = (tc + i.layout3DScaleAndOffset.xy) * i.layout3DScaleAndOffset.zw;

                half4 tex = tex2D(_Tex, tc);
                half3 c = DecodeHDR(tex, _Tex_HDR);
                c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
                c *= _Exposure;
                return half4(c, 1);
            }
            ENDCG
        }
    }

    Fallback Off
}