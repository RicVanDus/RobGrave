Shader "Unlit/S_Occluder"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            ZWrite On
            ColorMask 0
        }
    }
}