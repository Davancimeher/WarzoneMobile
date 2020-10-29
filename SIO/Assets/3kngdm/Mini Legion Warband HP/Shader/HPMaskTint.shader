// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HPMaskTint"
{
	Properties
	{
		_BaseColor("BaseColor", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Color01("Color01", Color) = (0,0,0,0)
		_Brightness("Brightness", Float) = 2
		_EmissionPower("EmissionPower", Float) = 1.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _BaseColor;
		uniform float4 _BaseColor_ST;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float4 _Color01;
		uniform float _Brightness;
		uniform float _EmissionPower;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _Color0 = float4(0,0,0,0);
			o.Albedo = _Color0.rgb;
			float2 uv_BaseColor = i.uv_texcoord * _BaseColor_ST.xy + _BaseColor_ST.zw;
			float4 tex2DNode16 = tex2D( _BaseColor, uv_BaseColor );
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode13 = tex2D( _Mask, uv_Mask );
			float4 temp_cast_1 = (tex2DNode13.r).xxxx;
			float4 blendOpSrc22 = tex2DNode16;
			float4 blendOpDest22 = min( temp_cast_1 , _Color01 );
			float4 lerpResult4 = lerp( tex2DNode16 , ( ( saturate( ( blendOpSrc22 * blendOpDest22 ) )) * _Brightness ) , tex2DNode13.r);
			o.Emission = ( lerpResult4 * _EmissionPower ).rgb;
			o.Specular = _Color0.rgb;
			o.Smoothness = 0.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
7;29;1906;1004;1344.91;582.8403;1.223254;True;True
Node;AmplifyShaderEditor.SamplerNode;13;-1584.08,-201.1143;Float;True;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-1533.454,67.83549;Float;False;Property;_Color01;Color01;2;0;Create;True;0;0;False;0;0,0,0,0;0.07552986,0.4010895,0.9338235,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;15;-1187.421,-22.27275;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;16;-1320.287,-492.1947;Float;True;Property;_BaseColor;BaseColor;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;22;-700.0234,-91.18038;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-691.5071,118.5692;Float;False;Property;_Brightness;Brightness;3;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-435.5347,11.95664;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;4;-321.1234,-394.204;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-628.3837,265.6878;Float;False;Property;_EmissionPower;EmissionPower;4;0;Create;True;0;0;False;0;1.2;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-210.0141,217.6653;Float;False;Constant;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-23.27925,405.6642;Float;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0,0,0,0;0.07552986,0.4010895,0.9338235,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;6.607246,-311.3464;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;380.389,-256.9344;Float;False;True;2;Float;ASEMaterialInspector;0;0;StandardSpecular;HPMaskTint;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;13;1
WireConnection;15;1;9;0
WireConnection;22;0;16;0
WireConnection;22;1;15;0
WireConnection;23;0;22;0
WireConnection;23;1;24;0
WireConnection;4;0;16;0
WireConnection;4;1;23;0
WireConnection;4;2;13;1
WireConnection;30;0;4;0
WireConnection;30;1;26;0
WireConnection;0;0;29;0
WireConnection;0;2;30;0
WireConnection;0;3;29;0
WireConnection;0;4;2;0
ASEEND*/
//CHKSM=9825FFD53509A07EC7D9236C423E232C53CB2FF6