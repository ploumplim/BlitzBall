// Made with Amplify Shader Editor v1.9.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PostProcess/BlitTest"
{
    Properties
    {
        [HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
        [HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
       LOD 100
        ZWrite Off
        
        
        
        Pass
        {
            Name "ColorBlitPass"
            
            HLSLPROGRAM
            #define ASE_VERSION 19900
            #define ASE_SRP_VERSION 170004
            #define ASE_USING_SAMPLING_MACROS 1

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            #pragma vertex myVert
            #pragma fragment frag
            #define ASE_NEEDS_TEXTURE_COORDINATES0
            #define ASE_NEEDS_FRAG_TEXTURE_COORDINATES0


            sampler2D _CameraOpaqueTexture;

            struct appdata
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
                
            };

            float4 Unity_Universal_SampleBuffer_BlitSource_float1_g15( float2 uv )
            {
            	uint2 pixelCoords = uint2(uv * _ScreenSize.xy);
            	return LOAD_TEXTURE2D_X_LOD(_BlitTexture, pixelCoords, 0);
            }
            


            v2f myVert(appdata v)
            {
                v2f output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                float4 pos = GetFullScreenTriangleVertexPosition(v.vertexID);
                float2 uv  = GetFullScreenTriangleTexCoord(v.vertexID);

                output.positionCS = pos;
                output.texcoord   = DYNAMIC_SCALING_APPLY_SCALEBIAS(uv);

                return output;
            }

            half4 frag (v2f i ) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // ase common template code
				float2 texCoord8 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv1_g15 = texCoord8;
				float4 localUnity_Universal_SampleBuffer_BlitSource_float1_g15 = Unity_Universal_SampleBuffer_BlitSource_float1_g15( uv1_g15 );
				float4 break13 = localUnity_Universal_SampleBuffer_BlitSource_float1_g15;
				float4 appendResult12 = (float4(( break13.x + 0.2 ) , break13.y , break13.z , break13.w));
				

                
                float4 color = appendResult12;
                return color;
            }
            ENDHLSL
        }
    }
	
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
	Fallback Off
}
/*ASEBEGIN
Version=19900
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;8;-336,272;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;30;-96,272;Inherit;False;URP Sample;-1;;15;5acefc1201f98724da3578d3ed4e7769;0;1;9;FLOAT2;0,0;False;6;FLOAT4;0;FLOAT3;2;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8
Node;AmplifyShaderEditor.BreakToComponentsNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;13;80,272;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;10;272,208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;12;432,240;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;0;640,240;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;100;14;PostProcess/BlitTest;c136ef60ecd6a9e4e9feb8f511c03c61;True;ColorBlitPass;0;0;ColorBlitPass;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;2;RenderType=Opaque=RenderType;RenderPipeline=UniversalPipeline;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;0;1;True;False;;True;0
WireConnection;30;9;8;0
WireConnection;13;0;30;0
WireConnection;10;0;13;0
WireConnection;12;0;10;0
WireConnection;12;1;13;1
WireConnection;12;2;13;2
WireConnection;12;3;13;3
WireConnection;0;0;12;0
ASEEND*/
//CHKSM=7594641DC65CA5E9D5D759DEF3FB4911E476DBC9