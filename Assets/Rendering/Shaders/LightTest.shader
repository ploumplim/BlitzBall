Shader "Unlit/LightTest"
{
    Properties
    {
        [MainTexture] _BaseMap ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        struct Attributes
        {
            float4 positionOS   : POSITION;
            float2 uv : TEXCOORD0;
            half3 normal : NORMAL;
        };

        struct Varyings
        {
            float4 positionHCS  : SV_POSITION;
            float2 uv : TEXCOORD0;
            half3 normal : TEXCOORD1;
        };
        ENDHLSL
        
        Pass
        {
            Name "DepthNormalsPass"
            Tags { "LightMode" = "DepthNormals" }
            
            // Write depth to the depth buffer
            ZWrite On

            // Don't write to the color buffer
            ColorMask 0
            
            HLSLPROGRAM
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                //OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ForwardPass"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM 
            #pragma vertex vert
            #pragma fragment frag

            CBUFFER_START(UnityPerMaterial)
            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float4 _LightColor, _LightDir, _LightPos;;
            sampler2D _LightTextureB0;
            float3 ambientCol;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            // UnityLight CreateLight(float2 uv, float3 worldPos, float viewZ)
            // {
            //     UnityLight light;
            //     light.dir = -_LightDir;
            //     float attenuation = 1;
            //     float3 shadowAttenuation = 1;
            //     bool shadowed = false;
            //
            //     //float3 col = step(0.1,_LightColor);
            //     #if defined(DIRECTIONAL) || defined(DIRECTIONAL_COOKIE)
            //     
            //         light.dir = -_LightDir;
            //         #if defined(DIRECTIONAL_COOKIE)
            //             float2 uvCookie = mul(unity_WorldToLight, float4(worldPos, 1)).xy;
            //             attenuation *= tex2Dbias(_LightTexture0, float4(uvCookie, 0, -8)).w;
            //         #endif
            //         
            //         #if defined(SHADOWS_SCREEN)
            //             shadowed = true;
            //             shadowAttenuation = tex2D(_ShadowMapTexture, uv).r;
            //         #endif
            //     
            //     #else
            //     
            //         float3 lightVec = _LightPos.xyz - worldPos;
            //         light.dir = normalize(lightVec);
            //
            //     
            //         attenuation *= tex2D(_LightTextureB0,(dot(lightVec, lightVec) * _LightPos.w).rr).UNITY_ATTEN_CHANNEL;
            //     
            //         #if defined(SPOT)
            //     
            //             float4 uvCookie = mul(unity_WorldToLight, float4(worldPos, 1));
            //             uvCookie.xy /= uvCookie.w;
            //             
            //             attenuation *= tex2Dbias(_LightTexture0, float4(uvCookie.xy, 0, -8)).w;
            //             attenuation *= uvCookie.w < 0;
            //
            //             #if defined(SHADOWS_DEPTH)
            //     
            //                 shadowed = true;
            //                 shadowAttenuation = UnitySampleShadowmap(mul(unity_WorldToShadow[0], float4(worldPos, 1)));
            //     
            //             #endif
            //     
            //         #else
            //     
            //             #if defined(POINT_COOKIE)
            //     
            //                 float3 uvCookie = mul(unity_WorldToLight, float4(worldPos, 1)).xyz;
            //     
            //                 attenuation *= texCUBEbias(_LightTexture0, float4(uvCookie, -8)).w;
            //     
            //             #endif
            //     
            //             #if defined(SHADOWS_CUBE)
            //     
            //                 shadowed = true;
            //                 shadowAttenuation = UnitySampleShadowmap(-lightVec);
            //
            //             #endif
            //         #endif
            //     #endif
            //
            //     if (shadowed)
            //     {
            //         float shadowFadeDistance = UnityComputeShadowFadeDistance(worldPos, viewZ);
            //         float shadowFade = UnityComputeShadowFade(shadowFadeDistance);
            //         shadowAttenuation = saturate(shadowAttenuation + shadowFade);
            //
            //         #if defined(UNITY_FAST_COHERENT_DYNAMIC_BRANCHING) && defined(SHADOWS_SOFT)
            //             UNITY_BRANCH
            //             if (shadowFade > 0.99)
            //             {
            //                 shadowAttenuation = 1;
            //             }
            //         #endif
            //     }
            //     
            //     light.color = _LightColor.rgb * (attenuation * shadowAttenuation);
            //     // light.color = _LightColor.rgb * (attenuation);
            //     light.color = lerp(ambientCol * light.color,light.color,shadowAttenuation);
            //     return light;
            // }
            
            half4 frag() : SV_Target
            {
                // Defining the color variable and returning it.
                half4 customColor = half4(0.5, 0, 0, 1);
                return customColor;
            }
            ENDHLSL
        }
    }
}
