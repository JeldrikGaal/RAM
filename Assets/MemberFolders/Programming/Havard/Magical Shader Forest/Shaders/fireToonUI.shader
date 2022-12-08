// Made with Amplify Shader Editor v1.9.0.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "James/ToonFireUI"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_fireDetail("fireDetail", Range( 0 , 20)) = 10
		_Speed("Speed", Float) = -0.15
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		_speedUp("speedUp", Range( -0.3 , 0)) = -0.14
		_firePower("firePower", Range( 0 , 1.5)) = 0.5
		_fireTreshold("fireTreshold", Range( -1 , 1)) = -0.5
		_noiseTiling("noiseTiling", Range( 0 , 1)) = 0.25
		[HDR]_Colourtop("Colour top", Color) = (0.1811172,0.04245281,1,1)
		[HDR]_Colourbottom("Colour bottom", Color) = (0,1,0.06287718,1)
		_Power("Power", Float) = 1

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp]
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend Off
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _Colourtop;
			uniform float4 _Colourbottom;
			uniform float2 _Tiling;
			uniform float2 _Offset;
			uniform float _Speed;
			uniform float _fireDetail;
			uniform float _noiseTiling;
			uniform float _speedUp;
			uniform float _firePower;
			uniform float _Power;
			uniform float _fireTreshold;
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash2( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi2( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash2( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord5 = IN.texcoord.xy * _Tiling + _Offset;
				float2 temp_cast_0 = (( _Speed * _Time.y )).xx;
				float2 texCoord14 = IN.texcoord.xy * float2( 1,1 ) + temp_cast_0;
				float simplePerlin2D1 = snoise( texCoord14*_fireDetail );
				simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
				float time2 = 2.0;
				float2 voronoiSmoothId2 = 0;
				float2 temp_cast_1 = (_noiseTiling).xx;
				float4 appendResult30 = (float4(0.0 , ( _Time.y * _speedUp ) , 0.0 , 0.0));
				float2 texCoord21 = IN.texcoord.xy * temp_cast_1 + appendResult30.xy;
				float2 temp_cast_3 = (_noiseTiling).xx;
				float2 texCoord27 = IN.texcoord.xy * temp_cast_3 + appendResult30.xy;
				float simplePerlin2D22 = snoise( texCoord27*_fireDetail );
				simplePerlin2D22 = simplePerlin2D22*0.5 + 0.5;
				float2 temp_cast_5 = (simplePerlin2D22).xx;
				float2 lerpResult23 = lerp( texCoord21 , temp_cast_5 , float2( 0.5,0.5 ));
				float2 coords2 = lerpResult23 * _fireDetail;
				float2 id2 = 0;
				float2 uv2 = 0;
				float voroi2 = voronoi2( coords2, time2, id2, uv2, 0, voronoiSmoothId2 );
				float2 temp_cast_6 = (( simplePerlin2D1 * voroi2 )).xx;
				float4 appendResult33 = (float4(0.0 , _firePower , 0.0 , 0.0));
				float2 lerpResult4 = lerp( texCoord5 , temp_cast_6 , appendResult33.xy);
				float4 tex2DNode7 = tex2D( _MainTex, lerpResult4 );
				float4 temp_cast_8 = (_Power).xxxx;
				float4 fireOutput47 = pow( tex2DNode7 , temp_cast_8 );
				float4 lerpResult56 = lerp( _Colourtop , _Colourbottom , fireOutput47);
				float4 dualColour116 = lerpResult56;
				float grayscale100 = Luminance(( tex2DNode7 * tex2DNode7.a ).rgb);
				float2 texCoord36 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult44 = clamp( ( ( ( 1.0 - sqrt( texCoord36 ).y ) + _fireTreshold ) * 2.0 ) , 0.0 , 1.0 );
				float clampResult49 = clamp( ( grayscale100 - clampResult44 ) , 0.0 , 1.0 );
				float fire68 = clampResult49;
				
				half4 color = ( dualColour116 * fire68 );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19002
2556.667;-471.3333;1088;829.6667;8637.438;4596.457;10.17788;True;False
Node;AmplifyShaderEditor.CommentaryNode;57;-4409.72,-1486.872;Inherit;False;3709.782;1871.214;Comment;33;47;7;4;5;3;33;20;2;1;19;34;23;14;17;22;21;26;18;27;30;25;9;29;31;16;32;100;101;131;133;145;164;165;fireEffect;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-4352.72,-137.9089;Inherit;False;Property;_speedUp;speedUp;4;0;Create;True;0;0;0;False;0;False;-0.14;-0.5;-0.3;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;16;-4323.536,-867.5835;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-3898.141,-490.7683;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-3955.31,-240.4887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2868.727,-620.6727;Inherit;False;Property;_fireDetail;fireDetail;0;0;Create;True;0;0;0;False;0;False;10;10;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-3986.041,90.81342;Inherit;False;Property;_noiseTiling;noiseTiling;7;0;Create;True;0;0;0;False;0;False;0.25;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-3676.141,-250.917;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;25;-2678.25,-386.5149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-3392.31,-94.48875;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;26;-3092.249,-369.5149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3463.246,-1077.481;Inherit;False;Property;_Speed;Speed;1;0;Create;True;0;0;0;False;0;False;-0.15;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3388.541,-376.4032;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-3249.246,-893.4813;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;22;-3126.527,-213.7165;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;71;-651.3488,-1208.616;Inherit;False;2489.55;796.4709;Comment;12;36;37;38;39;40;68;49;45;44;42;41;168;fireFinished;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2955.247,-983.4813;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-2847.778,-300.2827;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-612.2108,-850.8735;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;20;-2552.811,-1140.872;Inherit;False;Property;_Offset;Offset;3;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.VoronoiNode;2;-2553.105,-545.9428;Inherit;False;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;10;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;34;-2546.657,-248.2976;Inherit;False;Property;_firePower;firePower;5;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-2535.811,-1436.872;Inherit;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-2608.041,-791.2933;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-2340.225,-593.257;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2249.303,-970.3363;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2245.984,-311.0871;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SqrtOpNode;37;-379.2245,-857.1717;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;4;-2005.389,-655.3363;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;133;-2002.734,-849.6106;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;38;-179.979,-873.6661;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;7;-1753.81,-746.0146;Inherit;True;Property;_Fire;Fire;1;0;Create;True;0;0;0;False;0;False;-1;7cb90f33f0fe626499384f6a36a5527e;ca28a6baf78616145ab7c03ffb54833e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;39;83.26228,-870.7997;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;46.3256,-645.8012;Inherit;False;Property;_fireTreshold;fireTreshold;6;0;Create;True;0;0;0;False;0;False;-0.5;-0.5;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-1415.974,-502.3272;Inherit;False;Property;_Power;Power;11;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;317.3681,-826.0071;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-1394.933,-1047.898;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;100;-1142.906,-1052.309;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;580.3676,-816.0071;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;164;-1230.955,-693.8296;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;168;756.7313,-972.9733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-940.0534,-797.6769;Inherit;False;fireOutput;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;58;-193.9972,-2545.531;Inherit;False;1609.302;859.5372;Comment;15;85;84;83;86;115;112;73;114;113;72;56;54;55;116;159;dualColours;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;44;845.2653,-832.1722;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;45;1107.256,-912.7922;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;478.7245,-2269.845;Inherit;False;Property;_Colourbottom;Colour bottom;9;1;[HDR];Create;True;0;0;0;False;0;False;0,1,0.06287718,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;495.7265,-2483.124;Inherit;False;Property;_Colourtop;Colour top;8;1;[HDR];Create;True;0;0;0;False;0;False;0.1811172,0.04245281,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;72;196.644,-2495.929;Inherit;False;47;fireOutput;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;49;1372.221,-890.3051;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;56;805.2877,-2114.931;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;1119.344,-2116.604;Inherit;False;dualColour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;1614.204,-874.5351;Inherit;False;fire;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-365.239,-3.661928;Inherit;False;68;fire;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;117;-367.76,-206.6921;Inherit;False;116;dualColour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-123.536,-23.05074;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;159;512.2326,-2029.548;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-57.47895,-2018.136;Inherit;False;Constant;_Float5;Float 5;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;113;417.9497,-2291.634;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;86;235.5451,-1968.092;Inherit;False;Constant;_Float6;Float 6;13;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-57.0763,-2492.443;Inherit;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-54.45823,-2258.077;Inherit;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;0;False;0;False;1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;114;177.9497,-2271.634;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;73;193.2083,-2202.726;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0.8,0,0,0;False;2;COLOR;0.8,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;115;181.9497,-2009.634;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;112;391.9497,-1997.634;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;145;-1969.342,-381.1976;Inherit;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;146;208.5818,-14.18465;Float;False;True;-1;2;ASEMaterialInspector;0;6;James/ToonFireUI;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;0;5;False;;10;False;;0;5;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;7;True;_StencilComp;1;True;_StencilOp;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;29;0;16;0
WireConnection;29;1;32;0
WireConnection;30;0;31;0
WireConnection;30;1;29;0
WireConnection;25;0;9;0
WireConnection;27;0;101;0
WireConnection;27;1;30;0
WireConnection;26;0;25;0
WireConnection;21;0;101;0
WireConnection;21;1;30;0
WireConnection;17;0;18;0
WireConnection;17;1;16;0
WireConnection;22;0;27;0
WireConnection;22;1;26;0
WireConnection;14;1;17;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;2;0;23;0
WireConnection;2;2;9;0
WireConnection;1;0;14;0
WireConnection;1;1;9;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;5;0;19;0
WireConnection;5;1;20;0
WireConnection;33;1;34;0
WireConnection;37;0;36;0
WireConnection;4;0;5;0
WireConnection;4;1;3;0
WireConnection;4;2;33;0
WireConnection;38;0;37;0
WireConnection;7;0;133;0
WireConnection;7;1;4;0
WireConnection;39;0;38;1
WireConnection;41;0;39;0
WireConnection;41;1;40;0
WireConnection;131;0;7;0
WireConnection;131;1;7;4
WireConnection;100;0;131;0
WireConnection;42;0;41;0
WireConnection;164;0;7;0
WireConnection;164;1;165;0
WireConnection;168;0;100;0
WireConnection;47;0;164;0
WireConnection;44;0;42;0
WireConnection;45;0;168;0
WireConnection;45;1;44;0
WireConnection;49;0;45;0
WireConnection;56;0;54;0
WireConnection;56;1;55;0
WireConnection;56;2;72;0
WireConnection;116;0;56;0
WireConnection;68;0;49;0
WireConnection;51;0;117;0
WireConnection;51;1;69;0
WireConnection;159;0;73;0
WireConnection;113;0;72;0
WireConnection;114;0;113;0
WireConnection;73;0;114;0
WireConnection;73;1;83;0
WireConnection;73;2;84;0
WireConnection;73;3;85;0
WireConnection;73;4;115;0
WireConnection;115;0;112;0
WireConnection;112;0;86;0
WireConnection;146;0;51;0
ASEEND*/
//CHKSM=3C06203DB8BCA4395475783916152A690D5F12B0