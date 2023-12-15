// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/NiuQu_UV_Dissolve_Alpha02"
{
    Properties
    {
        _V ("V", Float) = 0.2
        _Niuqu ("Niuqu", 2D) = "white" {}
        _Tex ("Tex", 2D) = "white" {}
        _Niuqu_U ("Niuqu_U", Float) = 1
        _Niuqu_V ("Niuqu_V", Float) = 1
        _Tex_U ("Tex_U", Float) = 1
        _Tex_V ("Tex_V", Float) = 1
        _Tex_Color ("Tex_Color", Color) = (1, 1, 1, 1)
        _Tex_Scale ("Tex_Scale", Float) = 0
        _Mask ("Mask", 2D) = "white" {}
        _DissolveTex1 ("DissolveTex", 2D) = "white" {}
        _R_V1 ("R_V", Float) = 0
        _Dissolve_U ("Dissolve_U", Float) = 1
        _Dissolve_V ("Dissolve_V", Float) = 1
        [HideInInspector] _texcoord ("", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert 

        struct Input
        {
            float2 uv_Niuqu;
            float2 uv_Tex;
            float2 uv_Mask;
            float2 uv_DissolveTex1;
            float3 worldPos;
        };

        sampler2D _Niuqu;
        float4 _Niuqu_ST;
        sampler2D _Tex;
        float4 _Tex_ST;
        float _Niuqu_U;
        float _Niuqu_V;
        float _V;
        float _Tex_U;
        float _Tex_V;
        float4 _Tex_Color;
        sampler2D _Mask;
        float4 _Mask_ST;
        float _Tex_Scale;
        sampler2D _DissolveTex1;
        float4 _DissolveTex1_ST;
        float _Dissolve_U;
        float _Dissolve_V;
        float _R_V1;

  void surf(Input IN, inout SurfaceOutput o)
        {
            float2 appendResult9 = float2((_Niuqu_U * _Time.y), (_Time.y * _Niuqu_V));
            float2 uv_Niuqu = IN.uv_Niuqu * _Niuqu_ST.xy + _Niuqu_ST.zw + appendResult9;

            float2 appendResult29 = float2((_Tex_U * _Time.y), (_Time.y * _Tex_V));
            float2 uv_Tex = IN.uv_Tex * _Tex_ST.xy + _Tex_ST.zw + appendResult29;

            float2 uv_Mask = IN.uv_Mask * _Mask_ST.xy + _Mask_ST.zw;
            float4 tex2DNode31 = tex2D(_Mask, uv_Mask);

            float2 appendResult52 = float2((_Dissolve_U * _Time.y), (_Time.y * _Dissolve_V));
            float2 uv_DissolveTex1 = IN.uv_DissolveTex1 * _DissolveTex1_ST.xy + _DissolveTex1_ST.zw + appendResult52;

          float4 uv057 = float4(IN.uv_DissolveTex1.xy * float2(1, 1) + float2(0, 0), 0, 0);

            float lerpResult58 = lerp(_R_V1, -1.5, uv057.z);
            float clampResult60 = clamp(((tex2D(_DissolveTex1, uv_DissolveTex1).r * _R_V1) - lerpResult58), 0.0, 1.0);

            float4 tex2DNode17 = tex2D(_Tex, ((tex2D(_Niuqu, uv_Niuqu).r * _V) + uv_Tex));

            float4 appendResult46 = float4((tex2DNode17 * _Tex_Color * tex2DNode31 * _Tex_Scale).rgb, (tex2DNode17.a * clampResult60));

            o.Albedo = appendResult46.rgb;
            o.Alpha = appendResult46.a;
        }
        ENDCG
    }
    CustomEditor "ASEMaterialInspector"
}


