Shader /*ase_name*/ "NoeTemplate/URPImageEffect" /*end*/
{
    Properties
    {
        /*ase_props*/
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off
        
        /*ase_pass*/
        
        Pass
        {
            Name "ColorBlitPass"
            
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            #pragma vertex myVert
            #pragma fragment frag
            /*ase_pragma*/

            sampler2D _CameraOpaqueTexture;

            struct appdata
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                /*ase_vdata:p=p;uv0=tc0.xy;uv1=tc1.xy*/
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
                /*ase_interp(1,7):sp=sp.xyzw;uv0=tc0.xy;uv1=tc0.zw*/
            };

            /*ase_globals*/

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

            half4 frag (v2f i /*ase_frag_input*/) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // ase common template code
				/*ase_frag_code:i=v2f*/

                
                float4 color = /*ase_frag_out:Frag Color;Float4*/float4(1,0,0,1)/*end*/;
                return color;
            }
            ENDHLSL
        }
    }
}
