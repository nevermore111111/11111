// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/NiuQu_UV_Alpha"
{
	Properties
	{
		_V("V", Float) = 0.2
		_Niuqu("Niuqu", 2D) = "white" {}
		_Tex("Tex", 2D) = "white" {}
		_Niuqu_U("Niuqu_U", Float) = 1
		_Niuqu_V("Niuqu_V", Float) = 1
		_Tex_U("Tex_U", Float) = 1
		_Tex_V("Tex_V", Float) = 1
		_Tex_Color("Tex_Color", Color) = (1,1,1,1)
		_Tex_Scale("Tex_Scale", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
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
#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
			};

			uniform sampler2D _Tex;
			uniform sampler2D _Niuqu;
			uniform float4 _Niuqu_ST;
			uniform float _Niuqu_U;
			uniform float _Niuqu_V;
			uniform float _V;
			uniform float4 _Tex_ST;
			uniform float _Tex_U;
			uniform float _Tex_V;
			uniform float4 _Tex_Color;
			uniform float _Tex_Scale;
			uniform sampler2D _Mask;
			uniform float4 _Mask_ST;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
#endif
				float2 uv0_Niuqu = i.ase_texcoord1.xy * _Niuqu_ST.xy + _Niuqu_ST.zw;
				float2 appendResult9 = (float2(( _Niuqu_U * _Time.y ) , ( _Time.y * _Niuqu_V )));
				float2 uv0_Tex = i.ase_texcoord1.xy * _Tex_ST.xy + _Tex_ST.zw;
				float2 appendResult29 = (float2(( _Tex_U * _Time.y ) , ( _Time.y * _Tex_V )));
				float2 uv_Mask = i.ase_texcoord1.xy * _Mask_ST.xy + _Mask_ST.zw;
				
				
				finalColor = ( tex2D( _Tex, ( ( tex2D( _Niuqu, ( uv0_Niuqu + appendResult9 ) ).r * _V ) + ( uv0_Tex + appendResult29 ) ) ) * _Tex_Color * i.ase_color * _Tex_Scale * tex2D( _Mask, uv_Mask ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18100
0;24;2560;1355;1127.993;91.0676;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1585.194,-42.93573;Float;False;Property;_Niuqu_U;Niuqu_U;3;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;5;-1618.31,115.5844;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-1588.035,314.634;Float;False;Property;_Niuqu_V;Niuqu_V;4;0;Create;True;0;0;False;0;False;1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1295.072,222.9789;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1294.02,31.35297;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1180.998,-144.6987;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;9;-1079.998,107.3013;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1044.417,466.8342;Float;False;Property;_Tex_U;Tex_U;5;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1047.258,824.4039;Float;False;Property;_Tex_V;Tex_V;6;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;24;-1077.533,625.3542;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-753.2427,541.1229;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-881.6573,12.83917;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-754.2947,732.7488;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-640.2207,365.0712;Inherit;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;29;-539.2207,617.0712;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-540.4012,249.104;Inherit;False;Property;_V;V;0;0;Create;True;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-689.5364,-15.86792;Inherit;True;Property;_Niuqu;Niuqu;1;0;Create;True;0;0;False;0;False;-1;None;104be724d3fd4944cbddc4a027091251;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-340.8801,522.609;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-255.4012,96.104;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;6.598755,168.104;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;216.9302,138.8349;Inherit;True;Property;_Tex;Tex;2;0;Create;True;0;0;False;0;False;-1;None;20dd799a44a4d6541b56f1686c78e495;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;193.7463,844.9701;Inherit;True;Property;_Mask;Mask;9;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;35;325.746,1090.47;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;306.2406,335.0068;Inherit;False;Property;_Tex_Color;Tex_Color;7;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;352.0073,539.9324;Inherit;False;Property;_Tex_Scale;Tex_Scale;8;0;Create;True;0;0;False;0;False;0;2.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;637.1039,266.1909;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;22;824.9779,275.6119;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/NiuQu_UV_Alpha;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;7;0;5;2
WireConnection;7;1;3;0
WireConnection;6;0;4;0
WireConnection;6;1;5;2
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;27;0;25;0
WireConnection;27;1;24;2
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;28;0;24;2
WireConnection;28;1;26;0
WireConnection;29;0;27;0
WireConnection;29;1;28;0
WireConnection;2;1;10;0
WireConnection;30;0;23;0
WireConnection;30;1;29;0
WireConnection;11;0;2;1
WireConnection;11;1;12;0
WireConnection;13;0;11;0
WireConnection;13;1;30;0
WireConnection;17;1;13;0
WireConnection;18;0;17;0
WireConnection;18;1;20;0
WireConnection;18;2;35;0
WireConnection;18;3;36;0
WireConnection;18;4;31;0
WireConnection;22;0;18;0
ASEEND*/
//CHKSM=95794400BFDE8C67DBA731FEC04CE2F0469FE9EA