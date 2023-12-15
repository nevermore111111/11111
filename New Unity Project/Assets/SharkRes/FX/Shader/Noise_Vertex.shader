// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/Noise_Vertex"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Texture_V("Texture_V", Float) = 0
		_Texture_Color("Texture_Color", Color) = (0,0,0,0)
		_Tex_U("Tex_U", Float) = 1
		_Tex_V("Tex_V", Float) = 1
		_Niuqu("Niuqu", 2D) = "white" {}
		_Niuqu_V("Niuqu_V", Float) = 0.2
		_Niuqu_U("Niuqu_U", Float) = 1
		_Float0("Float 0", Float) = 1
		_Vertex("Vertex", 2D) = "white" {}
		_Vector("Vector", Vector) = (0,0,0,0)
		_Vertex_U("Vertex_U", Float) = 1
		_Vertex_V("Vertex_V", Float) = 1
		_Mask("Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcColor One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Vertex;
		uniform float4 _Vertex_ST;
		uniform float _Vertex_U;
		uniform float _Vertex_V;
		uniform half4 _Vector;
		uniform sampler2D _Texture;
		uniform sampler2D _Niuqu;
		uniform float _Niuqu_U;
		uniform float _Float0;
		uniform half _Niuqu_V;
		uniform float4 _Texture_ST;
		uniform float _Tex_U;
		uniform float _Tex_V;
		uniform half4 _Texture_Color;
		uniform half _Texture_V;
		uniform sampler2D _Mask;
		uniform half4 _Mask_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv0_Vertex = v.texcoord.xy * _Vertex_ST.xy + _Vertex_ST.zw;
			half2 appendResult9 = (half2(( _Vertex_U * _Time.y ) , ( _Time.y * _Vertex_V )));
			v.vertex.xyz += ( tex2Dlod( _Vertex, float4( ( uv0_Vertex + appendResult9 ), 0, 0.0) ) * _Vector ).rgb;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			half2 appendResult45 = (half2(( _Niuqu_U * _Time.y ) , ( _Time.y * _Float0 )));
			float2 uv0_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			half2 appendResult30 = (half2(( _Tex_U * _Time.y ) , ( _Time.y * _Tex_V )));
			half4 tex2DNode21 = tex2D( _Texture, ( ( tex2D( _Niuqu, ( i.uv_texcoord + appendResult45 ) ).r * _Niuqu_V ) + ( uv0_Texture + appendResult30 ) ) );
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			o.Emission = ( ( tex2DNode21.a * _Texture_Color * i.vertexColor * i.vertexColor.a * _Texture_V * tex2DNode21 ) * tex2D( _Mask, uv_Mask ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
2560;0;2560;1379;3159.109;1365.068;1.675811;True;True
Node;AmplifyShaderEditor.TimeNode;40;-2590.186,-909.7496;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-2559.91,-710.7;Float;False;Property;_Float0;Float 0;8;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2557.07,-1068.269;Float;False;Property;_Niuqu_U;Niuqu_U;7;0;Create;True;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-2265.896,-993.9811;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-2266.948,-802.3552;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2178.998,-47.51361;Float;False;Property;_Tex_V;Tex_V;4;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-2176.157,-405.0833;Float;False;Property;_Tex_U;Tex_U;3;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;27;-2209.273,-246.5634;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;45;-2051.873,-918.0328;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-2152.873,-1170.032;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1884.983,-330.7946;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1886.035,-139.1688;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1853.532,-1012.495;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TimeNode;6;-1824.2,305.9522;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-1771.961,-506.8464;Inherit;False;0;21;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-1513.276,-777.23;Inherit;False;Property;_Niuqu_V;Niuqu_V;6;0;Create;True;0;0;False;0;False;0.2;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-1670.961,-254.8463;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1791.084,147.4323;Float;False;Property;_Vertex_U;Vertex_U;11;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1793.925,505.002;Float;False;Property;_Vertex_V;Vertex_V;12;0;Create;True;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;47;-1661.411,-1041.202;Inherit;True;Property;_Niuqu;Niuqu;5;0;Create;True;0;0;False;0;False;-1;None;104be724d3fd4944cbddc4a027091251;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1499.91,221.721;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1227.276,-929.2301;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1472.62,-349.3085;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1500.962,413.3468;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-1285.888,297.6693;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1386.888,45.66926;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1051.47,-560.1882;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;21;-774.7384,-477.8185;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;False;-1;None;34d06db59ac904b4a93b8dd5f06fc729;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-1087.547,203.2071;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;36;-787.373,-79.01563;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-754.049,90.93951;Inherit;False;Property;_Texture_V;Texture_V;1;0;Create;True;0;0;False;0;False;0;1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;-786.373,-283.0156;Inherit;False;Property;_Texture_Color;Texture_Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-941.2999,215.5;Inherit;True;Property;_Vertex;Vertex;9;0;Create;True;0;0;False;0;False;-1;None;7f8ba80031ceb994fbb51ebc12f796b7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-388.373,-337.0156;Inherit;False;6;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;-438.373,17.98438;Inherit;True;Property;_Mask;Mask;13;0;Create;True;0;0;False;0;False;-1;None;57a0f6e5a2e0ff74797ce7655580daf6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;2;-821,538.5;Inherit;False;Property;_Vector;Vector;10;0;Create;True;0;0;False;0;False;0,0,0,0;0.1,0.15,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-359,245.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-155.373,-340.0156;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;236.9427,-379.8616;Half;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/Noise_Vertex;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;1;3;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;0;39;0
WireConnection;43;1;40;2
WireConnection;42;0;40;2
WireConnection;42;1;41;0
WireConnection;45;0;43;0
WireConnection;45;1;42;0
WireConnection;29;0;25;0
WireConnection;29;1;27;2
WireConnection;28;0;27;2
WireConnection;28;1;26;0
WireConnection;46;0;44;0
WireConnection;46;1;45;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;47;1;46;0
WireConnection;8;0;4;0
WireConnection;8;1;6;2
WireConnection;49;0;47;1
WireConnection;49;1;48;0
WireConnection;32;0;31;0
WireConnection;32;1;30;0
WireConnection;7;0;6;2
WireConnection;7;1;5;0
WireConnection;9;0;8;0
WireConnection;9;1;7;0
WireConnection;51;0;49;0
WireConnection;51;1;32;0
WireConnection;21;1;51;0
WireConnection;11;0;10;0
WireConnection;11;1;9;0
WireConnection;1;1;11;0
WireConnection;34;0;21;4
WireConnection;34;1;33;0
WireConnection;34;2;36;0
WireConnection;34;3;36;4
WireConnection;34;4;52;0
WireConnection;34;5;21;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;37;0;34;0
WireConnection;37;1;38;0
WireConnection;0;2;37;0
WireConnection;0;11;3;0
ASEEND*/
//CHKSM=7EEEDCF4C612E8C53A4CB1CF34544DAB73FF41E7