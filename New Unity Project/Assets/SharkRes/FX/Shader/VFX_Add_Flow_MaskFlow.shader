// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/VFX_Add_Flow_MaskFlow"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        _ColorPower("ColorPower", Float) = 2
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _Vector0("Vector 0", Vector) = (1,1,0,0)
        _Flow_Speed_X("Flow_Speed_X", Float) = 0
        _Flow_Speed_Y("Flow_Speed_Y", Float) = 0
        _Mask("Mask", 2D) = "white" {}
        [HideInInspector] _texcoord( "", 2D ) = "white" {}
    }

    SubShader
    {


        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        LOD 100

        CGINCLUDE
        #pragma target 3.0
        ENDCG
        Blend One One
        Cull Off
        ColorMask RGBA
        ZWrite Off
        ZTest LEqual



        Pass
        {
            Name "Unlit"

            CGPROGRAM
            #ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
		//only defining to not throw compilation error over Unity 5.5
		#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
            #endif
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "UnityShaderVariables.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 ase_texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
                float4 ase_texcoord : TEXCOORD0;
                float4 ase_color : COLOR;
            };

            uniform sampler2D _Mask;
            uniform float4 _Mask_ST;
            uniform half _ColorPower;
            uniform half4 _MainColor;
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform half4 _Vector0;
            uniform half _Flow_Speed_X;
            uniform half _Flow_Speed_Y;
            uniform half _IsClip;
            uniform half4 _Area;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.ase_texcoord.xy = v.ase_texcoord.xy;
                o.ase_color = v.color;

                //setting value to unused interpolator channels and avoid initialization warnings
                o.ase_texcoord.zw = 0;
                float3 vertexValue = float3(0, 0, 0);
                #if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
                #else
                v.vertex.xyz += vertexValue;
                #endif
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                fixed4 finalColor;
                float2 uv_Mask = i.ase_texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
                float2 uv0_MainTex = i.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                float2 appendResult23 = (float2(_Vector0.x, _Vector0.y));
                float mulTime42 = _Time.y * _Flow_Speed_X;
                float mulTime27 = _Time.y * _Flow_Speed_Y;
                float2 appendResult25 = (float2((_Vector0.w + (1.0 - mulTime42)), (_Vector0.z + (1.0 - mulTime27))));
                float4 tex2DNode2 = tex2D(_MainTex, (uv0_MainTex * appendResult23 + appendResult25));


                finalColor = (tex2D(_Mask, uv_Mask) * ((_ColorPower * _MainColor) * tex2DNode2 * i.ase_color * (
                    _MainColor.a * tex2DNode2.a * i.ase_color.a)));
                bool inArea = i.vertex.x >= _Area.x && i.vertex.x <= _Area.z;
                if (inArea == false && _IsClip == 1)
                {
                    finalColor = fixed4(0, 0, 0, 0);
                };
                return finalColor;
            }
            ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"


}
/*ASEBEGIN
Version=16800
423;234;1725;1030;1896.279;352.9005;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;41;-1554.636,544.126;Half;False;Property;_Flow_Speed_X;Flow_Speed_X;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1550.703,438.2115;Half;False;Property;_Flow_Speed_Y;Flow_Speed_Y;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;42;-1221.673,504.9369;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;27;-1251.74,363.0223;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-1057.528,499.2357;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;31;-1074.595,360.3212;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;21;-1409.945,80.3028;Half;False;Property;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;1,1,0,0;0.25,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-858.0916,336.9896;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-840.2791,476.0995;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-659.9638,248.5251;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1335.523,-218;Float;True;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;23;-660.2367,79.6173;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;-503.2521,-162.6837;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-13.34038,215.3755;Float;True;Property;_MainTex;MainTex;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;d54910b4ccf03e34797816ac0b4cdc25;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-5.020996,-526.6621;Half;False;Property;_ColorPower;ColorPower;1;0;Create;True;0;0;False;0;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;5;76.6595,20.13176;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-18.29471,-362.3501;Half;False;Property;_MainColor;MainColor;0;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;486.7012,79.93274;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;235.979,-424.6621;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;515.8763,-574.1837;Float;True;Property;_Mask;Mask;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;593.6283,-227.427;Float;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;854.117,-435.7314;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;17;435.0189,332.5138;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1151.25,-217.1366;Half;False;True;2;Half;ASEMaterialInspector;0;1;VFX/VFX_Add_Flow_MaskFlow;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;False;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;42;0;41;0
WireConnection;27;0;29;0
WireConnection;43;0;42;0
WireConnection;31;0;27;0
WireConnection;30;0;21;3
WireConnection;30;1;31;0
WireConnection;44;0;21;4
WireConnection;44;1;43;0
WireConnection;25;0;44;0
WireConnection;25;1;30;0
WireConnection;23;0;21;1
WireConnection;23;1;21;2
WireConnection;24;0;6;0
WireConnection;24;1;23;0
WireConnection;24;2;25;0
WireConnection;2;1;24;0
WireConnection;12;0;1;4
WireConnection;12;1;2;4
WireConnection;12;2;5;4
WireConnection;40;0;39;0
WireConnection;40;1;1;0
WireConnection;3;0;40;0
WireConnection;3;1;2;0
WireConnection;3;2;5;0
WireConnection;3;3;12;0
WireConnection;36;0;38;0
WireConnection;36;1;3;0
WireConnection;0;0;36;0
ASEEND*/
//CHKSM=20E0C8DB5BA80D3F2AD3ECD600285E4F2FF19E7B