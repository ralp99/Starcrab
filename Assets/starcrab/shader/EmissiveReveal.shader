Shader "EmissiveReveal"
{
	Properties 
	{
_diffuse("_diffuse", 2D) = "black" {}
_rangeSliderX("_rangeSliderX", Range(0.1,1) ) = 1
_rangeSliderY("_rangeSliderY", Range(0,1) ) = 0.706
_emissiveReveal("_emissiveReveal", 2D) = "black" {}
_emissiveCover("_emissiveCover", 2D) = "black" {}
_EmissiveColor("_EmissiveColor", Color) = (1,1,1,1)
_ColorFilter("_ColorFilter", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Back
ZWrite Off
ZTest LEqual
ColorMask RGBA
Blend SrcAlpha OneMinusSrcAlpha
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


sampler2D _diffuse;
float _rangeSliderX;
float _rangeSliderY;
sampler2D _emissiveReveal;
sampler2D _emissiveCover;
float4 _EmissiveColor;
float4 _ColorFilter;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec) * s.Alpha;
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_diffuse;
float2 uv_emissiveReveal;
float2 uv_emissiveCover;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D3=tex2D(_diffuse,(IN.uv_diffuse.xyxy).xy);
float4 Multiply4=_ColorFilter * Tex2D3;
float4 Tex2D2=tex2D(_emissiveReveal,(IN.uv_emissiveReveal.xyxy).xy);
float4 Split0=(IN.uv_emissiveCover.xyxy);
float4 Multiply0=float4( Split0.x, Split0.x, Split0.x, Split0.x) * _rangeSliderX.xxxx;
float4 Multiply1=float4( Split0.y, Split0.y, Split0.y, Split0.y) * _rangeSliderY.xxxx;
float4 Assemble0_3_NoInput = float4(0,0,0,0);
float4 Assemble0=float4(Multiply0.x, Multiply1.y, float4( Split0.z, Split0.z, Split0.z, Split0.z).z, Assemble0_3_NoInput.w);
float4 UV_Pan0=float4(Assemble0.x + float4( 1.0, 1.0, 1.0, 1.0 ).x,Assemble0.y,Assemble0.z,Assemble0.w);
float4 Tex2D1=tex2D(_emissiveCover,UV_Pan0.xy);
float4 Multiply2=Tex2D2 * Tex2D1;
float4 Multiply3=Multiply2 * _EmissiveColor;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply4;
o.Emission = Multiply3;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
