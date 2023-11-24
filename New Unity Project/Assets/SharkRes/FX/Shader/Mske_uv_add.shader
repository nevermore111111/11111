// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/add_uv"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Texture_U("Texture_U", Float) = 0
		_Texture_V("Texture_V", Float) = 0
		_Texture_Color("Texture_Color ", Color) = (1,1,1,1)
		_Mask("Mask", 2D) = "white" {}
		_glow("glow", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
			};

			uniform sampler2D _Texture;
			uniform half4 _Texture_ST;
			uniform float _Texture_U;
			uniform float _Texture_V;
			uniform half _glow;
			uniform float4 _Texture_Color;
			uniform sampler2D _Mask;
			uniform half4 _Mask_ST;

			
			v2f vert ( appdata v )
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
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				half2 uv0_Texture = i.ase_texcoord.xy * _Texture_ST.xy + _Texture_ST.zw;
				half2 appendResult10 = (half2(( _Texture_U * _Time.y ) , ( _Time.y * _Texture_V )));
				half4 tex2DNode2 = tex2D( _Texture, ( uv0_Texture + appendResult10 ) );
				float2 uv_Mask = i.ase_texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
				
				
				finalColor = ( ( tex2DNode2 * i.ase_color * i.ase_color.a * _glow * _Texture_Color * tex2DNode2.a * _Texture_Color.a ) * tex2D( _Mask, uv_Mask ).r );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17400
373;388;1996;991;1305.241;322.3804;1;True;False
Node;AmplifyShaderEditor.TimeNode;5;-1631.974,-20.37952;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-1600.974,-190.3795;Float;False;Property;_Texture_U;Texture_U;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1611.974,172.6205;Float;False;Property;_Texture_V;Texture_V;2;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1266.974,45.62048;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1270.974,-147.3795;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1039.974,-59.37952;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1095.974,-273.3795;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-793.9744,-126.3795;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-492.34,-298.4618;Inherit;False;Property;_glow;glow;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;18;-542.4325,297.4091;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-571.9744,-135.3795;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;-1;None;dc7902b9b1f57f64699fe399dfb4d11e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-496.8481,86.3236;Float;False;Property;_Texture_Color;Texture_Color ;3;0;Create;True;0;0;False;0;1,1,1,1;0.2039216,0.6941177,1,0.5686275;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;18.2065,51.25305;Inherit;False;7;7;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-236.4532,404.9363;Inherit;True;Property;_Mask;Mask;4;0;Create;True;0;0;False;0;-1;None;4a4403d5249a9624dbf494efb42a52f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;263.5468,222.9363;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;30;468,199;Half;False;True;-1;2;ASEMaterialInspector;100;1;VFX/add_uv;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;7;0;5;2
WireConnection;7;1;9;0
WireConnection;6;0;8;0
WireConnection;6;1;5;2
WireConnection;10;0;6;0
WireConnection;10;1;7;0
WireConnection;11;0;13;0
WireConnection;11;1;10;0
WireConnection;2;1;11;0
WireConnection;16;0;2;0
WireConnection;16;1;18;0
WireConnection;16;2;18;4
WireConnection;16;3;28;0
WireConnection;16;4;15;0
WireConnection;16;5;2;4
WireConnection;16;6;15;4
WireConnection;20;0;16;0
WireConnection;20;1;19;1
WireConnection;30;0;20;0
ASEEND*/
//CHKSM=B1D74D04C6204D1484DF5D3568420C0234DA5670